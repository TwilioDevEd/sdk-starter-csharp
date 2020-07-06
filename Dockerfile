FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /project
COPY TwilioSdkStarterDotnetCore.sln /project/
COPY src /project/src
COPY tests /project/tests
RUN dotnet restore && dotnet test ./tests/TwilioSdkStarterDotnetCore.Tests
