FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
ENV ASPNETCORE_ENVIRONMENT="development"
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BACK/BACK.csproj", "BACK/"]
RUN dotnet restore "BACK/BACK.csproj"
COPY . .
WORKDIR "/src/BACK"
RUN dotnet build "BACK.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BACK.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
EXPOSE 5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BACK.dll"]