namespace ApplicationCore.Entities
{
    /// <summary>
    /// Id of model
    /// </summary>
    /// <typeparam name="T">id type</typeparam>
    public class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
