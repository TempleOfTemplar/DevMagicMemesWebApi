using DevMagicMemesWebApi.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Identity
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtTokenOptions _jwtOptions;

        public AccountController(SignInManager<IdentityUser> signInManager, IOptions<JwtTokenOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("api/Account/Login")]
        public async Task<OperationResult<LoginResult>> LoginAsync([FromBody] LoginModel model)
        {
            var result = new OperationResult<LoginResult>();

            var user = await _signInManager.UserManager.FindByNameAsync(model.Email);

            if (user is null)
            {
                result.Message = Lang.IDENTITY_USERNAME_NOT_EXIST;

                return result;
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!signInResult.Succeeded)
            {
                result.Message = Lang.IDENTITY_INVALID_PASSWORD;

                return result;
            }

            var loginResult = new LoginResult();

            var claims = await _signInManager.UserManager.GetClaimsAsync(user);

            var expiresTime = DateTime.UtcNow.AddSeconds(loginResult.ExpireIn);

            var payload = new JwtPayload(_jwtOptions.Issuer, _jwtOptions.Audience, claims, null, expiresTime);

            var symmetricKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtOptions.SigningKey));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            loginResult.AccessToken = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(header, payload));

            result.Success = true;

            result.Data = loginResult;

            result.Code = StatusCodes.Status200OK;

            return result;
        }

        [AllowAnonymous]
        [HttpPost("api/Account/Register")]
        public async Task<OperationResult> RegisterAsync([FromBody] RegisterModel model)
        {
            var result = new OperationResult();

            var user = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            var identityResult = await _signInManager.UserManager.CreateAsync(user, model.Password);

            if (identityResult.Succeeded)
            {
                await _signInManager.UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));

                result.Success = true;

                result.Code = StatusCodes.Status200OK;
            }
            else
            {
                result.Message = String.Join(Environment.NewLine, identityResult.Errors.Select(x => x.Description));
            }

            return result;
        }

        [HttpPost("api/Account/ChangePassword")]
        public async Task<OperationResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var result = new OperationResult();

            var user = await _signInManager.UserManager.FindByNameAsync(model.Email);

            if (user is null)
            {
                result.Message = Lang.IDENTITY_USERNAME_NOT_EXIST;

                return result;
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.OldPassword, false);

            if (!signInResult.Succeeded)
            {
                result.Message = Lang.IDENTITY_INVALID_PASSWORD;

                return result;
            }

            var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            var identityResult = await _signInManager.UserManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (identityResult.Succeeded)
            {
                result.Success = true;

                result.Code = StatusCodes.Status200OK;
            }
            else
            {
                result.Message = String.Join(Environment.NewLine, identityResult.Errors.Select(x => x.Description));
            }

            return result;
        }
    }
}
