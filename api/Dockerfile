FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /api
COPY *.csproj .
RUN dotnet restore --packages .nuget
COPY . .
RUN dotnet publish -c Release -o out  --packages .nuget

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
EXPOSE 8192
WORKDIR /app
COPY --from=build /api/out .

ENTRYPOINT ["dotnet", "lookaroond.dll"]