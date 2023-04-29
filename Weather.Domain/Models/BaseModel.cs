using Weather.Domain.Entities;

namespace Weather.Domain.Models
{
    public class BaseModel<T>
    {
        public int LastRowIndex { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}