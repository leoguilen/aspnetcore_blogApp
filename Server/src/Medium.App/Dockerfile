# Building and Publishing App Stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0
WORKDIR /build
EXPOSE 80
COPY . .
RUN dotnet restore
RUN dotnet publish -c ${BUILDCONFIG} -o /app /p:Version=${VERSION}

# Running App Stage
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Medium.App.dll"]