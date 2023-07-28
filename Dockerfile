FROM node:16 as npm-stage
WORKDIR /app
COPY src/app/package*.json ./
RUN npm install
COPY src/app ./
RUN npm run build

# Stage 2: Build the .NET Core app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dotnet-stage
ARG REVISION
WORKDIR /webapi
COPY src/webapi/*.csproj ./
RUN dotnet restore
COPY src/webapi ./
RUN dotnet publish -c Release -o /app /p:VersionSuffix=$REVISION-$(date +'%m%d%H%M')

FROM mcr.microsoft.com/dotnet/aspnet:7.0
RUN echo 'APPINFO=$(dotnet testapi.dll --version) && \
  echo -e "Application\\t: $(sed -n 1p <<< "$APPINFO")" && \
  echo -e "Version\\t\\t: $(sed -n 2p <<< "$APPINFO")" && \
  echo -e "Revision\\t: $(sed -n 3p <<< "$APPINFO")" && \
  echo -e "Build time\\t: $(sed -n 4p <<< "$APPINFO")"' >> /root/.bashrc
WORKDIR /app
COPY --from=dotnet-stage /app .
COPY --from=npm-stage /app/build ./wwwroot

EXPOSE 8080
EXPOSE 9464
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "RecipeGen.dll"]
