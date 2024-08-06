using API_QCode.Context;
using API_QCode.Models;
using Microsoft.Win32;
using System;

namespace API_QCode.Servicios
{
    public class ValidacionEmpleado
    {
        private readonly DbContext_QCode _context;

        public ValidacionEmpleado(DbContext_QCode context)
        {
            _context = context;
        }

        public bool ValidarEmpleado(string usuario, string contra)
        {
            var register_key = _context.Llave_Contrasenas.FirstOrDefault();

            var empleado = _context.Empleados.SingleOrDefault(u => u.Usuario == usuario);
            if (empleado != null && Encript_Desencript.EncriptarContrasena(contra, register_key.Llave) == empleado.Contra)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
