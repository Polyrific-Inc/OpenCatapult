FROM microsoft/dotnet:aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 63042
EXPOSE 44306

FROM microsoft/dotnet:sdk AS build
WORKDIR /src
COPY src/API/Polyrific.Catapult.Api/Polyrific.Catapult.Api.csproj src/API/Polyrific.Catapult.Api/
COPY src/Shared/Polyrific.Catapult.Shared.Common/Polyrific.Catapult.Shared.Common.csproj src/Shared/Polyrific.Catapult.Shared.Common/
COPY src/API/Polyrific.Catapult.Api.Core/Polyrific.Catapult.Api.Core.csproj src/API/Polyrific.Catapult.Api.Core/
COPY src/Shared/Polyrific.Catapult.Shared.Dto/Polyrific.Catapult.Shared.Dto.csproj src/Shared/Polyrific.Catapult.Shared.Dto/
COPY src/API/Polyrific.Catapult.Api.Infrastructure/Polyrific.Catapult.Api.Infrastructure.csproj src/API/Polyrific.Catapult.Api.Infrastructure/
COPY src/API/Polyrific.Catapult.Api.Data/Polyrific.Catapult.Api.Data.csproj src/API/Polyrific.Catapult.Api.Data/
COPY src/Shared/Polyrific.Catapult.Shared.SmtpEmailNotification/Polyrific.Catapult.Shared.SmtpEmailNotification.csproj src/Shared/Polyrific.Catapult.Shared.SmtpEmailNotification/
COPY src/API/Polyrific.Catapult.Api.SecretVault/Polyrific.Catapult.Api.SecretVault.csproj src/API/Polyrific.Catapult.Api.SecretVault/
RUN dotnet restore src/API/Polyrific.Catapult.Api/Polyrific.Catapult.Api.csproj
COPY . .
WORKDIR /src/src/API/Polyrific.Catapult.Api
RUN dotnet build Polyrific.Catapult.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Polyrific.Catapult.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ocapi.dll"]
