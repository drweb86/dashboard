namespace Dashboard.Entitites
{
    public class User
    {
        public int Id { get; set; }
        // from the group model (Entity framework will connect the Primarykey and forign key)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
