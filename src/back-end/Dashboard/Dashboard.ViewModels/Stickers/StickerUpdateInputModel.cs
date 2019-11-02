namespace Dashboard.ViewModels.Stickers
{
    public class StickerUpdateInputModel
    {
        // public int Id { get; set; } because of automapper bug, it ignores all fields starting with Id.
        public int ItemId { get; set; }
        public string Text { get; set; }
        public string HtmlColor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
