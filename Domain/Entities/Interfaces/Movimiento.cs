using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Interfaces
{
    public class Movimiento
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string CodigoAlmacen { get; set; } = string.Empty;
        public string CodigoClasificacion { get; set; } = string.Empty;
        public double Unidades { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public double Surtidas { get; set; }
        public bool EsGranel { get; set; }
    }
}
