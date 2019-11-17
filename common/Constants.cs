using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    public class Constants
    {
        //for container
        public static string API_BASE_URL = "http://pw-reset-api/api/";
        public static string ORACLE_DATA_SOURCE = "database:1521/ORCLCDB.localdomain";

        //for local debugging vs
        //public static string API_BASE_URL = "http://localhost:5000/api/";
        //public static string ORACLE_DATA_SOURCE = "localhost:1521/db.AMERICAS.GLOBAL.NTTDATA.COM";
        //public static string ORACLE_DATA_SOURCE = "localhost:1521/ORCLCDB.localdomain";

        public static string HOST_NAME = "localhost";
        public static int VERIFY_CODE_VALID_MINS = 5;
        
        public static string EMAIL_ADDRESS = "sierranttsystem@gmail.com";
        public static string EMAIL_PASSWORD = "Sierra0812";
    }
}
