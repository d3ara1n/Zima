#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:927
EXPOSE 927

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Zima/Zima.csproj", "Zima/"]
RUN dotnet restore "Zima/Zima.csproj"
COPY . .
WORKDIR "/src/Zima"
RUN dotnet build "Zima.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zima.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zima.dll"]