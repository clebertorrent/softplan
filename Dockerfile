FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ./WebCustomAPI.ASE.BFF ./
RUN dotnet restore

RUN dotnet test

RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ENV TZ=America/Sao_Paulo

EXPOSE 80

WORKDIR /app

COPY --from=build-env /app/dist .

COPY ./WebCustomAPI.ASE.BFF/appsettings.*.json ./

ENTRYPOINT ["dotnet", "WebCustomAPI.ASE.BFF.dll"]