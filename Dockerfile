﻿FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env
ARG PROJECT_NAME
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore "${PROJECT_NAME}/${PROJECT_NAME}.csproj"

RUN dotnet publish "${PROJECT_NAME}/${PROJECT_NAME}.csproj" -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.1

ARG PROJECT_NAME
ENV DLL_NAME="${PROJECT_NAME}.dll"

WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["sh","-c","dotnet ${DLL_NAME}"]
