using API_QCode.Context;
using API_QCode.Models;
using API_QCode.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace API_QCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Excel_ImportController : ControllerBase
    {
        private readonly DbContext_QCode _context;

        public Excel_ImportController(DbContext_QCode context)
        {
            _context = context;
        }

        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string[] columnasFormato = { 
                "MARCA", 
                "MODELO", 
                "AÑO DE FABRICACION",
                "MATRICULA",
                "PROBLEMA",
                "DESCRIPCION DEL PROBLEMA",
                "VALOR"
            };

            ValidacionesExcel validador = new ValidacionesExcel(file);

            if (!validador.ValidarCabecerasExcel(file, columnasFormato))
            {
                string columnasFormatoString = string.Join(", ", columnasFormato);

                return BadRequest($"Error en cabeceras del excel. Orden obligatorio: {columnasFormatoString}.");                
            }

            var vehiculosEnExcel = new List<Vehiculo>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var vehiculo = new Vehiculo
                        {
                            Marca = worksheet.Cells[row, 1].Text,
                            Modelo = worksheet.Cells[row, 2].Text,
                            AnoFabricacion = int.Parse(worksheet.Cells[row, 3].Text),
                            Matricula = worksheet.Cells[row, 4].Text,
                            Problema = worksheet.Cells[row, 5].Text,
                            DescripcionProblema = worksheet.Cells[row, 6].Text,
                            Valor = double.Parse(worksheet.Cells[row, 7].Text),
                            FechaRegistro = DateTime.Now,
                            Estado = 0
                        };

                        vehiculosEnExcel.Add(vehiculo);
                    }
                }
            }

            var vehiculosEnBD = await _context.Vehiculos.ToListAsync();

            foreach (var vehiculo in vehiculosEnExcel)
            {
                var vehiculoEnBD = vehiculosEnBD.FirstOrDefault(v => v.Matricula == vehiculo.Matricula);

                if (vehiculoEnBD == null)
                {
                    if (DateTime.Now.Day % 2 == 0)
                        vehiculo.Valor = (vehiculo.Valor + (0.05 * vehiculo.Valor));

                    if (vehiculo.AnoFabricacion <= 1997)
                        vehiculo.Valor = (vehiculo.Valor + (0.20 * vehiculo.Valor));

                    _context.Vehiculos.Add(vehiculo);
                }
                else
                {
                    vehiculoEnBD.Marca = vehiculo.Marca;
                    vehiculoEnBD.Modelo = vehiculo.Modelo;
                    vehiculoEnBD.AnoFabricacion = vehiculo.AnoFabricacion;
                    vehiculoEnBD.Problema = vehiculo.Problema;
                    vehiculoEnBD.DescripcionProblema = vehiculo.DescripcionProblema;

                    vehiculoEnBD.Valor = vehiculo.Valor;

                    if (DateTime.Now.Day % 2 == 0)
                        vehiculoEnBD.Valor = (vehiculo.Valor + (0.05 * vehiculo.Valor));

                    if (vehiculo.AnoFabricacion <= 1997)
                        vehiculoEnBD.Valor = (vehiculo.Valor + (0.20 * vehiculo.Valor));

                    vehiculoEnBD.FechaRegistro = DateTime.Now;
                    vehiculoEnBD.Estado = 1;

                    _context.Vehiculos.Update(vehiculoEnBD);
                }
            }

            foreach (var vehiculoEnBD in vehiculosEnBD)
            {
                if (!vehiculosEnExcel.Any(v => v.Matricula == vehiculoEnBD.Matricula))
                {
                    vehiculoEnBD.Estado = 0;
                    vehiculoEnBD.FechaRegistro = DateTime.Now;
                    _context.Vehiculos.Update(vehiculoEnBD);
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Archivo subido. (" + vehiculosEnExcel.Count + ") registros procesados.");
        }
    }
}
