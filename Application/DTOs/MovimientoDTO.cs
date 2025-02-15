﻿using Core.Domain.Entities.SDK.Estructuras;
using Core.Domain.Entities.SQL;
using Domain.SDK_Comercial;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace Core.Application.DTOs
{
    public class MovimientoDTO
    {
        public int CIDMOVIMIENTO { get; set; }
        public int CIDDOCUMENTO { get; set; }
        public int CIDPRODUCTO { get; set; }
        public int CIDALMACEN { get; set; }
        public double CUNIDADES { get; set; }
        public MovimientoDTO(string aCodProdSer, string aCodAlmacen, string aReferenciaMov, string aCodClasificacion, double unidades)
        {
            this.aCodAlmacen = aCodAlmacen;
            this.aCodProdSer = aCodProdSer;
            this.aReferenciaMov = aReferenciaMov;
            this.aCodClasificacion = aCodClasificacion;
            CUNIDADES = unidades;
        }

        public MovimientoDTO() { }

        public MovimientoDTO(MovimientoSQL movimiento)
        {
            CIDMOVIMIENTO = movimiento.CIDMOVIMIENTO;
            CIDDOCUMENTO = movimiento.CIDDOCUMENTO;
            CIDPRODUCTO = movimiento.CIDPRODUCTO;
            CIDALMACEN = movimiento.CIDALMACEN;
            CUNIDADES = movimiento.CUNIDADES;
        }

        public string aCodProdSer { get; set; } = string.Empty;
        public string aCodAlmacen { get; set; } = string.Empty;
        public string aReferenciaMov { get; set; } = string.Empty;
        public string aCodClasificacion { get; set; } = string.Empty;

        public tMovimiento GetSDKMovementStruct()
        {
            return new tMovimiento
            {
                aUnidades = CUNIDADES,
                aCodProdSer = aCodProdSer,
                aCodAlmacen = aCodAlmacen,
                aReferencia = aReferenciaMov,
                aCodClasificacion = aCodClasificacion
            };
        }
    }
}
