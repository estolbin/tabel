FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env 
WORKDIR /app

COPY . ./
RUN dotnet restore 
RUN dotnet publish web_table.Web/web_table.Web.csproj -c Release -o out
#RUN dotnet publish web_tabel.API/web_tabel.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app 
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "web_table.Web.dll" ]
#ENTRYPOINT ["dotnet", "web_tabel.API.dll"]