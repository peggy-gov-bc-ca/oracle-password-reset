using System;

namespace common
{
    public class UserResetPwdInfo
    {
        public string UserName { get; set; }
        public string EmailAddr { get; set; }
        public string VerificationCode { get; set; }
        public string OraclePassword { get; set; }
        public string VerifyCodeExpiredTime { get; set; }
    }
}
