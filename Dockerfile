# Use the base image for .NET 6 runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Use the .NET 6 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Medpharm.API/Medpharm.API.csproj", "Medpharm.API/"]
COPY ["Medpharm.BusinessModels/Medpharm.BusinessModels.csproj", "Medpharm.BusinessModels/"]
COPY ["Medpharm.Common/Medpharm.Common.csproj", "Medpharm.Common/"]
COPY ["Medpharm.DataAccess/Medpharm.DataAccess.csproj", "Medpharm.DataAccess/"]
COPY ["Medpharm.Services/Medpharm.Services.csproj", "Medpharm.Services/"]

# Restore dependencies
RUN dotnet restore "Medpharm.API/Medpharm.API.csproj"

# Copy the full source
COPY . .

# Build the project
RUN dotnet build "Medpharm.API/Medpharm.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Medpharm.API/Medpharm.API.csproj" -c Release -o /app/publish

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Medpharm.API.dll"]
