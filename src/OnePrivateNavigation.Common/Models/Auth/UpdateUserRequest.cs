using System.ComponentModel.DataAnnotations;

namespace OnePrivateNavigation.Common.Models.Auth
{
    public class UpdateUserRequest
    {
        public string NewUsername { get; set; }

        [Required(ErrorMessage = "必须提供原密码")]
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}