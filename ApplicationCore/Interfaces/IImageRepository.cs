using ApplicationCore.Entities;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IImageRepository : IAsyncRepository<Image>
    {
        Task<Image> GetImageDetails(long imageId);
        Task<Image> GetImageWithComments(long imageId);
    }
}
