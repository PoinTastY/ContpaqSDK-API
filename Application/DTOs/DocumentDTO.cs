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
                aFolio = aFolio,
                aNumMoneda = aNumMoneda,
                aTipoCambio = aTipoCambio,
                aImporte = aImporte,
                aDescuentoDoc1 = aDescuentoDoc1,
                aDescuentoDoc2 = aDescuentoDoc2,
                aSistemaOrigen = aSistemaOrigen,
                aCodConcepto = aCodConcepto,
                aSerie = aSerie,
                aFecha = aFecha,
                aCodigoCteProv = aCodigoCteProv,
                aCodigoAgente = aCodigoAgente,
                aReferencia = aReferencia,
                aAfecta = aAfecta,
                aGasto1 = aGasto1,
                aGasto2 = aGasto2,
                aGasto3 = aGasto3
            };
        }
        public tMovimiento GetSDKMovementStruct()
        {
            return new tMovimiento
            {
                aConsecutivo = aConsecutivo,
                aUnidades = aUnidades,
                aPrecio = aPrecio,
                aCosto = aCosto,
                aCodProdSer = aCodProdSer,
                aCodAlmacen = aCodAlmacen,
                aReferencia = aReferenciaMov,
                aCodClasificacion = aCodClasificacion
            };
        }

        public DocumentDTO(tDocumento documento, int idDocumento)
        {
            CIDDOCUMENTO = idDocumento;
            aFolio = documento.aFolio;
            aNumMoneda = documento.aNumMoneda;
            aTipoCambio = documento.aTipoCambio;
            aImporte = documento.aImporte;
            aDescuentoDoc1 = documento.aDescuentoDoc1;
            aDescuentoDoc2 = documento.aDescuentoDoc2;
            aSistemaOrigen = documento.aSistemaOrigen;
            aCodConcepto = documento.aCodConcepto;
            aSerie = documento.aSerie;
            aFecha = documento.aFecha;
            aCodigoCteProv = documento.aCodigoCteProv;
            aCodigoAgente = documento.aCodigoAgente;
            aReferencia = documento.aReferencia;
            aAfecta = documento.aAfecta;
            aGasto1 = documento.aGasto1;
            aGasto2 = documento.aGasto2;
            aGasto3 = documento.aGasto3;
        }

        public DocumentDTO(DocumentSQL documento)
        {
            CIDDOCUMENTO = documento.CIDDOCUMENTO;
            CFECHA = documento.CFECHA;
            CRAZONSOCIAL = documento.CRAZONSOCIAL;
            CTOTAL = documento.CTOTAL;
            COBSERVACIONES = documento.COBSERVACIONES == null ? string.Empty : documento.COBSERVACIONES;
            CTEXTOEXTRA1 = documento.CTEXTOEXTRA1;
            CTEXTOEXTRA2 = documento.CTEXTOEXTRA2;
            CTEXTOEXTRA3 = documento.CTEXTOEXTRA3;
            CIMPRESO = documento.CIMPRESO;
            aReferencia = documento.CREFERENCIA;
            aSerie = documento.CSERIEDOCUMENTO;
            aFolio = documento.CFOLIO;
            aImporte = documento.CTOTAL;
        }
    }
}
