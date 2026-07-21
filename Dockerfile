FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProjectManagement/ProjectManagement.csproj", "ProjectManagement/"]
RUN dotnet restore "ProjectManagement/ProjectManagement.csproj"
COPY . .
WORKDIR "/src/ProjectManagement"
RUN dotnet publish "ProjectManagement.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProjectManagement.dll"]
