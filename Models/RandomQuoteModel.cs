using System.Diagnostics.CodeAnalysis;

namespace ErrorReportsGui.Models
{
    public class Originator
    {

        public string? id { get; set; }
        public string? language_code { get; set; }
        public string? description { get; set; }
        public string? name { get; set; }
        public string? url { get; set; }
    }

    public class RandomQuoteModel
    {

        public string? content { get; set; }

        public Originator? originator { get; set; }
    }
}