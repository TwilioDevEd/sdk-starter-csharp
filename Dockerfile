FROM microsoft/dotnet:2.1-sdk
WORKDIR /project
COPY TwilioSdkStarterDotnetCore.sln /project/
COPY src /project/src
COPY tests /project/tests
RUN dotnet restore && dotnet test ./tests/TwilioSdkStarterDotnetCore.Tests
