#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/MessageService/MessageService.csproj", "src/MessageService/"]
COPY ["src/MessageService.Data.Provider.MsSql.Ef/MessageService.Data.Provider.MsSql.Ef.csproj", "src/MessageService.Data.Provider.MsSql.Ef/"]
COPY ["src/MessageService.Data.Provider/MessageService.Data.Provider.csproj", "src/MessageService.Data.Provider/"]
COPY ["src/MessageService.Models.Db/MessageService.Models.Db.csproj", "src/MessageService.Models.Db/"]
COPY ["src/MessageService.Validation/MessageService.Validation.csproj", "src/MessageService.Validation/"]
COPY ["src/MessageService.Models.Dto/MessageService.Models.Dto.csproj", "src/MessageService.Models.Dto/"]
COPY ["src/MessageService.Business/MessageService.Business.csproj", "src/MessageService.Business/"]
COPY ["src/MessageService.Mappers/MessageService.Mappers.csproj", "src/MessageService.Mappers/"]
COPY ["src/MessageService.Models.Broker/MessageService.Models.Broker.csproj", "src/MessageService.Models.Broker/"]
COPY ["src/MessageService.Data/MessageService.Data.csproj", "src/MessageService.Data/"]
COPY ["src/MessageService.Broker/MessageService.Broker.csproj", "src/MessageService.Broker/"]
RUN dotnet restore "src/MessageService/MessageService.csproj"
COPY . .
WORKDIR "/src/src/MessageService"
RUN dotnet build "MessageService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LT.DigitalOffice.MessageService.dll"]