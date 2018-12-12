#h2 Quick'n Dirty template for Ef Core migrations throw console app and tooling.
quick quide/template for making a Ef core code first with migrations console app that can be used to have the context in seperate project from your other core app and manage migrations through it.

#h3 Pre Req.
Make sure you have a sqlserver instance available.
Or change the code to use another ef provider.

using docker:

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<YourStrong!Passw0rd>"  -p 1433:1433 --name sql1  -d microsoft/mssql-server-linux:2017-latest


1. set Connection string in appsettings.json,

2.  dotnet ef migrations add Initial

will create the first migrations. 


3. run dotnet run -- -u   to apply migrations

4. do changes to the models/context and generate a new  migration  dotnet ef migrations add FirstMigration.

5. run dotnet run -- -u   to apply the new ones.