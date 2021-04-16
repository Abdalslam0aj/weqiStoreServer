using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using weqi_store_api.Configuration;
using weqi_store_api.Data;
using weqi_store_api.Models.DTOs;
using weqi_store_api.Models.DTOs.Requests;
using weqi_store_api.Models.DTOs.Responses;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace weqi_store_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly WeqiDbContext _weqiDbContext;
        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optrionsMonitor,
            TokenValidationParameters tokenParams,
            WeqiDbContext weqiDbContext)
        {
            _userManager = userManager;
            _jwtConfig = optrionsMonitor.CurrentValue;
            _tokenValidationParams = tokenParams;
            _weqiDbContext = weqiDbContext;


        }

        [HttpPost]
        [Route("Register")]
         public async Task<IActionResult> Register([FromBody] UserRegistrationDto user) {
            if (ModelState.IsValid) {
                
                var existingUser =  _userManager.Users.Where(userd =>String.Equals(userd.PhoneNumber, user.phoneNumber)).ToList();
                if (existingUser.Count > 0) {
                    return BadRequest(new RegistrationResponse() {
                        Errors = new List<string>() {
                            "user Allready Exists"
                        },
                        Result = false
                        }
                    );
                
                }

                var newUser = new IdentityUser() { PhoneNumber = user.phoneNumber, UserName = user.userName };
                var isCreated = await _userManager.CreateAsync(newUser,user.password);

                if (isCreated.Succeeded) {
                    var jwtToken = await GenerateJwtToken(newUser);
                    return Ok(jwtToken);


                }
                else {
                    BadRequest(new RegistrationResponse()
                    {
                        Errors =
                        isCreated.Errors.Select(x => x.Description).ToList(),
                        Result = false
                    });



                }
            }

            return BadRequest(new RegistrationResponse() {
                Errors = new List<string>() {
                    "invaild payload"
                },
                Result = false}); 
            
         
         }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest) {

            if (ModelState.IsValid) {
                var existingUser = _userManager.Users.Where(userd => String.Equals(userd.PhoneNumber, loginRequest.phoneNumber)).ToList();
                if (existingUser.Count <= 0)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Invalid authentication request "
                        },
                        Result = false
                    }
                    );

                }

                var user = existingUser[0];

                var isCorrect = await _userManager.CheckPasswordAsync(user, loginRequest.password);

                if (isCorrect)
                {
                    var jwtToken = await  GenerateJwtToken(user);
                    return Ok(jwtToken);
                }
                else {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                            "Invalid authentication request"
                        },
                        Result = false
                    }
                    );
                }

            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                            "Invalid authentication request"
                        },
                Result = false
            }
            );



        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var res = await VerifyToken(tokenRequest);

                if (res == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                    "Invalid tokens"
                },
                        Result = false
                    });
                }

                return Ok(res);
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                "Invalid payload"
            },
                Result = false
            });
        }

        private async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
               
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

               
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

             
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

               
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Result = false
                    };
                }
                

               
                var storedRefreshToken =  _weqiDbContext.RefreshTokens.FirstOrDefault(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Result = false
                    };
                }

                
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Result = false
                    };
                }

                
                if (storedRefreshToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been used" },
                        Result = false
                    };
                }

                
                if (storedRefreshToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been revoked" },
                        Result = false
                    };
                }

                
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                
                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Result = false
                    };
                }

                storedRefreshToken.IsUsed = true;
                _weqiDbContext.RefreshTokens.Update(storedRefreshToken);
                await _weqiDbContext.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

            private async Task<AuthResult> GenerateJwtToken(IdentityUser user) {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim("PhoneNumber", user.PhoneNumber),
                    new Claim(JwtRegisteredClaimNames.Sub,user.PhoneNumber),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                //TODO: change time to 5 - 10 min
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            
            
            };
            var token = JwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = JwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshTokens() {                
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid() 
            };

            await _weqiDbContext.RefreshTokens.AddAsync(refreshToken);
            await _weqiDbContext.SaveChangesAsync(); 

            return new AuthResult() {
                Token = jwtToken,
                Result = true,
                RefreshToken = refreshToken.Token,
            };      
        
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYNZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    } 
}
