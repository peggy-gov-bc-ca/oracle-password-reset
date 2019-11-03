using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using pw_reset_api.Interfaces;
using common;
using Microsoft.Extensions.Logging;

namespace pw_reset_api.Repositories
{
    public class OracleUserRepository : IOracleUserRepository
    {
        IConfiguration configuration;
        ILogger _logger;

        public OracleUserRepository(IConfiguration _configuration, ILogger<IOracleUserRepository> logger)
        {
            this.configuration = _configuration;
            this._logger = logger;
        }

         public bool AddUserToRegister(UserRegisterInfo uri)
        {
            try
            {
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @" INSERT INTO oracle_pwd_reset_users (id, username, emailAddr)
                              VALUES(
                                (select CASE WHEN max(ID) IS null THEN 1 ELSE max(ID) + 1 END AS NewID from oracle_pwd_reset_users),
                                :username, 
                                :email)";

                            OracleParameter[] parameters = new OracleParameter[] {
                                             new OracleParameter("username",uri.UserName),
                                             new OracleParameter("email",uri.EmailAddr)
                                      };
                            cmd.Parameters.AddRange(parameters);
                            _logger.LogDebug("execute oracle cmd insert user");
                            int r = cmd.ExecuteNonQuery();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return false;
            }
            return true;
        }

        public bool IsUserRegistered(String username)
        {
            try
            {
                bool result = false;
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @"SELECT * FROM oracle_pwd_reset_users WHERE username=:username";
                            OracleParameter[] parameters = new OracleParameter[] { new OracleParameter("username",username) };
                            cmd.Parameters.AddRange(parameters);
                            OracleDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                result = true;
                                break;
                            }
                            reader.Dispose();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return false;
                        }
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return false;
            }
        }

        public bool IsEmailValid(UserEmailInfo userInfo)
        {
            try
            {
                bool result = false;
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @"SELECT * FROM oracle_pwd_reset_users WHERE username=:username and emailaddr=:email";
                            OracleParameter[] parameters = new OracleParameter[] { 
                                new OracleParameter("username", userInfo.UserName),
                                new OracleParameter("email", userInfo.EmailAddr)
                            };
                            cmd.Parameters.AddRange(parameters);
                            OracleDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                result = true;
                                break;
                            }
                            reader.Dispose();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return false;
                        }
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return false;
            }
        }

        public bool IsValidOracleUser(string username) 
        {
            try
            {
                bool result = false;
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @"SELECT * FROM all_users WHERE username=:username";
                            OracleParameter[] parameters = new OracleParameter[] {
                                new OracleParameter("username", username)
                            };
                            cmd.Parameters.AddRange(parameters);
                            OracleDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                result = true;
                                break;
                            }
                            reader.Dispose();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return false;
                        }
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return false;
            }
        }

        public bool IsValidOraclePassword(string username, string password)
        {
            bool result = false;
            try
            {
                OracleConnection con = new OracleConnection();
                con.ConnectionString ="User Id="+username+";Password="+password+";Data Source="+Constants.ORACLE_DATA_SOURCE;
                con.Open();
                _logger.LogInformation("oracle connection opened.");
                result = true;
                con.Close();
                _logger.LogInformation("oracle connection closed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return result;
            }
            return result;
        }

        public bool AddVerificationInfo(UserResetPwdInfo userResetPwdInfo)
        {
            try
            {
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @" UPDATE oracle_pwd_reset_users 
                                                 SET RECOVERYCODE = :recoveryCode,
                                                     EXPIRETIME = :expiryTime
                                                 WHERE USERNAME = :username
                              ";

                            OracleParameter[] parameters = new OracleParameter[] {
                                             new OracleParameter("recoveryCode",userResetPwdInfo.VerificationCode),
                                             new OracleParameter("expiryTime",userResetPwdInfo.VerifyCodeExpiredTime),
                                             new OracleParameter("username",userResetPwdInfo.UserName)
                                      };

                            cmd.Parameters.AddRange(parameters);
                            int r = cmd.ExecuteNonQuery();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return false;
            }
            return true;
        }

        public bool ChangeOracleUserPassword(string username, string password)
        {
            try
            {
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            //        cmd.CommandText = @"ALTER USER " + '"' + "JONNY" + '"' +
                            //" IDENTIFIED BY " + '"' + "333333" + '"';

                            cmd.CommandText = @"ALTER USER " + '"' + username + '"' +
                                                " IDENTIFIED BY " + '"' + password + '"';

                            //cmd.CommandText = @"ALTER USER :username IDENTIFIED BY :password;";
                            //OracleParameter[] parameters = new OracleParameter[] {
                            //                 new OracleParameter("username",username),
                            //                 new OracleParameter("password", password)
                            //          };

                            //cmd.Parameters.AddRange(parameters);
                            int r = cmd.ExecuteNonQuery();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return false;
            }
            return true;
        }

        public UserResetPwdInfo GetUserResetPwdInfo(string username)
        {
            UserResetPwdInfo userResetPwdInfo = new UserResetPwdInfo();
            try
            {
                var dbConnection = this.GetConnection();
 
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM " + "oracle_pwd_reset_users WHERE username=" + "'" + username + "'";
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            userResetPwdInfo.UserName = reader.IsDBNull(1)? "": reader.GetString(1);
                            userResetPwdInfo.EmailAddr = reader.GetString(2);
                            userResetPwdInfo.VerificationCode = reader.IsDBNull(3) ? "": reader.GetString(3);
                            userResetPwdInfo.VerifyCodeExpiredTime = reader.IsDBNull(4)? "": reader.GetString(4);
                            break;
                        }
                        reader.Dispose();
                        dbConnection.Close();
                        _logger.LogInformation("oracle connection closed.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return null;
            }
            return userResetPwdInfo;
        }

        public void AddInfoToAudit(string username, ApiResult result, String action) 
        {
            try
            {
                var dbConnection = this.GetConnection();
                if (dbConnection.State == System.Data.ConnectionState.Closed)
                {
                    dbConnection.Open();
                    _logger.LogInformation("oracle connection opened.");
                }
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    using (OracleCommand cmd = dbConnection.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @" INSERT INTO oracle_pwd_reset_audit (id, username, action, executetime, result,msg)
                              VALUES(
                                (select CASE WHEN max(ID) IS null THEN 1 ELSE max(ID) + 1 END AS NewID from oracle_pwd_reset_audit),
                                :username, 
                                :action,
                                :executetime,
                                :result,
                                :msg)";

                            OracleParameter[] parameters = new OracleParameter[] {
                                             new OracleParameter("username",username),
                                             new OracleParameter("action",action),
                                             new OracleParameter("executetime", DateTime.Now.ToString()),
                                             new OracleParameter("result", result.resultCode.ToString()),
                                             new OracleParameter("msg", result.errMsg)
                                      };
                            cmd.Parameters.AddRange(parameters);
                            int r = cmd.ExecuteNonQuery();
                            dbConnection.Close();
                            _logger.LogInformation("oracle connection closed.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message, null);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                return;
            }
            return;
        }

        private OracleConnection GetConnection()
        {
            var connectionString = configuration.GetSection("ConnectionStrings").GetSection("UserConnection").Value;
            _logger.LogDebug("create connection" + connectionString);
            var conn = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
            _logger.LogInformation("oracle connection created.");
            return conn;
        }
    }
}
