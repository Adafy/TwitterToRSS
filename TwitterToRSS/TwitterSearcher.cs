using System.Collections.Generic;
using System.Linq;
using LinqToTwitter;

namespace Adafy.TwitterToRSS
{
    public class TwitterSearcher
    {
        private static string _twitterAppId;
        private static string _twitterAppSecret;

        public TwitterSearcher(string twitterAppId, string twitterAppSecret)
        {
            _twitterAppId = twitterAppId;
            _twitterAppSecret = twitterAppSecret;
        }

        public List<TweetInfo> GetMessages(string fromUser)
        {
            var authorizer = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = _twitterAppId,
                    ConsumerSecret = _twitterAppSecret
                },
                AccessType = AuthAccessType.Read
            };

            authorizer.AuthorizeAsync().Wait();
            var ctx = new TwitterContext(authorizer);

            var formattedTag = "from:" + fromUser;

            var response = ctx.Search.Where(_ => _.Type == SearchType.Search && _.Query == formattedTag).ToList().SelectMany(_ => _.Statuses);

            var pictures = new List<TweetInfo>();

            foreach (var tweet in response)
            {
                var tags = tweet.Entities.HashTagEntities.Select(t => t.Tag);
                var url = $"http://twitter.com/{fromUser}/statuses/{tweet.StatusID}";

                var pictureInfo = new TweetInfo()
                {
                    Caption = tweet.Text,
                    PictureUrl = tweet.RetweetedStatus.User == null ? tweet.User.ProfileImageUrl : tweet.RetweetedStatus.User.ProfileImageUrl,
                    PostUrl = url,
                    SenderUserName = tweet.User.ScreenNameResponse,
                    SenderName = tweet.User.Name,
                    SendTime = tweet.CreatedAt.ToLocalTime(),
                    Text = tweet.Text
                };

                pictureInfo.HashTags.AddRange(tags);

                pictures.Add(pictureInfo);
            }

            return pictures;
        }
    }
}