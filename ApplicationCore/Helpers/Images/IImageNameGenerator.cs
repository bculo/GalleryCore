namespace ApplicationCore.Helpers.Images
{
    public interface IImageNameGenerator
    {
        string GetUniqueImageName(string fileNameWithExtension);
    }
}
