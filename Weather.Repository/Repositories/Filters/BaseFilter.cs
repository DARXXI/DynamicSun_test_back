using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Weather.Repository.Repositories.Filters
{
    public class BaseFilter
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int LastRow { get; set; }
        public string SortColumn { get; set; }
        public Weather.Domain.Enums.SortOrder SortOrder { get; set; }
        public int Take => 1 + EndRow - StartRow;
    }
}