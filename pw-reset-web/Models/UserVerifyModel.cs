using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using common;

namespace pw_reset_web.Models
{
    public class UserVerifyModel
    {
        public string EmailAddr { get; set; }

        [Required(ErrorMessage = "Verification Code can't be empty.")]
        [StringLength(8)]
        public string VerifyCode { get; set; }
        public string UserName { get; set; }

        public UserVerifyInfo ToInfo()
        {
            UserVerifyInfo uei = new UserVerifyInfo();
            uei.UserName = this.UserName;
            uei.EmailAddr = this.EmailAddr;
            uei.VerifyCode = this.VerifyCode;
            return uei;
        }
    }
}
