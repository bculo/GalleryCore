namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image tags
    /// </summary>
    public class Tag : BaseEntity<long>
    {
        public string Description { get; set; }

        public long ImageId { get; set; }
        public Image Image { get; set; }

        public override string ToString() => Description;
    }
}
