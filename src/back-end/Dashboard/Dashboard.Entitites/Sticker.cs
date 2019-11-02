using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboard.Entitites
{
    public class Sticker
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Text { get; set; }
        public string HtmlColor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
