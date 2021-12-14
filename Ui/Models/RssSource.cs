using System;

namespace Ui.Models
{
    public class RssSource
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid Id { get; internal set; }
    }
}
