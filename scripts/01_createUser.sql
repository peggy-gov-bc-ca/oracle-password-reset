-------------------------Create user C##ORACLE for database operation
CREATE USER "C##ORACLE" IDENTIFIED BY "123456"  
DEFAULT TABLESPACE "USERS"
TEMPORARY TABLESPACE "TEMP";

-- QUOTAS
ALTER USER "C##ORACLE" QUOTA UNLIMITED ON "SYSTEM";
ALTER USER "C##ORACLE" QUOTA UNLIMITED ON "SYSAUX";
ALTER USER "C##ORACLE" QUOTA UNLIMITED ON "USERS";

-- ROLES
GRANT "CONNECT" TO "C##ORACLE" WITH ADMIN OPTION;
GRANT CREATE SESSION TO "C##ORACLE" WITH ADMIN OPTION;
GRANT "DBA" TO "C##ORACLE" WITH ADMIN OPTION;

-- SYSTEM PRIVILEGES
GRANT ALTER USER TO "C##ORACLE" WITH ADMIN OPTION;
GRANT UPDATE ANY TABLE TO "C##ORACLE" WITH ADMIN OPTION;
GRANT CREATE ANY TABLE TO "C##ORACLE" WITH ADMIN OPTION;

-----------------------------Create user C##SUPERMAN for demo
CREATE USER "C##SUPERMAN" IDENTIFIED BY "super"
DEFAULT TABLESPACE "USERS"
TEMPORARY TABLESPACE "TEMP";

-- QUOTAS
ALTER USER "C##SUPERMAN" QUOTA UNLIMITED ON "SYSTEM";
ALTER USER "C##SUPERMAN" QUOTA UNLIMITED ON "SYSAUX";
ALTER USER "C##SUPERMAN" QUOTA UNLIMITED ON "USERS";

-- ROLES
GRANT "CONNECT" TO "C##SUPERMAN" WITH ADMIN OPTION;
GRANT CREATE SESSION TO "C##SUPERMAN" WITH ADMIN OPTION;

-----------------------------Create user C##WONDERWOMAN for test
CREATE USER "C##WONDERWOMAN" IDENTIFIED BY "wonder"
DEFAULT TABLESPACE "USERS"
TEMPORARY TABLESPACE "TEMP";

-- QUOTAS
ALTER USER "C##WONDERWOMAN" QUOTA UNLIMITED ON "SYSTEM";
ALTER USER "C##WONDERWOMAN" QUOTA UNLIMITED ON "SYSAUX";
ALTER USER "C##WONDERWOMAN" QUOTA UNLIMITED ON "USERS";

-- ROLES
GRANT "CONNECT" TO "C##WONDERWOMAN" WITH ADMIN OPTION;
GRANT CREATE SESSION TO "C##WONDERWOMAN" WITH ADMIN OPTION;