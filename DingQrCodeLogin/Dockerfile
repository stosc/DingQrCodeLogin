FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY DingQrCodeLogin/DingQrCodeLogin.csproj DingQrCodeLogin/
RUN dotnet restore "DingQrCodeLogin/DingQrCodeLogin.csproj"
COPY . .
WORKDIR "/src/DingQrCodeLogin"
RUN dotnet build "DingQrCodeLogin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DingQrCodeLogin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DingQrCodeLogin.dll"]
