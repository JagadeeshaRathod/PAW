# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Set the ASP.NET Core URL to port 7152
ENV ASPNETCORE_URLS=http://+:7152

EXPOSE 7152

ENTRYPOINT ["dotnet", "swas.UI.dll"]
