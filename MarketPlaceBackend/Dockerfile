FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 4000
EXPOSE 8000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /Verification
COPY *.csproj ./
RUN dotnet restore
COPY . .

WORKDIR /Verification
RUN dotnet build MarketPlaceBackend.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MarketPlaceBackend.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MarketPlaceBackend.dll"]