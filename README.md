SqlMonitor
==========
SqlMonitor is a tool that can be used to monitor performance of databases by performing a query at regular intervals 
and monitoring the time that the query is taking. This can be useful for identifying performance spikes and correlating
performance issues across multiple databases.

Queries are scheduled using cron expressions and can be configured to send alerts if their execution time exceeds a 
defined threshold.

Performance results may be graphed and overlayed upon one another to assist in correlating performance issues.

Supported Databases
------------------
The SqlMonitor application stores its scheduling/performance data in an SqlServer database.

For running queries, any database driver that supports the ADO.NET 2.0 provider framework should work. I have tested 
against MS SqlServer 2008 and Sybase 11.9.2 / Sybase 15.5.

Technologies
------------
* ASP.NET MVC3
* Entity Framework 4.1 (Code First)
* NServiceBus
* Castle Windsor (for IoC)
* Entity Framework
* Quartz.Net (for scheduling)
* Bootstrapper
* AutoMapper
* jQuery

Installation
------------
* Please ensure that MSMQ is installed before commencing.
* The system should be self installing 
    * the database should be created/installed automatically on the .\SQLEXPRESS database. 
    * The MSMQ queues should create automatically.

The query scheduling/performance database can be configured by adding/modifing the connection string with name 
"SqlMonitor" into the <connectionStrings> section of the application config file in both the web app and the 
server. For example:

    <connectionStrings>
       <add name="SqlMonitor" connectionString="server=.\SQLEXPRESS;database=SqlMonitor;user=SqlMonitor;password=$q1m0n;Integrated Security=False" providerName="System.Data.SqlClient"/>    
    </connectionStrings>
	
To configure databases to **query** from, add connection strings for the databases into the SqlMonitor.Server 
app.config file and then add the names of the connection strings into the SqlMonitor.Web web.config file in the 
*DatabaseNames* app setting:

    <add key="DatabaseNames" value="Orders,Billing,Shipping"/>
	
To debug:
* open the solution properties
  * select 'Multiple Startup Projects'
  * set action for SqlMonitor.Server and SqlMonitor.Web to 'Start'
* open the SqlMonitor.Server project properties.
  * select the Debug tab
  * under 'Start Action', select 'Start external program'
  * browse to the SqlMonitor.Server\bin\debug folder and select NServiceBus.Host.exe
	
Screenshots
-----------
Here is a screenshot of the query listing page. The graph at the bottom is showing the results of two different 
queries against different databases. 

![Dashboard](https://github.com/paulduran/SqlMonitor/raw/master/docs/sqlmon_dashboard.png)

Here is a screenshot of the 'add query' page. The help information for the cron expressions is shown in a popup.

![Add Query](https://github.com/paulduran/SqlMonitor/raw/master/docs/sqlmon_create_query.png)

Here is a screenshot of the 'query details' page. This page shows the configuration of the query and generates 
graphs of the query results, including a threshold line. 

![Query Details](https://github.com/paulduran/SqlMonitor/raw/master/docs/sqlmon_query_details.png)
	
Contact Me
----------
For any questions or comments, please contact me - paulduran at gmail dot com
