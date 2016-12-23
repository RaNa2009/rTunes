# rTunes ReadMe

## LocalDB

### SQL Server Management Studio

(LocalDb)\MSSQLLocalDB

### Database location
 
Then create database foo without specifying the location for its files:

create database foo

Open Windows Explorer and navigate to your profile (typing %USERPROFILE% in the address bar is a nice shortcut). If the database was created successfully there will be new files in this folder, foo.mdf and foo_log.ldf that represent your database. QED.

Given that user profile folder is likely not the best location to store database files, we advise developers creating databases to always specify the location for the database files, like in this T-SQL example:

create database foo on (name=‘foo’, filename=‘c:\DBs\foo.mdf’)