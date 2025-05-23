﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WalletFlow.Api/WalletFlow.Api.csproj", "src/WalletFlow.Api/"]
COPY ["src/WalletFlow.Application/WalletFlow.Application.csproj", "src/WalletFlow.Application/"]
COPY ["src/WalletFlow.Domain/WalletFlow.Domain.csproj", "src/WalletFlow.Domain/"]
COPY ["src/WalletFlow.Infrastructure/WalletFlow.Infrastructure.csproj", "src/WalletFlow.Infrastructure/"]
COPY ["src/WalletFlow.Shared/WalletFlow.Shared.csproj", "src/WalletFlow.Shared/"]
COPY ["src/WalletFlow.IoC/WalletFlow.IoC.csproj", "src/WalletFlow.IoC/"]

RUN dotnet restore "src/WalletFlow.Api/WalletFlow.Api.csproj"
COPY . .
WORKDIR "/src/src/WalletFlow.Api"
RUN dotnet build "WalletFlow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WalletFlow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletFlow.Api.dll"]
