# Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore deps; optimized for Docker's Build Cache
COPY ["JuscraftBot.sln", "."]
COPY ["JuscraftBot/JuscraftBot.csproj", "JuscraftBot/"]
RUN dotnet restore "JuscraftBot/JuscraftBot.csproj"

COPY . .
WORKDIR /src/JuscraftBot

# Create a release build
RUN dotnet publish "JuscraftBot.csproj" -c Release -o /app/publish --no-restore

# Create final runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Command to run when the container starts.
ENTRYPOINT ["dotnet", "JuscraftBot.dll"]