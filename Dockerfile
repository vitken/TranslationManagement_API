#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5184

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["External.ThirdParty.Services/External.ThirdParty.Services.csproj", "External.ThirdParty.Services/"]
COPY ["TM.Api/TranslationManagement.Api.csproj", "TM.Api/"]
RUN dotnet restore "TM.Api/TranslationManagement.Api.csproj"
WORKDIR "/src/TM.Api"
COPY . .
RUN dotnet build "TranslationManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TranslationManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TranslationManagement.Api.dll"]