using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using common;

namespace pw_reset_web.Models
{
    public class UserEmailModel
    {
        [Required(ErrorMessage ="Oracle Username can't be empty")]
        [StringLength(100)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Recovery Email Address can't be empty")]
        //[EmailAddress(ErrorMessage = "Email Address is not valid")]
        [StringLength(500)]
        public string EmailAddr { get; set; }

        public UserEmailInfo ToInfo()
        {
            UserEmailInfo uei = new UserEmailInfo();
            uei.UserName = this.UserName;
            uei.EmailAddr = this.EmailAddr;
            return uei;
        }
    }
}
