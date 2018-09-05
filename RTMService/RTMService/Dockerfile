FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80


FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /RTMService
COPY *.csproj ./
RUN dotnet restore
COPY . .

WORKDIR /RTMService
RUN dotnet build RTMService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish RTMService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RTMService.dll"]