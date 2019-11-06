  ------------------create table ORACLE_PWD_RESET_USERS
   CREATE TABLE "C##ORACLE"."ORACLE_PWD_RESET_USERS" 
   (	"ID" NUMBER NOT NULL ENABLE, 
	"USERNAME" VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	"EMAILADDR" VARCHAR2(500 BYTE) NOT NULL ENABLE, 
	"RECOVERYCODE" VARCHAR2(8 BYTE), 
	"EXPIRETIME" VARCHAR2(100 BYTE), 
	 CONSTRAINT "ORACLE_PWD_RESET_USERS_PK" PRIMARY KEY ("ID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS"  ENABLE
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;
  
 -------------------------------create table ORACLE_PWD_RESET_AUDIT
  CREATE TABLE "C##ORACLE"."ORACLE_PWD_RESET_AUDIT" 
   (	"ID" NUMBER NOT NULL ENABLE, 
	"USERNAME" VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	"ACTION" VARCHAR2(500 BYTE), 
	"EXECUTETIME" VARCHAR2(500 BYTE), 
	"RESULT" VARCHAR2(500 BYTE), 
	"MSG" VARCHAR2(500 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "USERS" ;