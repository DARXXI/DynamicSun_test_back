namespace Weather.Repository.Repositories
{
    public class BaseRepository
    {
        public readonly DataBaseContext Context;
        public BaseRepository(DataBaseContext context)
        {
            this.Context = context;
        }
        public DateTime CurrentDate = DateTime.Now.ToLocalTime();
    }
}