using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using common;

namespace pw_reset_web.Models
{
    public class UserResetPwdModel
    {
        [StringLength(100)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Recovery Email Address can't be empty")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [StringLength(500)]
        public string EmailAddr { get; set; }

        //[Required(ErrorMessage = "Verification Code can't be empty.")]
        [StringLength(8)]
        public string VerificationCode { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9#_$]{1,30}$", ErrorMessage = "Characters are not allowed.")]
        public string OraclePassword { get; set; }

        [Display(Name = "Re-type Oracle Password")]
        [Compare(nameof(OraclePassword), ErrorMessage = "Passwords don't match.")]
        public string ConfirmedOraclePassword { get; set; }
        public string VerifyCodeExpiredTimeStr { get; set; }

        public UserResetPwdInfo ToInfo()
        {
            UserResetPwdInfo urpi = new UserResetPwdInfo();
            urpi.UserName = this.UserName;
            urpi.EmailAddr = this.EmailAddr;
            urpi.OraclePassword = this.OraclePassword;
            urpi.VerificationCode = this.VerificationCode;
            urpi.VerifyCodeExpiredTime = this.VerifyCodeExpiredTimeStr;
            return urpi;
        }
    }
}
