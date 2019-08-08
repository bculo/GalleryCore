using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IDomainModel<T> where T : class
    {
        T ToDomainModel();
    }
}
