using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pw_reset_api.Interfaces;
using common;
using pw_reset_api.Utilities;

namespace pw_reset_api.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class OracleUserController : ControllerBase
    {
        IOracleUserRepository _oracleUserRepo;

        public OracleUserController(IOracleUserRepository userRepo)
        {
            this._oracleUserRepo = userRepo;
        }

        [Route("api/GetUserCodeExpireTime")]
        [HttpPost]
        public ActionResult GetUserCodeExpireTime(UserResetPwdInfo info)
        {
            UserResetPwdInfo userResetPwdInfo= _oracleUserRepo.GetUserResetPwdInfo(info.UserName);
            return Ok(userResetPwdInfo.VerifyCodeExpiredTime);
        }

        [Route("api/GetUserEmail")]
        [HttpPost]
        public ActionResult GetUserEmail(UserResetPwdInfo info)
        {
            UserResetPwdInfo userResetPwdInfo = _oracleUserRepo.GetUserResetPwdInfo(info.UserName);
            return Ok(userResetPwdInfo.EmailAddr);
        }


        [Route("api/VerifyCode")]
        [HttpPost]
        public ActionResult VerifyCode(UserVerifyInfo vInfo)
        {
            UserResetPwdInfo info = new UserResetPwdInfo();
            info.UserName = vInfo.UserName;
            info.EmailAddr = vInfo.EmailAddr;
            info.VerificationCode = vInfo.VerifyCode;
            return Ok(IsValidRequest(info));
        }


        [Route("api/DoResetPwd")]
        [HttpPost]
        public ActionResult DoResetPwd(UserResetPwdInfo info)
        {
            ApiResult result = new ApiResult();
            //result = IsValidRequest(info);
            if ( result.resultCode == ResultCode.SUCCESS)
            {
                if (_oracleUserRepo.ChangeOracleUserPassword(info.UserName, info.OraclePassword))
                {
                    result.resultCode = ResultCode.SUCCESS;
                    result.errMsg = "success";
                }
                else 
                {
                    result.resultCode = ResultCode.SET_USER_PASSWORD_FAILED;
                    result.errMsg = "Set user password failed";
                }
            }
            AddInfoToAudit(info.UserName, result, ACTION.RESET_PASSWORD.ToString());
            return Ok(result);
        }

        [Route("api/ResetPwd")]
        [HttpPost]
        public ActionResult ResetPwd(UserEmailInfo usernameEmail)
        {
            ApiResult result = new ApiResult(); 
            if (IsRegistered(usernameEmail.UserName))
            {
                string emailAddr = GetEmailAddress(usernameEmail.UserName);
                if (SendVerifyCode(usernameEmail))
                {
                    result.resultCode = ResultCode.SUCCESS;
                    result.errMsg = "Success";
                }
                else 
                {
                    result.resultCode = ResultCode.SEND_EMAIL_FAILED;
                    result.errMsg = "Sending verification code to the registered email address failed.";
                }
            }
            else
            {
                if (IsValidOracleUser(usernameEmail.UserName))
                {
                    result.resultCode = ResultCode.USER_NOT_REGISTERED;
                    result.errMsg = "You have not yet registered for the Password Reset service. Please register now.";
                }
                else
                {
                    result.resultCode = ResultCode.INVALID_ORACLE_USERNAME;
                    result.errMsg = "The entered Oracle username is invalid.";
                }
            }
            return Ok(result);
        }

        [Route("api/Register")]
        [HttpPost]
        public ActionResult Register(UserRegisterInfo registerInfo)
        {
            ApiResult result = new ApiResult();
            if (IsRegistered(registerInfo.UserName))
            {
                result.resultCode = ResultCode.USER_ALREADY_REGISTERED;
                result.errMsg = "User is already registerd. Do not need to register again.";
            }
            else
            {
                if (IsValidOracleUser(registerInfo.UserName))
                {
                    if (IsValidOraclePassword(registerInfo.UserName, registerInfo.OraclePassword))
                    {
                        if (_oracleUserRepo.AddUserToRegister(registerInfo))
                        {
                            result.resultCode = ResultCode.SUCCESS;
                            result.errMsg = "Success";
                        }
                        else
                        {
                            result.resultCode = ResultCode.REGISTER_USER_FAILED_IN_DB;
                            result.errMsg = "Registration failed. The user could not be registered to the DB.";
                        }
                    }
                    else
                    {
                        result.resultCode = ResultCode.INVALID_ORACLE_PASSWORD;
                        result.errMsg = "The provided oracle password is incorrect. Registration failed.";
                    }
                }
                else
                {
                    result.resultCode = ResultCode.INVALID_ORACLE_USERNAME;
                    result.errMsg = "The provided oracle username is incorrect. Registration failed.";
                }
            }
            AddInfoToAudit(registerInfo.UserName, result, ACTION.REGISTER.ToString());
            return Ok(result);
        }

        private bool IsRegistered(String username)
        {
            return _oracleUserRepo.IsUserRegistered(username);
        }

        private bool IsEmailValid(UserEmailInfo usernameEmail)
        {
            return _oracleUserRepo.IsEmailValid(usernameEmail); 
        }

        private string GetEmailAddress(string username)
        {
            return _oracleUserRepo.GetUserResetPwdInfo(username).EmailAddr;
        }

        private bool SendVerifyCode(UserEmailInfo usernameEmail)
        {
            string verifyCode = RandomString.GenerateRandomString(8);
            DateTime expiredTime = DateTime.Now.AddMinutes(Constants.VERIFY_CODE_VALID_MINS);
            bool result = SendEmail(usernameEmail, verifyCode, expiredTime.ToString());
            if (result)
            {
                UserResetPwdInfo urpi = new UserResetPwdInfo();
                urpi.UserName = usernameEmail.UserName;
                urpi.EmailAddr = usernameEmail.EmailAddr;
                urpi.VerificationCode = verifyCode;
                urpi.VerifyCodeExpiredTime = expiredTime.ToString();
                _oracleUserRepo.AddVerificationInfo(urpi);
            }
            return result;
        }

        private bool IsValidOracleUser(string username)
        {
            return _oracleUserRepo.IsValidOracleUser(username);
        }

        private bool IsValidOraclePassword(string username, string password)
        {
            return _oracleUserRepo.IsValidOraclePassword(username, password);
        }

        private ApiResult IsValidRequest(UserResetPwdInfo info)
        {
            ApiResult result = new ApiResult();
            UserResetPwdInfo infoInDB = _oracleUserRepo.GetUserResetPwdInfo(info.UserName);
            if (infoInDB.VerificationCode == info.VerificationCode)
            {
                DateTime now = DateTime.Now;
                DateTime expired = DateTime.Parse(infoInDB.VerifyCodeExpiredTime);
                if (now < expired)
                {
                    result.resultCode = ResultCode.SUCCESS;
                    return result;
                }
                else
                {
                    result.resultCode = ResultCode.VERIFICATION_CODE_EXPIRED;
                    result.errMsg = "The entered verification code has already expired, please reset your oracle database password again.";
                    return result;
                }
            }
            else {
                result.resultCode = ResultCode.INVALID_VERIFICATION_CODE;
                result.errMsg = "The verification code is incorrect, please check your verification email and try again.";
            }
            return result;
        }

        private bool SendEmail(UserEmailInfo usernameEmail, string verifyCode, string expireTime)
        {
            string content = "Dear " + usernameEmail.UserName + ",<br/> " + "<p>Your Oracle password recovery verification code is "
                + verifyCode
                + " ."
                + " Please note that this recovery code will expire at "
                + expireTime
                + "(5 minutes from your reset request).</p>";
            return EmailUtil.SendEmail(usernameEmail.EmailAddr, content);
            
        }

        private void AddInfoToAudit(string username, ApiResult result, string action)
        {
            _oracleUserRepo.AddInfoToAudit(username, result, action);
        }
    }
}