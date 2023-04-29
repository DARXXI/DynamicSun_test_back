namespace Weather.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}