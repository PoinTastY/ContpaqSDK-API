using Core.Domain.Entities.SQL;

namespace Application.DTOs
{
    public class ClienteProveedorDto
    {
        public int CIDCLIENTEPROVEEDOR { get; set; }
        public string CCODIGOCLIENTE { get; set; } = null!;
        public string CRAZONSOCIAL { get; set; } = null!;
        public string CRFC { get; set; } = null!;
        public int CIDVALORCLASIFCLIENTE1 { get; set; }
        public int CIDVALORCLASIFCLIENTE2 { get; set; }
        public int CIDVALORCLASIFCLIENTE3 { get; set; }
        public int CIDVALORCLASIFCLIENTE4 { get; set; }
        public int CIDVALORCLASIFCLIENTE5 { get; set; }
        public int CIDVALORCLASIFCLIENTE6 { get; set; }
        public int CTIPOCLIENTE { get; set; }
        public int CESTATUS { get; set; }
        public double CLIMITECREDITOCLIENTE { get; set; }
        public int CIDVALORCLASIFPROVEEDOR1 { get; set; }
        public int CIDVALORCLASIFPROVEEDOR2 { get; set; }
        public int CIDVALORCLASIFPROVEEDOR3 { get; set; }
        public int CIDVALORCLASIFPROVEEDOR4 { get; set; }
        public int CIDVALORCLASIFPROVEEDOR5 { get; set; }
        public int CIDVALORCLASIFPROVEEDOR6 { get; set; }
        public string CTEXTOEXTRA1 { get; set; } = null!;
        public string CTEXTOEXTRA2 { get; set; } = null!;
        public string CTEXTOEXTRA3 { get; set; } = null!;
        public string CEMAIL1 { get; set; } = null!;
        public string CEMAIL2 { get; set; } = null!;
        public string CEMAIL3 { get; set; } = null!;
        public string CMETODOPAG { get; set; } = null!;
        public string CUSOCFDI { get; set; } = null!;
        public string CREGIMFISC { get; set; } = null!;
        public ClienteProveedorDto()
        {
        }
        public ClienteProveedorDto(ClienteProveedorSQL clienteSQL)
        {
            CIDCLIENTEPROVEEDOR = clienteSQL.CIDCLIENTEPROVEEDOR;
            CCODIGOCLIENTE = clienteSQL.CCODIGOCLIENTE;
            CRAZONSOCIAL = clienteSQL.CRAZONSOCIAL;
            CRFC = clienteSQL.CRFC;
            CIDVALORCLASIFCLIENTE1 = clienteSQL.CIDVALORCLASIFCLIENTE1;
            CIDVALORCLASIFCLIENTE2 = clienteSQL.CIDVALORCLASIFCLIENTE2;
            CIDVALORCLASIFCLIENTE3 = clienteSQL.CIDVALORCLASIFCLIENTE3;
            CIDVALORCLASIFCLIENTE4 = clienteSQL.CIDVALORCLASIFCLIENTE4;
            CIDVALORCLASIFCLIENTE5 = clienteSQL.CIDVALORCLASIFCLIENTE5;
            CIDVALORCLASIFCLIENTE6 = clienteSQL.CIDVALORCLASIFCLIENTE6;
            CTIPOCLIENTE = clienteSQL.CTIPOCLIENTE;
            CESTATUS = clienteSQL.CESTATUS;
            CLIMITECREDITOCLIENTE = clienteSQL.CLIMITECREDITOCLIENTE;
            CIDVALORCLASIFPROVEEDOR1 = clienteSQL.CIDVALORCLASIFPROVEEDOR1;
            CIDVALORCLASIFPROVEEDOR2 = clienteSQL.CIDVALORCLASIFPROVEEDOR2;
            CIDVALORCLASIFPROVEEDOR3 = clienteSQL.CIDVALORCLASIFPROVEEDOR3;
            CIDVALORCLASIFPROVEEDOR4 = clienteSQL.CIDVALORCLASIFPROVEEDOR4;
            CIDVALORCLASIFPROVEEDOR5 = clienteSQL.CIDVALORCLASIFPROVEEDOR5;
            CIDVALORCLASIFPROVEEDOR6 = clienteSQL.CIDVALORCLASIFPROVEEDOR6;
            CTEXTOEXTRA1 = clienteSQL.CTEXTOEXTRA1;
            CTEXTOEXTRA2 = clienteSQL.CTEXTOEXTRA2;
            CTEXTOEXTRA3 = clienteSQL.CTEXTOEXTRA3;
            CEMAIL1 = clienteSQL.CEMAIL1;
            CEMAIL2 = clienteSQL.CEMAIL2;
            CEMAIL3 = clienteSQL.CEMAIL3;
            CMETODOPAG = clienteSQL.CMETODOPAG;
            CUSOCFDI = clienteSQL.CUSOCFDI;
            CREGIMFISC = clienteSQL.CREGIMFISC;
        }
    }
}
