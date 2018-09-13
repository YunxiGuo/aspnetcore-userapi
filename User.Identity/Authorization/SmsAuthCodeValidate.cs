using IdentityServer4.Validation;
using System.Threading.Tasks;
using IdentityServer4.Models;
using User.Identity.Service;

namespace User.Identity.Authorization
{
    public class SmsAuthCodeValidate : IExtensionGrantValidator
    {
        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;

        public SmsAuthCodeValidate(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            //验证auth_code
            if (!await _authCodeService.CheckAuthCodeAsync(phone,code))
            {
                context.Result = errorValidationResult;
                return;
            }
            
            //验证手机号
            var userId = await _userService.CheckOrCreate(phone);
            if (userId < 1)
            {
                context.Result = errorValidationResult;
                return;
            }
            context.Result = new GrantValidationResult(userId.ToString(), GrantType);
            return;
        }

        public string GrantType { get; } = "SmsAuthCode";
    }
}