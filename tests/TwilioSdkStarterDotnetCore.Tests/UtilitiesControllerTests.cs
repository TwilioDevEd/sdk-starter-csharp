using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using TwilioSdkStarterDotnetCore.Web.Controllers;
using TwilioSdkStarterDotnetCore.Web.Models;


namespace TwilioSdkStarterDotnetCore.Tests
{
  [TestFixture]
  public class UtilitiesControllerTests
  {
    private TwilioAccount _twilioAccount;

    private UtilitiesController _sut;

    [SetUp]
    public void Setup()
    {
      _twilioAccount = new TwilioAccount
      {
        AuthToken = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        AccountSid = "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ApiKey = "SKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ApiSecret = "aXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        ChatServiceSid = "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        NotificationServiceSid = "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
        SyncServiceSid = "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
      };

      var options = Options.Create(_twilioAccount);

      _sut = new UtilitiesController(options);
    }

    [Test]
    public void GivenNullOptions_Should_ThrowException()
    {
      Assert.That(
          () => new UtilitiesController(null),
          Throws.ArgumentNullException.With.Message.Contains("Parameter 'twilioAccount'"));
    }

    [Test]
    public void Token_Returns_ExpectedJsonResult()
    {
      var result = _sut.Token();
      var tokenResult = (Dictionary<string, string>)result.Value;

      Assert.True(tokenResult.ContainsKey("token"));
      Assert.True(tokenResult.ContainsKey("identity"));
    }
  }
}
