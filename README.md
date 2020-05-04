# oracle-password-reset

Simple Proof-of-Concept application to allow users with a web interface to change and reset their oracle passwords.

## Running the App Locally

1. Clone the repo. `git clone https://github.com/peggy-gov-bc-ca/oracle-password-reset.git`.

2. Navigate to the project root. `cd oracle-password-reset`.

3. Open a command prompt/terminal shell.

4. Run `docker login container-registry.oracle.com`. In order for this to work, you may need to do the following first:
- If you do not already have an oracle account, create one at `https://profile.oracle.com/myprofile/account/create-account.jspx`.
- Use the credentials from your newly created account to login (for the docker login command).

5. Run `docker-compose up`. This should spin up all your services (DB, Web, and API).

6. After a while, the oracle server should be up. Then, in another terminal run `docker ps` to see the containers running currently.

7. Then run `docker exec -it oracle-password-reset_database_1 /bin/bash` to enter the shell of the DB container.

8. Run `echo exit | sqlplus sys/Oradoc_db1@localhost/ORCLCDB.localdomain as sysdba @01_createUser.sql` to execute the createUser script. If this does not work, you may try running the script manually by doing the following from within the container:
- Run `sqlplus`.
- Login with username as `sys as sysdba`.
- Password will be `Oradoc_db1`.
- Copy and paste the `createUser.sql` script as is here when in the `SQL` shell to execute the sql and create the user.

9. Run `echo exit | sqlplus C##ORACLE/123456@localhost/ORCLCDB.localdomain @02_createTable.sql` to execute the createTable script. If this does not work, do the same thing as before, except use the following credentials:
- Username as `C##ORACLE`.
- Password as `123456`.
- Copy and paste the `createTable.sql` script as is here when in the `SQL` shell to execute the sql and create the table(s).

## SMTP Setup

1. Open powershell.

2. Run `docker exec -it oracle-password-reset_pw-reset-api_1 /bin/bash` to enter the API container shell.

3. Get the latest packages: `apt-get update`.

4. Install ssmtp: `apt-get -y install ssmtp`.

5. Exit the API shell.

6. Copy a file from a container to the host, you can use the command: `docker cp <containerId>:/file/path within/container /host/path/target`. So in our case:
```
docker cp <containerId>:/etc/ssmtp/ssmtp.conf .
```

7. Modify the ssmtp.conf as follows:
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

8. Place the modified ssmtp.conf to api container etc/ssmtp/ssmtp.conf (override the old one)

## App Frontend

Visit `localhost:5050` on your favourite browser to see the WEB frontend in action.
