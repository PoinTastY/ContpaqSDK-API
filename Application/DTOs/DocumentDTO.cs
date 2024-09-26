using Domain.Entities;
using Domain.Entities.Estructuras;
using System.Runtime.InteropServices;

namespace Application.DTOs
{
    public class DocumentDTO
    {
        public int CIDDOCUMENTO { get; set; }

        public tDocumento STRUCTDOCUMENTO { get; set; }

        public tMovimiento STRUCTMOVIMIENTO { get; set; }

        public double CFOLIO { get; set; }

        public DateTime CFECHA { get; set; }

        public string CRAZONSOCIAL { get; set; } = string.Empty;

        public string CREFERENCIA { get; set; } = string.Empty;

        public double CTOTAL { get; set; }
        public string COBSERVACIONES { get; set; } = string.Empty;
        public string CTEXTOEXTRA1 { get; set; } = string.Empty;
        public string CTEXTOEXTRA2 { get; set; } = string.Empty;
        public string CTEXTOEXTRA3 { get; set; } = string.Empty;
        public int CIMPRESO { get; set; }

        public DocumentDTO(Document document)
        {
            CIDDOCUMENTO = document.CIDDOCUMENTO;
            CFOLIO = document.CFOLIO;
            CFECHA = document.CFECHA;
            CRAZONSOCIAL = document.CRAZONSOCIAL;
            CREFERENCIA = document.CREFERENCIA;
            CTOTAL = document.CTOTAL;
        }
        public DocumentDTO(tDocumento structDocumento,int idDocummento)
        {
            CIDDOCUMENTO = idDocummento;
            CFOLIO = structDocumento.aFolio;
            CREFERENCIA = structDocumento.aReferencia;
            CTOTAL = structDocumento.aImporte;
            CFECHA = DateTime.Parse(structDocumento.aFecha);
            STRUCTDOCUMENTO = structDocumento;
        }

        public DocumentDTO(tDocumento structDocumento, tMovimiento structMovimiento)
        {
            STRUCTDOCUMENTO = structDocumento;
            STRUCTMOVIMIENTO = structMovimiento;
        }

        public tDocumento ToSDKStruct()
        {
            return new tDocumento
            {
                aFolio = this.CFOLIO,
                aReferencia = this.CREFERENCIA
            };
        }
    }
}
