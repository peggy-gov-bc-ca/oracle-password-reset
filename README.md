# oracle-password-reset
oracle-password-reset
##deploy steps:
1. get all things from github
2. check out the all things
3. start powershell
4. in powershell, go to the folder of the project
4. input 
```bash
docker login container-registry.oracle.com
```
5. input 
```bash
docker-compose up
```
6. After a while, the oracle server should be up, then start another powershell
7. input
```bash
 docker ps
```
8. input 
```bash
docker exec -it oracle-password-reset_database_1 /bin/bash
```

9. input 
```bash
echo exit | sqlplus sys/Oradoc_db1@localhost/ORCLCDB.localdomain as sysdba @01_createUser.sql
```
10. input 
```bash
echo exit | sqlplus C##ORACLE/123456@localhost/ORCLCDB.localdomain @02_createTable.sql
```

11. Then you can start browser to visit localhost:5050


SendGrid setup
key name: oraclePasswordReset
key: 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
Account username: SierraSystem
Password: sierrasystem8
