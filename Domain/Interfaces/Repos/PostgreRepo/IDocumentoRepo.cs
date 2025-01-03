﻿using Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repos.PostgreRepo
{
    public interface IDocumentoRepo
    {
        /// <summary>
        /// Adds a document to the database with his movements
        /// </summary>
        /// <param name="pedido"></param>
        /// <param name="movimientos"></param>
        /// <returns> id of the created document</returns>
        Task<int> AddDocumentoAndMovimientoAsync(Documento pedido, List<Movimiento> movimientos);

        /// <summary>
        /// Obtiene los documentos pendientes de ser pesados
        /// </summary>
        /// <returns>Lista de documentos esperando pa chambear</returns>
        Task<List<Documento>>GetDocumentosPendientes();

        /// <summary>
        /// Actualiza el documento en la base de datos, se usa para marcarlo como surtido y dar id de contpaqi
        /// </summary>
        /// <param name="documento"></param>
        /// <returns>throw if ereror</returns>
        Task UpdateDocumentoAsync(Documento documento);
    }
}
