FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["sandbox/apis/Yggdrasil.Imports.Api/Yggdrasil.Imports.Api.csproj", "sandbox/apis/Yggdrasil.Imports.Api/"]
RUN dotnet restore "sandbox/apis/Yggdrasil.Imports.Api/Yggdrasil.Imports.Api.csproj"
COPY . .
WORKDIR "/src/sandbox/apis/Yggdrasil.Imports.Api"
RUN dotnet build "Yggdrasil.Imports.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yggdrasil.Imports.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yggdrasil.Imports.Api.dll"]
