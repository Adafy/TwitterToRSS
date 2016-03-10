using Microsoft.Azure;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses;
using Nancy.TinyIoc;

namespace Adafy.TwitterToRSS
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += (ctx) =>
            {
                if (string.IsNullOrEmpty(CloudConfigurationManager.GetSetting("TWITTER_APP_ID")))
                    return new TextResponse(HttpStatusCode.PreconditionFailed, "Missing configuration parameter TWITTER_APP_ID");
                if (string.IsNullOrEmpty(CloudConfigurationManager.GetSetting("TWITTER_APP_SECRET")))
                    return new TextResponse(HttpStatusCode.PreconditionFailed, "Missing configuration parameter TWITTER_APP_SECRET");

                return null;
            };
        }
    }
}