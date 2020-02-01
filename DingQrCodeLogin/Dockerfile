FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY DingQrCodeLogin/DingQrCodeLogin.csproj DingQrCodeLogin/
COPY DingQrCodeLogin/nuget.config DingQrCodeLogin/
RUN dotnet restore "DingQrCodeLogin/DingQrCodeLogin.csproj" --source https://nuget.cdn.azure.cn/v3/index.json
COPY . .
WORKDIR "/src/DingQrCodeLogin"
RUN dotnet build "DingQrCodeLogin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DingQrCodeLogin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DingQrCodeLogin.dll"]
