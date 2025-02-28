# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudyCards.Server/StudyCards.Server.csproj", "StudyCards.Server/"]
RUN dotnet restore "./StudyCards.Server/StudyCards.Server.csproj"
COPY . .
WORKDIR "/src/StudyCards.Server"
RUN dotnet build "./StudyCards.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

###

FROM with-node AS copyui
WORKDIR /src
COPY ["StudyCards.UI/studycardsui", "studycards.client/"]



FROM copyui AS buildui
WORKDIR /src/studycards.client/
RUN npm install
RUN npm run build

###


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StudyCards.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=buildui /src/studycards.client/dist/studycardsui/browser wwwroot/
ENTRYPOINT ["dotnet", "StudyCards.Server.dll"]
