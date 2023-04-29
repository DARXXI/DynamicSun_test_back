using Microsoft.Office.Interop.Access.Dao;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Entities;
using Weather.Repository.Repositories.Filters;
using Weather.Repository.Repositories.Interfaces;
using Microsoft.Extensions.Configuration.UserSecrets;
using Weather.Domain.Models;

namespace Weather.Repository.Repositories
{
    public class WeatherRepository: BaseRepository, IWeatherRepository<WeatherFilter>
    {
        public WeatherRepository(DataBaseContext context) : base(context)
        {

        }

        public BaseModel<Weather.Domain.Entities.Weather> All(WeatherFilter weatherFilter)
        {
            var propertyGetter = DynamicExpressions.DynamicExpressions.GetPropertyGetter<Weather.Domain.Entities.Weather>(weatherFilter.SortColumn);

            var query = Context.Weather.AsQueryable();
            var lengthOfGrid = Context.Weather.Count();

            var temp = query;
            if (weatherFilter.Year != null && weatherFilter.Month != null)
            {
                query = query.Where(t => t.Date.Year == weatherFilter.Year && t.Date.Month == weatherFilter.Month);
                lengthOfGrid = query.Count();
            }

            query = weatherFilter.SortOrder == Weather.Domain.Enums.SortOrder.Asc
                ? query.OrderBy(propertyGetter)
                : query.OrderByDescending(propertyGetter);
            if (weatherFilter.SortColumn == "date")
            {
                query = weatherFilter.SortOrder == Weather.Domain.Enums.SortOrder.Asc
                ? query.OrderBy(propertyGetter).ThenBy(t=>t.Time)
                : query.OrderByDescending(propertyGetter).ThenByDescending(t => t.Time);
            }

            query = query.Skip(weatherFilter.StartRow).Take(weatherFilter.Take);

            var queryWeathers = query.ToArray();

            var weathersModel = new BaseModel<Weather.Domain.Entities.Weather>() { Data = queryWeathers, LastRowIndex = lengthOfGrid };
            return weathersModel;
        }

        public void ClearDataByYear(int year)
        {
            var weatherToDelete = Context.Weather.Where(t=>t.Date.Year == year);

            if (weatherToDelete != null)
            {
                Context.Weather.RemoveRange(weatherToDelete);
            }    
        }

        public void Add(Weather.Domain.Entities.Weather weather, CancellationToken cancellationToken)
        {
            Context.Weather.Add(weather);
        }

        public void Update()
        {
            Context.SaveChanges();
        }
    }
}
