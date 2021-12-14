using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using Ui.Models;
using Application = Microsoft.Maui.Controls.Application;

namespace Ui
{
    public partial class App : Application
	{
		public static ObservableCollection<RssSource> RssSources { get; set; }

		public App()
		{
			InitializeComponent();

			RssSources = new ObservableCollection<RssSource>
			{
				new RssSource
				{
					Id = Guid.NewGuid(),
					Title = "JW.ORG",
					Url = "https://www.jw.org/it/news/jw-news/rss/NewsSubsectionRSSFeed/feed.xml"
				},
				new RssSource
				{
					Id = Guid.NewGuid(),
					Title = "Sviluppi legali",
					Url= "https://www.jw.org/it/news/sviluppi-legali-diritti-umani/rss/NewsSubsectionRSSFeed/feed.xml"
				}
			};

			MainPage = new NavigationPage();
			MainPage.Navigation.PushAsync(new MainPage());
		}
	}
}
