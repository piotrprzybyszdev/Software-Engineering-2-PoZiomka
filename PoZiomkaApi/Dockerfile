FROM mcr.microsoft.com/dotnet/sdk:8.0

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["PoZiomkaApi/PoZiomkaApi.csproj", "PoZiomkaApi/"]

RUN dotnet restore "./PoZiomkaApi/PoZiomkaApi.csproj"
COPY . .

WORKDIR "/src/PoZiomkaApi"
RUN dotnet build "./PoZiomkaApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN dotnet publish "./PoZiomkaApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

WORKDIR /app/publish
ENTRYPOINT ["dotnet", "PoZiomkaApi.dll"]
