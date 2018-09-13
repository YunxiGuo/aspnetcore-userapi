using IdentityServer4.Validation;
using System.Threading.Tasks;
using User.Identity.Service;

namespace User.Identity.Authorization
{
    public class SmsAuthCodeValidate : IExtensionGrantValidator
    {
        private IAuthCodeService _authCodeService;
        private IUserService _userService;

        public SmsAuthCodeValidate(IAuthCodeService authCodeService, IUserService userService)
        {
            _authCodeService = authCodeService;
            _userService = userService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            //验证auth_code
            context.Result = new GrantValidationResult();
            //var authResult = _authCodeService.CheckAuthCodeAsync(context.Request.AuthorizationCode)
            throw new System.NotImplementedException();
        }

        public string GrantType { get; } = "SmsAuthCode";
    }
}