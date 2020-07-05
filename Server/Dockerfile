FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-image
 
ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

WORKDIR /home/app
EXPOSE 80

COPY ./*.sln ./
COPY ./src ./src
COPY ./test ./test

RUN dotnet restore
 
COPY . .

RUN dotnet publish ./src/Medium.App/Medium.App.csproj -c ${BUILDCONFIG} -o /publish /p:Version=${VERSION}
 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
 
WORKDIR /publish
 
COPY --from=build-image /publish .

ENTRYPOINT ["dotnet", "Medium.App.dll"]