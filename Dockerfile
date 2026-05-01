FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ClinicApp/*.csproj ClinicApp/
RUN dotnet restore ClinicApp/ClinicApp.csproj

COPY ClinicApp/ ClinicApp/

WORKDIR /src/ClinicApp
RUN dotnet publish -c Release -o /app



FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app .


ENTRYPOINT ["dotnet", "ClinicApp.dll"]

