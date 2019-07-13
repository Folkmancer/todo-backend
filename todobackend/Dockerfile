FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["todo-backend/todo-backend.csproj", "todo-backend/"]
RUN dotnet restore "todo-backend/todo-backend.csproj"
COPY . .
WORKDIR "/src/todo-backend"
RUN dotnet build "todo-backend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "todo-backend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "todo-backend.dll"]