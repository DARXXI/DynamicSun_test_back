using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Weather.Domain.Enums;
using Weather.Repository.Repositories.Filters;
using Weather.Repository.Repositories.Interfaces;
using Weather.Web.Extensions;
using Weather.Web.Services.Interfaces;

namespace Weather.Web.Services
{
    public class ExcelService: IExcelService
    {
        private IWebHostEnvironment _hostingEnvironment;
        private IWeatherRepository<WeatherFilter> _weatherRepository;
        public string fullPath { get; set; }
        public ExcelService(IWebHostEnvironment hostingEnvironment, IWeatherRepository<WeatherFilter> weatherRepository) 
        {
            _weatherRepository = weatherRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public string GeneratePath(string fileName)
        {
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            this.fullPath = Path.Combine(newPath, fileName);

            return fullPath;
        }

        public bool ReadExcel(IFormFile file,out string error,  CancellationToken cancellationToken)
        {
            try
            {
                if (fullPath.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                        string sFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension != ".xls" && sFileExtension != ".xlsx")
                        {
                            error = "Неправильное расширение файла!";
                            return false;
                        }
                        if (sFileName.Length > 4)
                        {
                            string yearStr = sFileName.Substring(sFileName.Length - 4);
                            if (yearStr.Length < 4)
                            {
                                error = "Неправильное название файла!";
                                return false;
                            }
                            try
                            {
                                int year = Convert.ToInt32(yearStr);
                                //delete where sqlyear = year
                                _weatherRepository.ClearDataByYear(year);
                            }
                            catch
                            {
                                error = "Неправильный формат файла!";
                                return false;
                            }
                        }
                        List<ISheet> sheets = new List<ISheet>();
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats
                            for (int i = 0; i< hssfwb.NumberOfSheets; i++)
                                sheets.Add(hssfwb.GetSheetAt(i));
                        }
                        else //xlsx
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                            for (int i = 0; i < hssfwb.NumberOfSheets; i++)
                                sheets.Add(hssfwb.GetSheetAt(i));
                        }
                        foreach(var sheet in sheets)
                        { 
                            IRow headerRow = sheet.GetRow(2); //Get Header Row
                            if (headerRow == null)
                            {
                                error = "Неправильный формат файла!";
                                return false;
                            }
                            int cellCount = headerRow.LastCellNum;
                            if (cellCount != 12)
                            {
                                error = "Неправильный формат файла!";
                                return false;
                            }
                            for (int j = 0; j < cellCount; j++)
                            {
                                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            }
                            for (int i = (sheet.FirstRowNum + 4); i <= sheet.LastRowNum; i++) //Read Excel File TABLE 
                            {
                               //New Weather
                                Weather.Domain.Entities.Weather weather = new();
                                var allfileds = weather.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic
                                    | BindingFlags.Public | BindingFlags.Static).ToArray(); 

                                IRow row = sheet.GetRow(i);
                                if (row == null) continue;

                                bool SkipAdd = false;
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    string cellValue = row.GetCell(j) == null ? string.Empty : row.GetCell(j).ToString();

                                    if (allfileds[j].FieldType == typeof(DateOnly))
                                    {
                                        try
                                        {
                                            DateOnly.Parse(cellValue);
                                        }
                                        catch 
                                        {
                                            //TODO logging
                                            error = "Неправильный формат данных!";
                                            SkipAdd = true;
                                            break;
                                        }
                                        allfileds[j].SetValue(weather, DateOnly.Parse(cellValue));
                                        continue;
                                    }

                                    if (allfileds[j].FieldType == typeof(Double))
                                    {
                                        try 
                                        {
                                            Convert.ToDouble(cellValue);
                                        
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        allfileds[j].SetValue(weather, Convert.ToDouble(cellValue));
                                        continue;
                                    }

                                    if (allfileds[j].FieldType == typeof(Nullable<Double>))
                                    {
                                        try
                                        {
                                            cellValue.ToNullable<double>();         
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        allfileds[j].SetValue(weather, cellValue.ToNullable<double>());
                                        continue;
                                    }

                                    if (allfileds[j].FieldType == typeof(Nullable<Int32>))
                                    {
                                        try
                                        {
                                            cellValue.ToNullable<Int32>();    
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        allfileds[j].SetValue(weather, cellValue.ToNullable<Int32>());
                                        continue;
                                    }

                                    if (allfileds[j].FieldType == typeof(TimeOnly))
                                    {
                                        try
                                        {
                                            TimeOnly.Parse(cellValue);   
                                        }
                                        catch
                                        {
                                            error = "Неправильный формат данных!";
                                            SkipAdd = true;
                                            break;
                                        }
                                        allfileds[j].SetValue(weather, TimeOnly.Parse(cellValue));
                                        continue;
                                    }

                                    else 
                                    {
                                        try 
                                        {
                                            cellValue.ToString();                                   
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        allfileds[j].SetValue(weather, cellValue.ToString());
                                        continue;
                                    }
                                
                                }
                                if (SkipAdd)
                                {
                                    continue;
                                }
                                _weatherRepository.Add(weather, cancellationToken);
                            }
                            _weatherRepository.Update();
                        }
                    }
                    error = "";
                    return true;
                }
                error = "Ошибка чтения файла!";
                return false;
            }
            catch
            {
                error = "Ошибка чтения файла!";
                return false;
            }
        }
    }
}
