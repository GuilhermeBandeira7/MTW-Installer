namespace EntityMtwServer.Entities
{
    public class VehicleModel
    {
        public long Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SubModel { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int Year { get; set; } = int.MinValue;
        public string Color { get; set; } = string.Empty;
    }
}
