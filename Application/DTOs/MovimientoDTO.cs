using Domain.Entities;

namespace Application.DTOs
{
    public class MovimientoDTO
    {
        public int CIDMOVIMIENTO { get; set; }
        public int CIDDOCUMENTO { get; set; }
        public int CIDPRODUCTO { get; set; }
        public int CIDALMACEN { get; set; }
        public double CUNIDADES { get; set; }
        public DateTime CFECHA { get; set; }
        public int CAFECTADOINVENTARIO { get; set; }
        public string CTEXTOEXTRA1 { get; set; } = null!;
        public string CTEXTOEXTRA2 { get; set; } = null!;
        public string CTEXTOEXTRA3 { get; set; } = null!;
        public MovimientoDTO(MovimientoSQL movimiento)
        {
            CIDMOVIMIENTO = movimiento.CIDMOVIMIENTO;
            CIDDOCUMENTO = movimiento.CIDDOCUMENTO;
            CIDPRODUCTO = movimiento.CIDPRODUCTO;
            CIDALMACEN = movimiento.CIDALMACEN;
            CUNIDADES = movimiento.CUNIDADES;
            CFECHA = movimiento.CFECHA;
            CAFECTADOINVENTARIO = movimiento.CAFECTADOINVENTARIO;
            CTEXTOEXTRA1 = movimiento.CTEXTOEXTRA1;
            CTEXTOEXTRA2 = movimiento.CTEXTOEXTRA2;
            CTEXTOEXTRA3 = movimiento.CTEXTOEXTRA3;
        }
    }
}
