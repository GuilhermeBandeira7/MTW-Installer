namespace EntityMtwServer.Entities
{
    public class Cell
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Group? Gallery { get; set; }
        public Group? Block { get; set; }
        public Group? Penitentiary { get; set; }
        public DVC? Dvc { get; set; } = new DVC();
        public List<User> Members { get; set; } = new List<User>();
        public List<Session>? Sessions { get; set; } = new List<Session>();
    }
}
