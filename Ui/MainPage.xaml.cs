using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Toolkit.Parsers.Rss;

namespace Ui
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            var task = Parse("https://www.jw.org/it/news/jw-news/rss/NewsSubsectionRSSFeed/feed.xml");
        }

        public async Task<IEnumerable<RssSchema>> Parse(string url)
        {
            string feed = null;

            using (var client = new HttpClient())
            {
                feed = await client.GetStringAsync(url);
            }

            if (feed == null) return new List<RssSchema>();

            try
            {
                var parser = new RssParser();
                var rss = parser.Parse(feed);
                lstItems.ItemsSource = rss.ToList().OrderByDescending(t => t.PublishDate);
                return rss;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (RssSchema)e.CurrentSelection.FirstOrDefault();
            Browser.OpenAsync(item.FeedUrl);
        }
	}
}
