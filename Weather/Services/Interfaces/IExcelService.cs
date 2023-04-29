using System.Drawing;

namespace Weather.Web.Services.Interfaces
{
    public interface IExcelService
    {
        bool ReadExcel(IFormFile file, CancellationToken cancellationToken);
        string GeneratePath(string fileName);

    }
}