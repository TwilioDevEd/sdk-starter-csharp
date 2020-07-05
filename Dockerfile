FROM microsoft/dotnet:3.1-sdk
WORKDIR /project
COPY TwilioSdkStarterDotnetCore.sln /project/
COPY src /project/src
COPY tests /project/tests
RUN dotnet restore && dotnet test ./tests/TwilioSdkStarterDotnetCore.Tests
