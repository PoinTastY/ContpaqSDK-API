using Core.Domain.Entities.SDK.Estructuras;
using Domain.SDK_Comercial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities.DTOs
{
    /// <summary>
    /// DTO de Documento, se utiliza para mapear los datos de un documento, compatibles con Postgres
    /// </summary>
    public class DocumentoDto
    {
        public int IdPostgres { get; set; } = 0;
        public int IdContpaqiSQL { get; set; } = 0;
        public double Folio { get; set; } = 0;
        public int NumMoneda { get; set; } = 0;
        public double TipoCambio { get; set; } = 0;
        public double Importe { get; set; } = 0;
        public double DescuentoDoc1 { get; set; } = 0;
        public double DescuentoDoc2 { get; set; } = 0;
        public int SistemaOrigen { get; set; } = 0;
        public string CodConcepto { get; set; } = string.Empty; 
        public string Serie { get; set; } = string.Empty; 
        /// <summary>
        /// REQUIRED Format: "MM/dd/yyyy"
        /// </summary>
        public string Fecha { get; set; } = string.Empty;
        public string CodigoCteProv { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string CodigoAgente { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
        public int Afecta { get; set; } = 0;
        public double Gasto1 { get; set; } = 0;
        public double Gasto2 { get; set; } = 0;
        public double Gasto3 { get; set; } = 0;
        public string Observaciones { get; set; } = string.Empty;
        public string TextoExtra1 { get; set; } = string.Empty;
        public string TextoExtra2 { get; set; } = string.Empty;
        public string TextoExtra3 { get; set; } = string.Empty;
        public bool Surtido { get; set; } = false;
        public bool Impreso { get; set; } = false;
        public DocumentoDto() { }

        public tDocumento ToSDKDocumento()
        {
            return new tDocumento() {
                aFolio = Folio,
                aNumMoneda = NumMoneda,
                aTipoCambio = TipoCambio,
                aImporte = Importe,
                aDescuentoDoc1 = DescuentoDoc1,
                aDescuentoDoc2 = DescuentoDoc2,
                aSistemaOrigen = SistemaOrigen,
                aCodConcepto = CodConcepto,
                aSerie = Serie,
                aFecha = Fecha,
                aCodigoCteProv = CodigoCteProv,
                aCodigoAgente = CodigoAgente,
                aReferencia = Referencia,
                aAfecta = Afecta,
                aGasto1 = Gasto1,
                aGasto2 = Gasto2,
                aGasto3 = Gasto3
            };
        }
    }
}
