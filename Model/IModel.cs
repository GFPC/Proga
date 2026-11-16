namespace Model
{
    public interface IModel
    {
        event Action DataChanged;
        IReadOnlyList<Student> GetAll();
        void Add(string name, string speciality, string group);
        void Delete(int id);
    }
}
