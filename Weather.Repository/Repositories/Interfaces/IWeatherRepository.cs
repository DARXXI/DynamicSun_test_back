using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Entities;
using Weather.Domain.Models;
using Weather.Repository.Repositories.Filters;

namespace Weather.Repository.Repositories.Interfaces
{
    public interface IWeatherRepository<TFilter> where TFilter : BaseFilter
    {
        BaseModel<Weather.Domain.Entities.Weather> All(TFilter filter);
        void Add(Weather.Domain.Entities.Weather weather, CancellationToken cancellationToken);
        void ClearDataByYear(int year);
        void Update();
    }
}
