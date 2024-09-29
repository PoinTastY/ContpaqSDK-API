using System.Runtime.InteropServices;
using Domain.SDK_Comercial;

namespace Domain.Entities.Estructuras
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct tMovimiento
    {
        public int aConsecutivo;
        public double aUnidades;
        public double aPrecio;
        public double aCosto;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
        public string aCodProdSer;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
        public string aCodAlmacen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongReferencia)]
        public string aReferencia;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
        public string aCodClasificacion;
    }
}
