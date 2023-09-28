#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5400
EXPOSE 5401

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["collabBackend/collabBackend.csproj", "collabBackend/"]
COPY ["collabDB/collabDB.csproj", "collabDB/"]
RUN dotnet restore "collabBackend/collabBackend.csproj"
COPY . .
WORKDIR "/src/collabBackend"
RUN dotnet build "collabBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "collabBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "collabBackend.dll"]