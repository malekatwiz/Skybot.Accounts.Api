FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Skybot.Accounts.Api/Skybot.Accounts.Api.csproj", "Skybot.Accounts.Api/"]
RUN dotnet restore "Skybot.Accounts.Api/Skybot.Accounts.Api.csproj"
COPY . .
WORKDIR "/src/Skybot.Accounts.Api"
RUN dotnet build "Skybot.Accounts.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Skybot.Accounts.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Skybot.Accounts.Api.dll"]