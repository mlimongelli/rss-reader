using Microsoft.Toolkit.Parsers.Rss;
using System;

namespace Ui.Models
{
    public class RssEntry
    {
        public Guid SourceId { get; set; }

        public DateTime PublishDate { get; set; }

        public string Summary { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
