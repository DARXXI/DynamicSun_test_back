namespace Weather.Repository.Repositories.Filters
{
    public class WeatherFilter : BaseFilter
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
    }
}
