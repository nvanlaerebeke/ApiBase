#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["sample/Sample/Sample.csproj", "sample/Sample/"]
COPY ["sample/Sample.API/Sample.API.csproj", "sample/Sample.API/"]
COPY ["src/ApiBase/ApiBase.csproj", "src/ApiBase/"]
RUN dotnet restore "sample/Sample/Sample.csproj"
COPY . .
WORKDIR "/src/sample/Sample"
RUN dotnet build "Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.dll"]