FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Piloto.Api.WebApi/Piloto.Api.WebApi.csproj", "Piloto.Api.WebApi/"]
COPY ["Piloto.Api.Application/Piloto.Api.Application.csproj", "Piloto.Api.Application/"]
COPY ["Piloto.Api.Application.DTO/Piloto.Api.Application.DTO.csproj", "Piloto.Api.Application.DTO/"]
COPY ["Piloto.Api.Domain.Core/Piloto.Api.Domain.Core.csproj", "Piloto.Api.Domain.Core/"]
COPY ["Piloto.Api.Domain/Piloto.Api.Domain.csproj", "Piloto.Api.Domain/"]
COPY ["Piloto.Api.Domain.Services/Piloto.Api.Domain.Services.csproj", "Piloto.Api.Domain.Services/"]
COPY ["Piloto.Api.Infrastructure.CrossCutting.Adapter/Piloto.Api.Infrastructure.CrossCutting.Adapter.csproj", "Piloto.Api.Infrastructure.CrossCutting.Adapter/"]
COPY ["Piloto.Api.Infrastructure.Data.Repository/Piloto.Api.Infrastructure.Data.Repository.csproj", "Piloto.Api.Infrastructure.Data.Repository/"]
COPY ["Piloto.Api.Infrastructure.Data/Piloto.Api.Infrastructure.Data.csproj", "Piloto.Api.Infrastructure.Data/"]
RUN dotnet restore "./Piloto.Api.WebApi/Piloto.Api.WebApi.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "./Piloto.Api.WebApi/Piloto.Api.WebApi.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "./Piloto.Api.WebApi/Piloto.Api.WebApi.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./Piloto.Api.WebApi/certs /app/certs 
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certs/aspnetcore-selfsigned.crt 
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath=/app/certs/aspnetcore-selfsigned.key
ENV ASPNETCORE_ENVIRONMENT=Docker
#ENV PATH="${PATH}:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "Piloto.Api.WebApi.dll"]