# Cities Manager Project Setup

## Run SQL Server
```shell
sudo docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=FranciscoOro41$" -p 1433:1433 --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
```

## Apply Migrations 
### From CitiesManager.Infrastructure
```shell
cd ./CitiesManager.Infrastructure
dotnet ef database update --startup-project ../CitiesManager.WebAPI
```

