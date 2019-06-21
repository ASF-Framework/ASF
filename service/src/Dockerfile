FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
VOLUME /app/AppData

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["ASF.Web/ASF.Web.csproj", "src/ASF.Web/"]
COPY ["ASF.Core/ASF.Core.csproj", "src/ASF.Core/"]
COPY ["ASF.EntityFramework.Storage/ASF.EntityFramework.Storage.csproj", "src/ASF.EntityFramework.Storage/"]
RUN dotnet restore "src/ASF.Web/ASF.Web.csproj"
COPY . .
WORKDIR "/src/ASF.Web"
RUN dotnet build "ASF.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ASF.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ASF.Web.dll"]
