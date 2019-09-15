FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
VOLUME /app/AppData

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/ASF.Web/ASF.Web.csproj", "src/ASF.Web/"]
COPY ["src/ASF.Core/ASF.Core.csproj", "src/ASF.Core/"]
COPY ["src/ASF.EntityFramework.Storage/ASF.EntityFramework.Storage.csproj", "src/ASF.EntityFramework.Storage/"]
RUN dotnet restore "src/ASF.Web/ASF.Web.csproj"
COPY . .
WORKDIR "/src/src/ASF.Web"
RUN dotnet build "ASF.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ASF.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASF.Web.dll"]