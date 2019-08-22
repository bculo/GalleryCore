namespace ApplicationCore.Helpers.Images
{
    public class ImageUpdate<T>
    {
        public T Instance { get; set; }
        public bool ImageChanged { get; set; }
        public string OldImageUrl { get; set; }
    }
}
