namespace ApplicationCore.Helpers.Service
{
    public class ServiceResult<T> : ServiceNoResult
    {
        public T Result { get; set; }
    }
}
