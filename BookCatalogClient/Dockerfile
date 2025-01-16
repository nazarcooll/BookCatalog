FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookCatalogClient/BookCatalogClient.csproj", "BookCatalogClient/"]
RUN dotnet restore "./BookCatalogClient/BookCatalogClient.csproj"
COPY . .
WORKDIR "/src/BookCatalogClient"
RUN dotnet build BookCatalogClient.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish BookCatalogClient.csproj -c $BUILD_CONFIGURATION -o /app/publish

FROM nginx:alpine AS final
COPY --from=build /src/BookCatalogClient/nginx.conf etc/nginx/nginx.conf
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
ENTRYPOINT ["nginx", "-g", "daemon off;"]