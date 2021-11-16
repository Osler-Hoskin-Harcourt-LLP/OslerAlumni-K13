namespace OslerAlumni.Mvc.Extensions.OslerControls.RichTextField.Models.InlineWidgetModels
{
    public class YouTubeInlineWidget : IInlineWidetModel
    {
        public string YouTubeEmbedId { get; set; }
        public string Name { get; set; }
        public string Transcript { get; set; }

        public string YouTubeEmbedUrl => $"https://www.youtube.com/embed/{YouTubeEmbedId}";
    }
}
