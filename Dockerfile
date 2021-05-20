FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ./WebCustomAPI ./
RUN dotnet restore

RUN dotnet test

RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0

ENV TZ=America/Sao_Paulo

EXPOSE 9092

WORKDIR /app

COPY --from=build-env /app/dist .

COPY ./WebCustomAPI/appsettings.*.json ./

ENTRYPOINT ["dotnet", "WebCustomAPI.dll"]