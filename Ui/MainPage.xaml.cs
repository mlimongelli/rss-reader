using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Toolkit.Parsers.Rss;
using Ui.Models;

namespace Ui
{
	public partial class MainPage : ContentPage
	{
        public ObservableCollection<RssSource> RssSources { get; set; }

        public ObservableCollection<RssSchema> RssSchemas { get; set; } = new ObservableCollection<RssSchema>();

        public MainPage()
		{
			InitializeComponent();

            RssSources = new ObservableCollection<RssSource>
            {
                new RssSource
                {
                    Title = "JW.ORG",
                    Url = "https://www.jw.org/it/news/jw-news/rss/NewsSubsectionRSSFeed/feed.xml"
                }
            };

            lstItems.ItemsSource = RssSchemas;
            pkrRssSource.ItemsSource = RssSources;

            foreach (var source in RssSources) 
            {
                Parse(source.Url);
            }
        }

        public async Task Parse(string url)
        {
            string feed = null;

            using (var client = new HttpClient())
            {
                feed = await client.GetStringAsync(url);
            }

            if (feed == null) return;

            try
            {
                var parser = new RssParser();
                var rss = parser.Parse(feed);
                foreach (var i in rss) 
                {
                    RssSchemas.Add(i);
                }

                RssSchemas = new ObservableCollection<RssSchema>(RssSchemas.ToList().OrderByDescending(t => t.PublishDate));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void lstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (RssSchema)e.CurrentSelection.FirstOrDefault();
            Browser.OpenAsync(item.FeedUrl);
        }
	}
}
