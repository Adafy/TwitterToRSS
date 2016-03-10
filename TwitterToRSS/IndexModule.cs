using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Azure;
using Nancy;

namespace Adafy.TwitterToRSS
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/twitter/{user}"] = (p) =>
            {
                var result = GetTwitterMessages(p.user);
                return result;
            };
        }

        public string GetTwitterMessages(string fromUser)
        {
            var searcher = new TwitterSearcher(CloudConfigurationManager.GetSetting("TWITTER_APP_ID"), CloudConfigurationManager.GetSetting("TWITTER_APP_SECRET"));
            var messages = searcher.GetMessages(fromUser);

            var feed = CreateFeed("Twitter", messages);
            var str = CreateResultString(feed);

            var result = str.ToString();

            return result;
        }

        private static SyndicationFeed CreateFeed(string feedName, List<TweetInfo> messages)
        {
            var feed = new SyndicationFeed { Title = new TextSyndicationContent(feedName) };

            var items = new List<SyndicationItem>();

            foreach (var msg in messages)
            {
                var item = new SyndicationItem { Title = new TextSyndicationContent(msg.Caption) };

                item.Authors.Add(new SyndicationPerson("", msg.FormattedName, ""));
                item.PublishDate = msg.SendTime;

                item.Links.Add(new SyndicationLink(new Uri(msg.PostUrl, UriKind.Absolute)));

                if (!string.IsNullOrWhiteSpace(msg.PictureUrl))
                {
                    item.ElementExtensions.Add(
                        new XElement("enclosure",
                            new XAttribute("type", "image/*"),
                            new XAttribute("url", msg.PictureUrl)
                        ).CreateReader()
                    );
                }

                item.Id = msg.PostUrl;
                item.Content = new TextSyndicationContent(msg.Text);

                items.Add(item);
            }

            feed.Items = items;

            return feed;
        }

        private static StringBuilder CreateResultString(SyndicationFeed feed)
        {
            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);

            StringBuilder result = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(result))
            {
                formatter.WriteTo(writer);
            }

            return result;
        }
    }
}