namespace Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public int PlaceId { get; set; }
        public int Level { get; set; }
    }
}
