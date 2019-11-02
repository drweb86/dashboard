namespace Dashboard.Dtos.Stickers
{
    public class StickerUpdateDto
    {
        // public int Id { get; } Because of automapper BUG, it ignores fields starting with Id for some reason.
        public int ItemId { get; set; }
        public string Text { get; set; }
        public string HtmlColor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
