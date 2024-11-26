FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Fiap.McTech.Cart.Api/Fiap.McTech.Cart.Api.csproj", "Fiap.McTech.Cart.Api/"]
RUN dotnet restore "./src/Fiap.McTech.Cart.Api/Fiap.McTech.Cart.Api.csproj"
WORKDIR "/src/Fiap.McTech.Cart.Api"
RUN dotnet build "./Fiap.McTech.Cart.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fiap.McTech.Cart.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN addgroup --system nonroot \
    && adduser --system --ingroup nonroot nonroot
WORKDIR /app
COPY --from=publish /app/publish .
USER nonroot
ENTRYPOINT ["dotnet", "Fiap.McTech.Cart.Api.dll"]
