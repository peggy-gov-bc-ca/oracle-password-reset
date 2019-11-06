# oracle-password-reset
oracle-password-reset
deploy steps:
1. get all things from github
2. check out the all things
3. start powershell
4. in powershell, go to the folder of the project
4. run docker login container-registry.oracle.com
5. run "docker-compose up"
6. After a while, the oracle server should be up, then start another powershell
7. run docker ps
8. run docker exec -it oracle-password-reset_database_1 /bin/bash
9. run echo exit | sqlplus sys/Oradoc_db1@localhost/ORCLCDB.localdomain as sysdba @01_createUser.sql
10. run echo exit | sqlplus C##ORACLE/123456@localhost/ORCLCDB.localdomain @02_createTable.sql
11. Then you can start browser to visit localhost:5050
