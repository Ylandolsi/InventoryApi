# Specifies the base image for the first build stage 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["InventoryApi/InventoryApi.csproj", "InventoryApi/"]
# excute the restore command ti restore nuget packages ( dependencies )
RUN dotnet restore "InventoryApi/InventoryApi.csproj"

# Copy all the files from the current directory
# to the working directory in the container
COPY . .
# change the working directory to the InventoryApi folder
WORKDIR "/src/InventoryApi"
# build the project in release mode && output the build to /app/build
RUN dotnet build "InventoryApi.csproj" -c Release -o /app/build

# start a new stage to publish the application
# using the build stage as its base 
FROM build AS publish
# publish the application to /publish
RUN dotnet publish "InventoryApi.csproj" -c Release -o /publish

# start the final stage 
# uses the aspnet base image instead of the sdk (smaller runtime image)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# copy the published files from the publish stage 
# to the /app directory
COPY --from=publish /publish .
# expose the ports 80 and 443
# informs docker that the container listens on the specified network ports 
EXPOSE 8080
EXPOSE 443

# # loads the appsetting.Docker.json file
# ENV ASPNETCORE_ENVIRONMENT=Docker
#-----
# docker run -e ASPNETCORE_ENVIRONMENT=Development myapp
# if not set, default to Production
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
# run the application 
ENTRYPOINT ["dotnet", "InventoryApi.dll"]