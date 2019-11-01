using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using common;

namespace pw_reset_web.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage ="Oracle Username can't be empty")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Recovery Email Address can't be empty")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [StringLength(500)]
        public string EmailAddr { get; set; }

        [Required(ErrorMessage = "Oracle Password can't be empty")]
        public string OraclePwd { get; set; }

        public UserRegisterInfo ToInfo()
        {
            UserRegisterInfo uri = new UserRegisterInfo();
            uri.UserName = this.UserName;
            uri.EmailAddr = this.EmailAddr;
            uri.OraclePassword = this.OraclePwd;
            return uri;
        }
    }
}
