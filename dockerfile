FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env 
WORKDIR /app

COPY . ./

#RUN dotnet restore 
RUN dotnet publish web_table.Web/web_table.Web.csproj -c Release -o out

#add command for migrate db
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /app/web_table.Web
RUN dotnet ef database update 


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app 
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "web_table.Web.dll" ]
