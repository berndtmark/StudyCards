ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS with-node
RUN apt-get update
RUN apt-get -y install curl
RUN curl -sL https://deb.nodesource.com/setup_24.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

## API ##

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StudyCards.Api/StudyCards.Api.csproj", "StudyCards.Api/"]
RUN dotnet restore "./StudyCards.Api/StudyCards.Api.csproj"
COPY . .
WORKDIR "/src/StudyCards.Api"
RUN dotnet build "./StudyCards.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StudyCards.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

## UI ##

FROM with-node AS copyui
WORKDIR /src
COPY ["StudyCards.UI/studycardsui", "studycards.client/"]

FROM copyui AS buildui
WORKDIR /src/studycards.client/
RUN npm install
RUN npm run build:prod

## Final ##
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=buildui /src/studycards.client/dist/studycardsui/browser wwwroot/
ENTRYPOINT ["dotnet", "StudyCards.Api.dll"]
