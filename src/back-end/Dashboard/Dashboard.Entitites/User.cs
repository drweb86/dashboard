using System.Collections.Generic;

namespace Dashboard.Entitites
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ICollection<Sticker> Stickers { get; set; }
    }
}
