using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace PrimeMarket.Models
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "ادخل اسم الصلاحية")]
        [Display(Name = "اسم الصلاحية")]
        public string RoleName { get; set; }
    }

    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "ادخل اسم الصلاحية")]
        [Display(Name = "اسم الصلاحية")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }

    public class UserRole_UserList_ViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsSelected { get; set; }
    }
}