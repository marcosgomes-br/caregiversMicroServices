#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Caregivers.Consumers.Api/Caregivers.Consumers.Api.csproj", "Caregivers.Consumers.Api/"]
RUN dotnet restore "./Caregivers.Consumers.Api/Caregivers.Consumers.Api.csproj"
COPY . .
WORKDIR "/src/Caregivers.Consumers.Api"
RUN dotnet build "./Caregivers.Consumers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Caregivers.Consumers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Caregivers.Consumers.Api.dll"]