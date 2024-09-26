using Domain.Entities;
using Domain.Entities.Estructuras;

namespace Application.DTOs
{
    public class DocumentDTO
    {
        //weas del struct documento:
        public double aFolio { get; set; }
        public int aNumMoneda { get; set; }
        public double aTipoCambio { get; set; }
        public double aImporte { get; set; }
        public double aDescuentoDoc1 { get; set; }
        public double aDescuentoDoc2 { get; set; }
        public int aSistemaOrigen { get; set; }
        public string aCodConcepto { get; set; } = string.Empty;
        public string aSerie { get; set; } = string.Empty;
        public string aFecha { get; set; } = string.Empty;
        public string aCodigoCteProv { get; set; } = string.Empty;
        public string aCodigoAgente { get; set; } = string.Empty;
        public string aReferencia { get; set; } = string.Empty;
        public int aAfecta { get; set; }
        public double aGasto1 { get; set; }
        public double aGasto2 { get; set; }
        public double aGasto3 { get; set; }

        //weas del struct movimiento:
        public int aConsecutivo { get; set; }
        public double aUnidades { get; set; }
        public double aPrecio { get; set; }
        public double aCosto { get; set; }
        public string aCodProdSer { get; set; } = string.Empty;
        public string aCodAlmacen { get; set; } = string.Empty;
        public string aReferenciaMov { get; set; } = string.Empty;
        public string aCodClasificacion { get; set; } = string.Empty;

        public int CIDDOCUMENTO { get; set; }
        public DateTime CFECHA { get; set; }
        public string CRAZONSOCIAL { get; set; } = string.Empty;
        public double CTOTAL { get; set; }
        public string COBSERVACIONES { get; set; } = string.Empty;
        public string CTEXTOEXTRA1 { get; set; } = string.Empty;
        public string CTEXTOEXTRA2 { get; set; } = string.Empty;
        public string CTEXTOEXTRA3 { get; set; } = string.Empty;
        public int CIMPRESO { get; set; }

        public DocumentDTO() { }

        public tDocumento GetSDKDocumentStruct()
        {
            return new tDocumento
            {
                aFolio = this.aFolio,
                aNumMoneda = this.aNumMoneda,
                aTipoCambio = this.aTipoCambio,
                aImporte = this.aImporte,
                aDescuentoDoc1 = this.aDescuentoDoc1,
                aDescuentoDoc2 = this.aDescuentoDoc2,
                aSistemaOrigen = this.aSistemaOrigen,
                aCodConcepto = this.aCodConcepto,
                aSerie = this.aSerie,
                aFecha = this.aFecha,
                aCodigoCteProv = this.aCodigoCteProv,
                aCodigoAgente = this.aCodigoAgente,
                aReferencia = this.aReferencia,
                aAfecta = this.aAfecta,
                aGasto1 = this.aGasto1,
                aGasto2 = this.aGasto2,
                aGasto3 = this.aGasto3
            };
        }
        public tMovimiento GetSDKMovementStruct()
        {
            return new tMovimiento
            {
                aConsecutivo = this.aConsecutivo,
                aUnidades = this.aUnidades,
                aPrecio = this.aPrecio,
                aCosto = this.aCosto,
                aCodProdSer = this.aCodProdSer,
                aCodAlmacen = this.aCodAlmacen,
                aReferencia = this.aReferenciaMov,
                aCodClasificacion = this.aCodClasificacion
            };
        }

        public DocumentDTO(tDocumento documento, int idDocumento)
        {
            this.CIDDOCUMENTO = idDocumento;
            this.aFolio = documento.aFolio;
            this.aNumMoneda = documento.aNumMoneda;
            this.aTipoCambio = documento.aTipoCambio;
            this.aImporte = documento.aImporte;
            this.aDescuentoDoc1 = documento.aDescuentoDoc1;
            this.aDescuentoDoc2 = documento.aDescuentoDoc2;
            this.aSistemaOrigen = documento.aSistemaOrigen;
            this.aCodConcepto = documento.aCodConcepto;
            this.aSerie = documento.aSerie;
            this.aFecha = documento.aFecha;
            this.aCodigoCteProv = documento.aCodigoCteProv;
            this.aCodigoAgente = documento.aCodigoAgente;
            this.aReferencia = documento.aReferencia;
            this.aAfecta = documento.aAfecta;
            this.aGasto1 = documento.aGasto1;
            this.aGasto2 = documento.aGasto2;
            this.aGasto3 = documento.aGasto3;
        }
    }
}
