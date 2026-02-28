# run from the `src` folder of the project, e.g.:
# docker build -f ItTrAp.UserService/Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

COPY ./ItTrAp.UserService/ItTrAp.UserService.csproj ./ItTrAp.UserService/
COPY ./ItTrAp.Shared/ItTrAp.Shared.csproj ./ItTrAp.Shared/

RUN dotnet restore ./ItTrAp.UserService/ItTrAp.UserService.csproj

# Copy everything
COPY ./ItTrAp.UserService/ ./ItTrAp.UserService/
COPY ./ItTrAp.Shared/ ./ItTrAp.Shared/

# Build and publish a release
RUN dotnet publish ./ItTrAp.UserService/ItTrAp.UserService.csproj -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["./ItTrAp.UserService"]