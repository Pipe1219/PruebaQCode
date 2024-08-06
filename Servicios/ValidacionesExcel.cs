using OfficeOpenXml;

namespace API_QCode.Servicios
{
    public class ValidacionesExcel
    {
        public IFormFile File { get; set; }

        public ValidacionesExcel(IFormFile file)
        {
            File = file;
        }

        public bool ValidarCabecerasExcel(IFormFile file, string[] ColumnasEsperadas)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return false;
                    }

                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
                        .Select(cell => cell.Text).ToArray();

                    return headerRow.SequenceEqual(ColumnasEsperadas);
                }
            }
        }
    }
}
