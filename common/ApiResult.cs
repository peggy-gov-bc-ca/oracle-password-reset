using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    public class ApiResult
    {
        public ResultCode resultCode { get; set; }
        public string errMsg { get; set; }
    }

    public enum ResultCode
    {
        SUCCESS = 0,
        SEND_EMAIL_FAILED = 1,
        INVALID_EMAIL = 2,
        USER_NOT_REGISTERED = 3,
        INVALID_ORACLE_USERNAME=4,
        REGISTER_USER_FAILED_IN_DB = 5,
        INVALID_ORACLE_PASSWORD = 6,
        SET_USER_PASSWORD_FAILED = 7,
        INVALID_VERIFICATION_CODE = 8,
        VERIFICATION_CODE_EXPIRED = 9,
        USER_ALREADY_REGISTERED = 10
    }

    public enum ACTION
    {
        REGISTER = 0,
        RESET_PASSWORD = 1
    }
}
