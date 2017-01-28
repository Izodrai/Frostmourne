# Frostmourne

## Synopsis

It's a project for retrieve and analyse data stock exchange from xtb ( https://www.xtb.com/ ).


Using the api -> http://developers.xstore.pro/


For the developpers you can create an illimited demo account

## Motivation

I develop it for learn C# and use mathematical concepts like Mobile Average Simple and Exponential, Moving Average Convergence Divergence...


I want to know if it's possible to analyse the market with these tools and follow the tendencies of the Forex.


## Step1 -> Installation of XtbDataRetriever and Database Market

- First you can download the project and open it with Visual Studio 2015 and github plugin.
- You need to have an MySQL database with the last version. After that you can feed a new database with the sql script in the project : 
Frostmourne/XtbDataRetriever/Dbs/Sources/sql_creator.sql
- When this three last steps are done you will be update the App.config and add next parameters :


```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
    <appSettings>
      <add key="XtbServer" value="demo"/>
      <add key="XtbUserId" value="<your_xtb_user_id>"/>
      <add key="XtbUserPwd" value="<your_xtb_user_pwd>"/>

      <add key="MysqlServer" value="<your_mysql_server_name>"/>
      <add key="MysqlDatabase" value="<your_mysql_database_name>"/>
      <add key="MysqlLogin" value="<your_mysql_user_login>"/>
      <add key="MysqlPassword" value="<your_mysql_user_pwd>"/>
    </appSettings>
</configuration>
```

After that you can build and execute the project, normaly it will retrieve the data of the last month for the symbol EUR/USD.


If you want more symbols you can add them in the table `symbols`. 


Currently, you have already two more symbols, EUR/GBP and GBP/USD by their are not active.


For change that, you need to update the BOOLEAN var `active` by true. The solution will now retrieve these symbols (on the next restart)

