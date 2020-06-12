using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Notify.V1.Service;
using Twilio.Rest.Sync.V1;
using TwilioSdkStarterDotnetCore.Web.Models;

namespace TwilioSdkStarterDotnetCore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly TwilioAccount _twilioAccount;

        public UtilitiesController(IOptions<TwilioAccount> twilioAccount)
        {
            if (twilioAccount == null)
            {
                throw new ArgumentNullException(nameof(twilioAccount));
            }
            _twilioAccount = twilioAccount.Value;

            // if the Sync Service SID is blank, use the default Sync Service instead
            if (_twilioAccount.SyncServiceSid == string.Empty)
            {
                _twilioAccount.SyncServiceSid = "default";
            }
            TwilioClient.Init(
                    _twilioAccount.ApiKey,
                    _twilioAccount.ApiSecret,
                    _twilioAccount.AccountSid
                );

            // Ensure that the Sync Default Service is provisioned
            ProvisionSyncDefaultService(_twilioAccount.SyncServiceSid);

        }

        [HttpGet("/token")]
        public JsonResult Token()
        {
            // This can be tracked internally by your web app.
            var identity = randomUserId();

            return CreateTokenResult(identity);
        }

        [HttpGet("/token/{identity:alpha}")]
        public JsonResult TokenForIdentity(string identity)
        {
            return CreateTokenResult(identity);
        }

        private JsonResult CreateTokenResult(string identity) 
        {

            var grants = new HashSet<IGrant>();

            var videoGrant = new VideoGrant();

            grants.Add(videoGrant);

            if (_twilioAccount.ChatServiceSid != String.Empty)
            {
                // Create a "grant" which enables a client to use IPM as a given user,
                // on a given device.
                var chatGrant = new ChatGrant()
                {
                    ServiceSid = _twilioAccount.ChatServiceSid
                };

                grants.Add(chatGrant);
            }

            if (_twilioAccount.SyncServiceSid != String.Empty)
            {
                var syncGrant = new SyncGrant()
                {
                    ServiceSid = _twilioAccount.SyncServiceSid
                };

                grants.Add(syncGrant);
            }

            var token = new Token(

                _twilioAccount.AccountSid,
                _twilioAccount.ApiKey,
                _twilioAccount.ApiSecret,
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
            var _ = CreateNotificationServiceBinding(request);

            return new JsonResult(new Dictionary<string, string>()
                {
                    {"message", "Binding Created!"}
                }
            );
        }
        private void ProvisionSyncDefaultService(string serviceSid)
        {
            if (serviceSid.Equals("default"))
            {
                ServiceResource.Fetch("default");
            }
        }
        private string randomUserId()
        {
            return Guid.NewGuid().ToString();
        }

        private BindingResource CreateNotificationServiceBinding(RegisterRequest request)
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
                pathServiceSid: _twilioAccount.NotificationServiceSid,
                identity: request.Identity,
                bindingType: bindingType,
                address: request.Address
            );
        }
    }
}