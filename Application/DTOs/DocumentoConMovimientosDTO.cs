using Domain.Entities.Estructuras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class DocumentoConMovimientosDTO
    {
        public tDocumento Documento { get; set; }
        public List<tMovimiento> Movimientos { get; set; } = new List<tMovimiento>();
    }
}
