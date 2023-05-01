using System.Drawing;

namespace Weather.Web.Services.Interfaces
{
    public interface IExcelService
    {
        bool ReadExcel(IFormFile file,out string error, CancellationToken cancellationToken);
        string GeneratePath(string fileName);

    }
}