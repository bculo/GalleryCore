using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// User interface
    /// </summary>
    public interface IUploader : IDomainModel<Uploader>
    {
        

        string UserId { get; }
        string UserName { get; }
        string UserMail { get; }
    }
}
