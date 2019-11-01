using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pw_reset_api.Interfaces
{
    public interface IOracleUserRepository
    {
        //object GetUserStatus(string username);
        bool AddUserToRegister(UserRegisterInfo uri);
        bool IsUserRegistered(String username);
        bool IsEmailValid(UserEmailInfo username);
        bool IsValidOracleUser(string username);
        bool IsValidOraclePassword(string username, string password);
        bool AddVerificationInfo(UserResetPwdInfo userResetPwdInfo);
        //string GetUserCodeExpireTime(string username);
        bool ChangeOracleUserPassword(string username, string password);
        UserResetPwdInfo GetUserResetPwdInfo(string username);
        void AddInfoToAudit(string username, ApiResult result, string action);
    }
}
