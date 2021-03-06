﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrimeMarket.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required (ErrorMessage ="ادخل البريد الالكترونى")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "ادخل البريد الالكترونى")]
        [Display(Name = "البريد الالكترونى")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "ادخل كلمة السر")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [Display(Name = "تذكرنى ؟")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "ادخل البريد الالكترونى")]
        [EmailAddress]
        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ادخل الاسم بالكامل")]
        [Display(Name = "الاسم بالكامل")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "ادخل كلمة السر")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة السر")]
        [Compare("Password", ErrorMessage = "كلمة المرور وكلمة المرور التأكيدية غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "ادخل البريد الالكترونى")]
        [EmailAddress]
        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ادخل كلمة السر")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة السر")]
        [Compare("Password", ErrorMessage = "كلمة المرور وكلمة المرور التأكيدية غير متطابقين")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "ادخل البريد الالكترونى")]
        [EmailAddress]
        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }
    }
}
