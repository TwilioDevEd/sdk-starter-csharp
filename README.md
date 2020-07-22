<a href="https://www.twilio.com">
  <img src="https://static0.twilio.com/marketing/bundles/marketing/img/logos/wordmark-red.svg" alt="Twilio" width="250" />
</a>

# Twilio SDK Starter Application for C#

[![Build Status](https://travis-ci.org/TwilioDevEd/sdk-starter-csharp.svg?branch=master)](https://travis-ci.org/TwilioDevEd/sdk-starter-csharp)

## .NET Core 3.1 Project

This sample project demonstrates how to use Twilio APIs in a C# web
application. Once the app is up and running, check out [the home page](http://localhost:3000)
to see which demos you can run. You'll find examples for [Chat](https://www.twilio.com/chat),
[Video](https://www.twilio.com/video), [Sync](https://www.twilio.com/sync), and more.

Let's get started!

NOTE: This project requires Visual Studio 2019, or .NET Core 3.1 installed on your computer.

## Setup

1. Install [.NET Core](https://www.microsoft.com/net/core).
2. Clone this repository:
    ```bash
    git clone https://github.com/TwilioDevEd/sdk-starter-csharp.git
    ```

## Configure the sample application

To run the application, you'll need to gather your Twilio account credentials and configure them
in User Secrets via the `secrets.json` file. If you are unsure how to do this, check out this blog post on [User Secrets](https://www.twilio.com/blog/2018/05/user-secrets-in-a-net-core-web-app.html).
These credentials should mirror those in the `appsettings.json` file found in the root of the `TwilioSdkStarterDotnetCore.Web` project.

### Configure account information

Every sample in the demo requires some basic credentials from your Twilio account. Configure these first.

| Config Value       | Description                                                                                                           |
| :----------------- | :-------------------------------------------------------------------------------------------------------------------- |
| `TwilioAccount:AccountSid` | Your primary Twilio account identifier - find this [in the console here](https://www.twilio.com/console).             |
| `TwilioAccount:ApiKey`     | Used to authenticate - [generate one here](https://www.twilio.com/console/dev-tools/api-keys).                        |
| `TwilioAccount:ApiSecret`  | Used to authenticate - [just like the above, you'll get one here](https://www.twilio.com/console/dev-tools/api-keys). |

To set a configuration value, use the `dotnet` command line:

```bash
cd sdk-starter-dotnet-core/src/TwilioSdkStarterDotnetCore.Web
dotnet user-secrets set "TwilioAccount:AccountSid" "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
dotnet user-secrets set "TwilioAccount:ApiKey" "SKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
dotnet user-secrets set "TwilioAccount:ApiSecret" "xxxxxxxxxxxxxxxxxxxxxxxx"
```

#### A Note on API Keys

When you generate an API key pair at the URLs above, your API Secret will only be shown once -
make sure to save this information in a secure location.

## Configure product-specific settings

Depending on which demos you'd like to run, you may need to configure a few more values.

### Configuring Twilio Sync

Twilio Sync works out of the box, using default settings per account.

### Configuring Twilio Chat

In addition to the above, you'll need to [generate a Chat Service](https://www.twilio.com/console/chat/services) in the Twilio Console. Put the result in your secrets.

| Config Value           | Where to get one.                                                                       |
| :--------------------- | :-------------------------------------------------------------------------------------- |
| `TwilioAccount:ChatServiceSid` | [Generate one in the Twilio Chat console](https://www.twilio.com/console/chat/services) |

```bash
cd sdk-starter-csharp/src/TwilioSdkStarterDotnetCore.Web
dotnet user-secrets set "TwilioAccount:ChatServiceSid" "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
```

### Configuring Twilio Notify

You will need to create a Notify Service and add at least one credential on the [Mobile Push Credential screen](https://www.twilio.com/console/notify/credentials) (such as Apple Push Notification Service or Firebase Cloud Messaging for Android) to send notifications using Notify.

| Config Value                   | Where to get one.                                                                                                                  |
| :----------------------------- | :--------------------------------------------------------------------------------------------------------------------------------- |
| `TwilioAccount:NotificationServiceSid` | Generate one in the [Notify Console](https://www.twilio.com/console/notify/services). |
| A Push Credential              | Generate one with Apple or Google and [configure it as a Notify credential](https://www.twilio.com/console/notify/credentials).    |

```bash
cd sdk-starter-csharp/src/TwilioSdkStarterDotnetCore.Web
dotnet user-secrets set "TwilioAccount:NotificationServiceSid" "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
```

## Run The Application

### Visual Studio

Open `TwilioSdkStarterDotnetCore.sln` and press _F5_ or click the Run button

### Windows CLI, OS X or Linux

```bash
cd sdk-starter-csharp/src/TwilioSdkStarterDotnetCore.Web
dotnet restore
dotnet run
```

Your application should now be running at [http://localhost:50768/](http://localhost:50768/).

![Home Screen](https://cloud.githubusercontent.com/assets/809856/23171215/8107bd9e-f817-11e6-94c5-2b132d798fae.png)

Check your config values, and follow the links to the demo applications!

## Running the SDK Starter Kit with ngrok

If you are going to connect to this SDK Starter Kit with a mobile app (and you should try it out!), your phone won't be able to access localhost directly. You'll need to create a publicly accessible URL using a tool like [ngrok](https://ngrok.com/) to send HTTP/HTTPS traffic to a server running on your localhost. Use HTTPS to make web connections that retrieve a Twilio access token.

```bash
ngrok http 50768
```

## Run Tests

```bash
dotnet test tests/TwilioSdkStarterDotnetCore.Tests
```

## License

MIT
