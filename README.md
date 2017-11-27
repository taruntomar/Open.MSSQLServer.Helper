# db-dealer <a href="https://travis-ci.org/taruntomar/db-dealer"><img src="https://travis-ci.org/taruntomar/db-dealer.svg?branch=master"></a>
This is a lib to deal with sql server database

You can start interacting with SQL Server Database straight forward by creating instance with connection string.
```c#
 DatabaseManager dbManager = new  DatabaseManager(<connection string>);
```

Now you can execute write query like below,
```c#
var query = string.Format("insert into <table_name> values ('{0}','{1}')",value1,value2);
_dbmanager.ExecuteSQLWriter(query);
```
and read query like below,

```c#
Class[] authCodes = _dbmanager.ExecuteSQLReader<Class>("select * from <table>",row=> new Class(){ Field=row["Field"].ToString() });
```
