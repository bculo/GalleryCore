namespace ApplicationCore.Enumeration
{
    /// <summary>
    /// Enumeration (Enums are evil)
    /// </summary>
    public abstract class Enumeration
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;
    }
}
