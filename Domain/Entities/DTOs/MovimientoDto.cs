using Core.Domain.Entities.SDK.Estructuras;

namespace Core.Domain.Entities.DTOs
{
    public class MovimientoDto
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        public string CodigoProducto { get; set; } = string.Empty;
        public string CodigoAlmacen { get; set; } = string.Empty;
        public string CodigoClasificacion { get; set; } = string.Empty;
        public double Unidades { get; set; }
        public double Precio { get; set; }
        public double Costo { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public double Surtidas { get; set; }
        public bool EsGranel { get; set; }
        public MovimientoDto() { }
        public tMovimiento ToSDKMovimiento()
        {
            return new tMovimiento
            {
                aConsecutivo = Id,
                aUnidades = Unidades,
                aPrecio = Precio,
                aCosto = Costo,
                aCodProdSer = CodigoProducto,
                aCodAlmacen = CodigoAlmacen,
                aReferencia = Referencia,
                aCodClasificacion = CodigoClasificacion
            };
        }
    }
}
