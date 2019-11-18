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
11. start another powershell
12. input
```bash
docker exec -it oracle-password-reset_pw-reset-api_1 /bin/bash
```
13.input 
```
apt-get update
```
14. input
```
apt-get -y install ssmtp
```
15. get out of the api shell
16. copy a file from a container to the host, you can use the command
docker cp <containerId>:/file/path/within/container /host/path/target
 so, input command
 ```
 docker cp <containerId>:/etc/ssmtp/ssmtp.conf .
 ```
 17. modifie ssmtp.conf like following:
 ```
 #
# Config file for sSMTP sendmail
#
# The person who gets all mail for userids < 1000
# Make this empty to disable rewriting.
root=sierranttsystem@gmail.com

# The place where the mail goes. The actual machine name is required no 
# MX records are consulted. Commonly mailhosts are named mail.domain.com
mailhub=smtp.gmail.com:587
AuthUser=sierranttsystem@gmail.com
AuthPass=Sierra0812
USETLS=YES
USESTRTTLS=YES


# Where will the mail seem to come from?
#rewriteDomain=

# The full hostname
hostname=container id

# Are users allowed to set their own From: address?
# YES - Allow the user to specify their own From: address
# NO - Use the system generated From: address
#FromLineOverride=YES
```
18. put the modified ssmtp.conf to api container etc/ssmtp/ssmtp.conf (override the old one

11. Then you can start browser to visit localhost:5050

