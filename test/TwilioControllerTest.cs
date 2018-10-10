using System.Collections.Generic;
using sdk_starter_dotnet_core.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Twilio.Rest.Notify.V1.Service;
using Xunit;

namespace sdk_starter_dotnet_core.Test
{
    public class TwilioControllerTest
    {
        private static readonly Dictionary<string, string> _options = new Dictionary<string, string>
            {
                { "TWILIO_ACCOUNT_SID", "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                { "TWILIO_API_KEY", "SKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                { "TWILIO_API_SECRET", "aXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                { "TWILIO_CHAT_SERVICE_SID", "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                { "TWILIO_SYNC_SERVICE_SID", "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" },
                { "TWILIO_NOTIFICATION_SERVICE_SID", "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" }

            };
        private static IOptions<AppSettings> _ctrlOptions;
        private static AppSettings _appSettings;

        public TwilioControllerTest()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(_options)
                .Build();

            _appSettings = new AppSettings();
            config.Bind(_appSettings);
            _ctrlOptions = Options.Create(_appSettings);
        }

        [Fact]
        public void Config_ReturnsTwilioConfig()
        {
            // Arrange
            var controller = new TwilioController(_ctrlOptions);

            // Act
            var result = controller.Config();

            // Assert
            Assert.Equal(result.Value, _appSettings);
        }

        [Fact]
        public void Token_ReturnsATokenAndIdentity()
        {
            // Arrange
            var controller = new TwilioController(_ctrlOptions);

            // Act
            var result = controller.Token();
            var tokenResult = (Dictionary<string, string>)result.Value;

            // Assert
            Assert.True(tokenResult.ContainsKey("token"));
            Assert.True(tokenResult.ContainsKey("identity"));
        }

        [Fact]
        public void Register_CreatesBinding()
        {
            // Arrange
            var controller = new Mock<TwilioController> (_ctrlOptions) { CallBase = true };

            controller.Setup(x => x.CreateNotificationServiceBinding(It.IsAny<RegisterRequest>()))
                .Returns(BindingResource.FromJson(""));

            var request = new RegisterRequest()
            {
                Address = "test_address",
                BindingType = "fcm",
                identity = "000001"
            };

            // Act
            var result = controller.Object.Register(request);

            // Assert
            var registerResult = (Dictionary<string, string>)result.Value;
            Assert.Equal(registerResult["message"], "Binding Created!");
        }

        [Fact]
        public void SendNotification_SendsNotification()
        {
            // Arrange
            var controller = new Mock<TwilioController> (_ctrlOptions) { CallBase = true };

            controller.Setup(x => x.CreateNotification(It.IsAny<string>()))
                .Returns(NotificationResource.FromJson(""));

            var sendNotificationRequest = new SendNotificationRequest()
            {
                identity = "0000001"
            };

            // Act
            var result = controller.Object.SendNotification(sendNotificationRequest);

            // Assert
            var sendNotificationResult = (Dictionary<string, string>)result.Value;
            Assert.Equal(sendNotificationResult["message"], "Successful sending of notification.");
        }
    }
}
