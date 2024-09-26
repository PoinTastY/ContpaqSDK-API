namespace Domain.SDK_Comercial
{
    public class SDKSettings
    {
        public string NombrePAQ { get; set; }
        public string DirEmpresa { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Binarios { get; set; }
        public string SerieDocumento { get; set; }
        public string ConceptoDocumento { get; set; }
        public string SerialPort { get; set; }
        public string Payee { get; set; }
        public string SQLConnectionString { get; set; }
        public string CMETODOPAGO { get; set; }

        public SDKSettings() { }
    }
}
