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
        public IList<RssEntry> AllEntries = new List<RssEntry>();
        public ObservableCollection<RssEntry> RssEntries { get; set; } = new ObservableCollection<RssEntry>();

        public MainPage()
		{
			InitializeComponent();

            pkrRssSource.ItemsSource = App.RssSources;

            foreach (var source in App.RssSources) 
            {
                Parse(source);
            }
            lstItems.ItemsSource = RssEntries;
        }

        public void Parse(RssSource source)
        {
            string feed = null;

            using (var client = new HttpClient())
            {
                feed = client.GetStringAsync(source.Url).Result;
            }

            if (feed == null) return;

            try
            {
                var parser = new RssParser();
                var rss = parser.Parse(feed);
                foreach (var i in rss)
                {
                    AllEntries.Add(new RssEntry
                    {
                        SourceId = source.Id,
                        PublishDate = i.PublishDate,
                        Summary = i.Summary,
                        Title = i.Title,
                        ImageUrl = i.ImageUrl,
                    });
                }

                RssEntries = new ObservableCollection<RssEntry>(AllEntries.OrderByDescending(t => t.PublishDate));
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

        private void btnAddSource_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSourcePage());
        }

        private void pkrRssSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSource = pkrRssSource.SelectedItem as RssSource;

            RssEntries.Clear();
            RssEntries = new ObservableCollection<RssEntry>(
                AllEntries.Where(t => t.SourceId == selectedSource.Id).OrderByDescending(t => t.PublishDate));
            lstItems.ItemsSource = RssEntries;
        }
    }
}
