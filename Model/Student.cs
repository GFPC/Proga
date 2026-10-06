namespace Model
{
    public class Student : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
    }
}
