# Removendo containers parados
docker container prune
# Subindo servidor de banco de dados SQL Server
docker run --name blog-db -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Master2020#" -p 1433:1433 -v medium-db-sql:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
# Subindo serviço do SEQ
docker run --name blog-seq --rm -it -e ACCEPT_EULA=Y -p 5341:80 -d datalust/seq
