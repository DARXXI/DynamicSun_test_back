using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Weather.Web.Controllers.Base;
using Weather.Repository.Repositories.Filters;
using Weather.Repository.Repositories;
using Weather.Repository.Repositories.Interfaces;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.IO;
using System.Web;
using Weather.Web.Services.Interfaces;

namespace Weather.Web.Controllers
{
    public class WeatherController : BaseController
    {
        private readonly IWeatherRepository<WeatherFilter> _weatherRepository;
        private readonly IExcelService _excelService;
        public WeatherController(IWeatherRepository<WeatherFilter> weatherRepository, IExcelService excelService) {
            _weatherRepository = weatherRepository;
            _excelService = excelService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Uploading()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetData(WeatherFilter weatherFilter)
        {
            var weathers = _weatherRepository.All(weatherFilter);

            return Json(weathers);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files, CancellationToken cancellationToken)
        {
            if (files.Count == 0)
            {
                return BadRequest();
            }
            foreach (var file in files)
            {               
                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.GetFullPath(_excelService.GeneratePath(file.FileName)), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);      
                    }
                    if (!_excelService.ReadExcel(file, cancellationToken))
                    {
                        FileInfo currentFile = new FileInfo(Path.GetFullPath(_excelService.GeneratePath(file.FileName)));
                        currentFile.Delete();
                        return Json("Wrong file type or format!");
                    };
                }  
            }
            return RedirectToAction("Index");
        }
    }
}
