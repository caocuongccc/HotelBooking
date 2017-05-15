using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Einvoice_Customer.Models
{
    public class CustomerViewModel
    {        
        [Display(Name = "CusCode", ResourceType = typeof(Language.Resource))]
        public string CusCode { get; set; }

        [Display(Name = "CusName", ResourceType = typeof(Language.Resource))]
        public string Name { get; set; }

        [Display(Name = "Addr", ResourceType = typeof(Language.Resource))]
        public string Address { get; set; }

        [Phone]
        [Display(Name = "Phone", ResourceType = typeof(Language.Resource))]
        public string Phone { get; set; }

        [Display(Name = "MST", ResourceType = typeof(Language.Resource))]
        public string Taxcode { get; set; }

        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Language.Resource))]
        public string Email { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "taikhoan", ResourceType = typeof(Language.Resource))]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Chưa nhập mật khẩu hiện tại")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Mật khẩu hiện tại")]
        //[Remote("CheckCurrentPassword", "Account",
        //    HttpMethod = "POST",
        //    AdditionalFields = "UserName",
        //    ErrorMessage = "Mật khẩu hiện tại chưa đúng.")]
        //public string OldPassword { get; set; }

        [Required(ErrorMessage = "Chưa nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "{0} ít nhất phải có {2} ký tự trở lên.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPass", ResourceType = typeof(Language.Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ReNewPass", ResourceType = typeof(Language.Resource))]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại chưa khớp.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordGuestViewModel
    {
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
               
        [Required(ErrorMessage = "Chưa nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "{0} ít nhất phải có {2} ký tự trở lên.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại chưa khớp.")]
        public string ConfirmPassword { get; set; }


        public string FullName { get; set; }
    }
    public class DoLoginViewModel
    {
        [StringLength(150, ErrorMessage = "Username can't too long 150 character.")]
        [Required(ErrorMessage = "Not enter username (cuscode)")]
        [Display(Name = "Username:")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Not enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Display(Name = "Remember", ResourceType = typeof(Language.Resource))]

        public bool RememberMe { get; set; }
    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Not enter username")]
        [Display(Name = "Username:")]
        public string CustomerCode { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Not enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwork:")]
        public string Password { get; set; }

        [Display(Name = "Remember", ResourceType = typeof(Language.Resource))]
        public bool RememberMe { get; set; }


    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        
        
        
        [Display(Name = "Account", ResourceType = typeof(Language.Resource))]
        public string Account { get; set; }
        
    }
}
