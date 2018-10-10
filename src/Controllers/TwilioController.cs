using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Notify.V1.Service;
using Twilio.Rest.Sync.V1;

namespace sdk_starter_dotnet_core.Controllers
{
    [Produces("application/json")]
    public class TwilioController : Controller
    {
        private readonly AppSettings _appSettings;

        public TwilioController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            // if the Sync Service SID is blank, use the default Sync Service instead
            if (_appSettings.TWILIO_SYNC_SERVICE_SID == String.Empty) {
                _appSettings.TWILIO_SYNC_SERVICE_SID = "default";
            }

            TwilioClient.Init(
                _appSettings.TWILIO_API_KEY,
                _appSettings.TWILIO_API_SECRET,
                _appSettings.TWILIO_ACCOUNT_SID
            );

            // Ensure that the Sync Default Service is provisioned
            ProvisionSyncDefaultService(_appSettings.TWILIO_SYNC_SERVICE_SID);
        }

        [HttpGet("/config")]
        public JsonResult Config()
        {
            return new JsonResult(_appSettings);
        }

        [HttpGet("/token")]
        public JsonResult Token()
        {
            // This can be tracked internally by your web app.
            var identity = randomUserId();


            var grants = new HashSet<IGrant>();

            var videoGrant = new VideoGrant();

            grants.Add(videoGrant);

            if (_appSettings.TWILIO_CHAT_SERVICE_SID != String.Empty)
            {
                // Create a "grant" which enables a client to use IPM as a given user,
                // on a given device.
                var chatGrant = new ChatGrant()
                {
                    ServiceSid = _appSettings.TWILIO_CHAT_SERVICE_SID,
                };

                grants.Add(chatGrant);
            }

            if (_appSettings.TWILIO_SYNC_SERVICE_SID != String.Empty)
            {
                var syncGrant = new SyncGrant()
                {
                    ServiceSid = _appSettings.TWILIO_SYNC_SERVICE_SID
                };

                grants.Add(syncGrant);
            }

            var token = new Token(
                _appSettings.TWILIO_ACCOUNT_SID,
                _appSettings.TWILIO_API_KEY,
                _appSettings.TWILIO_API_SECRET,
                identity,
                grants: grants
            ).ToJwt();

            return new JsonResult(new Dictionary<string, string>()
            {
                {"identity", identity},
                {"token", token}
            });
        }

        [HttpPost("/register")]
        public JsonResult Register([FromBody] RegisterRequest request)
        {
            CreateNotificationServiceBinding(request);

            return new JsonResult(new Dictionary<string, string>()
                {
                    {"message", "Binding Created!"}
                }
            );
        }

        [HttpPost("/send-notification/")]
        public JsonResult SendNotification(SendNotificationRequest request)
        {
            CreateNotification(request.identity);

            return new JsonResult(new Dictionary<string, string>()
            {
                {"message", "Successful sending of notification."}
            });
        }

        public virtual NotificationResource CreateNotification(string identity)
        {
            // there is only one identity/device this notification will be sent
            // to. However, this method requires a list. In case there is more than one.
            var identities = new List<string>()
            {
                identity
            };

            return NotificationResource.Create(
                _appSettings.TWILIO_NOTIFICATION_SERVICE_SID,
                identities,
                body: "Hello, world!"
            );
        }

        public virtual BindingResource CreateNotificationServiceBinding(RegisterRequest request)
        {
            // This dumpster fire of a switch statement should be temporary
            // until we introduce a way to parse the enum
            BindingResource.BindingTypeEnum bindingType;
            switch (request.BindingType.ToLower())
            {
                case "apn":
                    bindingType = BindingResource.BindingTypeEnum.Apn;
                    break;
                case "fcm":
                    bindingType = BindingResource.BindingTypeEnum.Fcm;
                    break;
                case "gcm":
                    bindingType = BindingResource.BindingTypeEnum.Gcm;
                    break;
                case "sms":
                    bindingType = BindingResource.BindingTypeEnum.Sms;
                    break;
                case "facebookmessenger":
                    bindingType = BindingResource.BindingTypeEnum.FacebookMessenger;
                    break;
                case "alexa":
                    bindingType = BindingResource.BindingTypeEnum.Alexa;
                    break;
                default:
                    throw new Exception("Unknown binding type.");
            }


            return BindingResource.Create(
                pathServiceSid: _appSettings.TWILIO_NOTIFICATION_SERVICE_SID,
                identity: request.identity,
                bindingType: bindingType,
                address: request.Address
            );
        }

        private void ProvisionSyncDefaultService(string serviceSid)
        {
            if (_appSettings.TWILIO_SYNC_SERVICE_SID.Equals("default"))
            {
                ServiceResource.Fetch("default");
            }
        }

        private string randomUserId()
        {
            return Guid.NewGuid().ToString();
        }

        public static String TitleCaseString(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }
    }
}
