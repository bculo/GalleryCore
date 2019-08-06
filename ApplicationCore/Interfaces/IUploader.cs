using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IUploader
    {
        Uploader ToDomainModel();
        string UserId { get; }
        string UserMail { get; }
    }
}
