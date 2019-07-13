FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["todobackend/todobackend.csproj", "todobackend/"]
RUN dotnet restore "todobackend/todobackend.csproj"
COPY . .
WORKDIR "/src/todobackend"
RUN dotnet build "todobackend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "todobackend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "todobackend.dll"]