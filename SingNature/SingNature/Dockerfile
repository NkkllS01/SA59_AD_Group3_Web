# Use official ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8000

# Use .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SingNature/SingNature/SingNature.csproj", "SingNature/SingNature/"]
RUN dotnet restore "/src/SingNature/SingNature/SingNature.csproj"

COPY . .
WORKDIR "/src/SingNature/SingNature"
RUN dotnet build "SingNature.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "SingNature.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Disable HTTPS inside Docker
ENV ASPNETCORE_URLS=http://+:8000

ENTRYPOINT ["dotnet", "SingNature.dll"]
