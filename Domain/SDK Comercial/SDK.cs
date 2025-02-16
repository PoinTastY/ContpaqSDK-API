using Core.Domain.Entities.SDK.Estructuras;
using System.Runtime.InteropServices;
using System.Text;

namespace Domain.SDK_Comercial
{
    public class SDK
    {
        #region Funciones Generales

        /// <summary>
        /// Establece el directorio de las DEPENDENCIAS del dll, para que pueda encontrar las librerias necesarias
        /// </summary>
        /// <param name="pPtrDirActual">String</param>
        /// <returns>SDK error code</returns>
        [DllImport("KERNEL32")]
        public static extern int SetDllDirectory(string lpPathName);

        /// <summary>
        /// Establece el directorio, para que el dll encuentre los archivos extras que necesita
        /// </summary>
        /// <param name="lpPathName"></param>
        /// <returns></returns>
        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string lpPathName);

        /// <summary>
        /// Inicia sesion con el usuario y la contra especificadas
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="pass"></param>
        [DllImport("MGWServicios.dll", EntryPoint = "fInicioSesionSDK")]
        public static extern void fInicioSesionSDK(string usuario, string pass);

        /// <summary>
        /// Establece el nombre del sistema (p ej. Contpaqi Comercial)
        /// </summary>
        /// <param name="aNombrePAQ"></param>
        /// <returns></returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fSetNombrePAQ", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fSetNombrePAQ(String aNombrePAQ);

        /// <summary>
        /// Se supone que libera el uso del sdk, pero a veces se queda tirado
        /// </summary>
        [DllImport("MGWServicios.dll", EntryPoint = "fTerminaSDK")]
        public static extern void fTerminaSDK();

        /// <summary>
        /// Funcion para obtener el mensaje del codigo de error
        /// </summary>
        /// <param name="NumerError"></param>
        /// <param name="Mensaje"></param>
        /// <param name="Longitud"></param>
        [DllImport("MGWServicios.dll", EntryPoint = "fError")]
        public static extern void fError(int NumerError, StringBuilder Mensaje, int Longitud);


        #endregion

        #region Funciones de Empresa

        /// <summary>
        /// Abre la empresa con el directorio especificado
        /// </summary>
        /// <param name="Directorio">Directorio de la db de la empresa</param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fAbreEmpresa")]
        public static extern int fAbreEmpresa(string Directorio);

        /// <summary>
        /// Cierra la empresa para liberar el consumo del usuario
        /// </summary>
        [DllImport("MGWServicios.dll", EntryPoint = "fCierraEmpresa")]
        public static extern void fCierraEmpresa();



        /// <summary>
        /// Poisciona el puntero en la primer empresa encontrada, entrega coincidencia por referencia
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="aNombreEmpresa"></param>
        /// <param name="aDirEmpresa"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fPosPrimerEmpresa")]
        public static extern int fPosPrimerEmpresa(ref int idEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirEmpresa);

        /// <summary>
        /// Avanza el puntero al siguiente registro de empresa (si el retorno es 2, es que ya es el ultimo registro), por referencia
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="aNombreEmpresa"></param>
        /// <param name="aDirEmpresa"></param>
        /// <returns>SDK Error Code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fPosSiguienteEmpresa")]
        public static extern int fPosSiguienteEmpresa(ref int idEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirEmpresa);

        #endregion

        #region Manejo de Documentos
        /// <summary>
        /// Calcula el siguiente folio de la serie y concepto especificados
        /// </summary>
        /// <param name="aIdMovimiento"></param>
        /// <returns></returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fCalculaMovtoSerieCapa")]
        public static extern int fCalculaMovtoSerieCapa(int aIdMovimiento);

        /// <summary>
        /// Obtiene el siguiente folio del concepto y serie, TODO: probar si se pide uno al mismo tiempo, es el mismo o diferente
        /// </summary>
        /// <param name="codConcepto"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <returns>SDK Error Code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fSiguienteFolio")]
        public static extern int fSiguienteFolio(string codConcepto, StringBuilder serie, ref double folio);

        /// <summary>
        /// Da de alta un tDocumento, y regresa por referencia su id en la tabla de documentos, y el documento con los datos referenciados
        /// (id documento puede ser 0, porque si todo sale bien nos actualizara por referencia el id del final)
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <param name="atDocumento"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fAltaDocumento")]
        public static extern int fAltaDocumento(ref int idDocumento, ref tDocumento atDocumento);

        /// <summary>
        /// BAJO NIVEL, establece el valor de un campo (Columna, valor) de la tabla de sql en el registro que este posicionado el puntero actualmente
        /// </summary>
        /// <param name="aCampo"></param>
        /// <param name="aValor"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fSetDatoDocumento")]
        public static extern int fSetDatoDocumento(string aCampo, string aValor);

        /// <summary>
        /// Lee el valor de un campo 
        /// </summary>
        /// <param name="aCampo"></param>
        /// <param name="aValor"></param>
        /// <param name="aLen"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fLeeDatoDocumento")]
        public static extern int fLeeDatoDocumento(string aCampo, StringBuilder aValor, int aLongitud);

        /// <summary>
        /// Establece un filtro para la busqueda de documentos en la tabla con todos los valores especificados
        /// </summary>
        /// <param name="aFechaInicio"></param>
        /// <param name="aFechaFin"></param>
        /// <param name="aCodigoConcepto"></param>
        /// <param name="aCodigoCteProv"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fFiltroDocumento")]
        public static extern int fFiltroDocumento(StringBuilder aFechaInicio, StringBuilder aFechaFin, StringBuilder aCodigoConcepto, StringBuilder aCodigoCteProv);

        /// <summary>
        /// Deshace el filtro establecido con fFiltroDocumento
        /// </summary>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fCancelarFiltroDocumento")]
        public static extern int fCancelaFiltroDocumento();

        /// <summary>
        /// Alch no se, ya existe agregar documento xd
        /// </summary>
        /// <returns></returns>
        [DllImport("MGWServicios.dll")]
        public static extern int fInsertarDocumento();

        /// <summary>
        /// Libera los recursos, para editar el documento en el que este actualmente el documento.
        /// </summary>
        /// <returns></returns>
        [DllImport("MGWServicios.dll")]
        public static extern int fEditarDocumento();

        /// <summary>
        /// Guarda las modificaciones, si es que previamente se ejecuto editar documento.
        /// </summary>
        /// <returns></returns>
        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaDocumento();

        /// <summary>
        /// Aborta la previa operacion de editar documento
        /// </summary>
        /// <returns></returns>
        [DllImport("MGWServicios.dll")]
        public static extern int fCancelarModificacionDocumento();

        /// <summary>
        /// Posiciona el puntero en el registro de la tabla con el Id Proporcionado
        /// OJO id de la tabla, no folio
        /// </summary>
        /// <param name="aIdDocumento"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fBuscarIdDocumento")]
        public static extern int fBuscarIdDocumento(int aIdDocumento);

        /// <summary>
        /// Busca un documento por su serie, concepto y folio y se posiciona en el puntero si hay coincidencia
        /// </summary>
        /// <param name="aCodConcepto"></param>
        /// <param name="aSerie"></param>
        /// <param name="aFolio"></param>
        /// <returns>SDKError code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fBuscarDocumento")]
        public static extern int fBuscarDocumento(string aCodConcepto, string aSerie, string aFolio);

        /// <summary>
        /// Posiciona el puntero en el ultimo registro de la tabla de documentos
        /// </summary>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll")]
        public static extern int fPosUltimoDocumento();

        /// <summary>
        /// Establece el estado de impreso al documento con el puntero actual (primero buscar documento)
        /// </summary>
        /// <param name="aImpreso"></param>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fDocumentoImpreso")]
        public static extern int fDocumentoImpreso(bool aImpreso);

        /// <summary>
        /// Cancela el documento con el puntero actual (primero buscar documento)
        /// </summary>
        /// <returns>SDK error code</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fCancelaDocumento")]
        public static extern int fCancelaDocumento();

        #endregion

        #region Manejo de Movimientos

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoSeriesCapas_Param")]
        public static extern int fAltaMovimientoSeriesCapas_Param(string aIdMovimiento, string aUnidades, string aTipoCambio, string aSeries, string aPedimento, string aAgencia, string aFechaPedimento, string aNumeroLote, string aFechaFabricacion, string aFechaCaducidad);


        /// <summary>
        /// Da de alta un movimiento, retornando por referencia el id del movimiento, que es la pkey de su tabla en sql, y el struct de movimiento con el folio completo creo
        /// </summary>
        /// <param name="aIdDocumento"></param>
        /// <param name="aIdMovimiento"></param>
        /// <param name="astMovimiento"></param>
        /// <returns>SDK ERRR CODE IF BAD REQUEST</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimiento")]
        public static extern int fAltaMovimiento(int aIdDocumento, ref int aIdMovimiento, ref tMovimiento astMovimiento);

        /// <summary>
        /// Posiciona el puntero en el primer movimiento de la tabla de movimientos que coincida con el id
        /// </summary>
        /// <param name="aIdMovimiento"></param>
        /// <returns>SDK ERRR CODE IF BAD REQUEST</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fBuscarIdMovimiento")]
        public static extern int fBuscarIdMovimiento(int aIdMovimiento);

        /// <summary>
        /// Habilita la edicion del movimiento en el que este posicionado el puntero
        /// </summary>
        /// <returns>SDK ERRR CODE IF BAD REQUEST</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fEditarMovimiento")]
        public static extern int fEditarMovimiento();

        /// <summary>
        /// Establece el valor de un campo de la tabla de movimientos en el registro que este posicionado el puntero
        /// </summary>
        /// <param name="aCampo">La columna objetivo</param>
        /// <param name="aValor">el valor a asignar</param>
        /// <returns>SDK ERRR CODE IF BAD REQUEST</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fSetDatoMovimiento")]
        public static extern int fSetDatoMovimiento(string aCampo, string aValor);

        /// <summary>
        /// Guarda los cambios realizados en el movimiento en el que este posicionado el puntero
        /// </summary>
        /// <returns>SDK ERRR CODE IF BAD REQUEST</returns>
        [DllImport("MGWServicios.dll", EntryPoint = "fGuardaMovimiento")]
        public static extern int fGuardaMovimiento();

        #endregion

        #region Manejo de Clientes

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaIdCteProv(int aIdCteProv);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaCteProv(string aCodCteProv);

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaCteProv(ref int aIdCteProv, tCteProv astCteProv);

        #endregion

        #region Manejo de Productos

        [DllImport("MGWServicios.dll", EntryPoint = "fActualizaProducto")]
        public static extern int fActualizaProducto(string aCodigoProducto, ref tProducto astProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaProducto")]
        public static extern int fAltaProducto(ref int aIdProducto, ref tProducto astProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fBorraProducto")]
        public static extern int fBorraProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaIdProducto")]
        public static extern int fBuscaIdProducto(int aIdProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaProducto")]
        public static extern int fBuscaProducto(string aCodProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fEditaProducto")]
        public static extern int fEditaProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fEliminarProducto")]
        public static extern int fEliminarProducto(string aCodigoProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fGuardaProducto")]
        public static extern int fGuardaProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fInsertaProducto")]
        public static extern int fInsertaProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fLeeDatoProducto")]
        public static extern int fLeeDatoProducto(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWServicios.dll", EntryPoint = "fLlenaRegistroProducto")]
        public static extern int fLlenaRegistroProducto(ref tProducto astProducto, int aEsAlta);

        [DllImport("MGWServicios.dll", EntryPoint = "fPosAnteriorProducto")]
        public static extern int fPosAnteriorProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosBOFProducto")]
        public static extern int fPosBOFProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosEOFProducto")]
        public static extern int fPosEOFProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosPrimerProducto")]
        public static extern int fPosPrimerProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosSiguienteProducto")]
        public static extern int fPosSiguienteProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosUltimoProducto")]
        public static extern int fPosUltimoProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fSetDatoProducto")]
        public static extern int fSetDatoProducto(string aCampo, string aValor);

        [DllImport("MGWServicios.dll", EntryPoint = "fSetDescripcionProducto")]
        public static extern int fSetDescripcionProducto(string aCampo, string aValor);

        #endregion

        /// <summary>
        /// Recibe un codigo de error generado por funciones del sdk
        /// </summary>
        /// <param name="iError">Codigo de error generado por metodo del sdk</param>
        /// <returns>String del mensaje</returns>
        public static string rError(int iError)
        {
            StringBuilder msj = new StringBuilder(512);
            if (iError != 0)
            {
                fError(iError, msj, 512);
            }
            return msj.ToString();
        }

    }
}