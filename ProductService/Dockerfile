FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
WORKDIR /src
COPY *.sln ./
COPY API/*.csproj ./API/
COPY Application/*.csproj ./Application/
COPY Persistence/*.csproj ./Persistence/
COPY MessagePublish/*.csproj ./MessagePublish/
COPY Domain/*.csproj ./Domain/
RUN dotnet clean
RUN dotnet restore


# copy everything else and build
COPY . ./
WORKDIR /src/API
RUN dotnet publish -c Release -o /app

WORKDIR /src/Application
RUN dotnet publish -c Release -o /app

WORKDIR /src/Persistence
RUN dotnet publish -c Release -o /app

WORKDIR /src/MessagePublish
RUN dotnet publish -c Release -o /app

WORKDIR /src/Domain
RUN dotnet publish -c Release -o /app

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app .
EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "API.dll"]
