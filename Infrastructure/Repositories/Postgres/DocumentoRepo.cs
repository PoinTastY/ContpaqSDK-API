﻿using Domain.Entities.Interfaces;
using Domain.Interfaces.Repos.PostgreRepo;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Postgres
{
    public class DocumentoRepo : IDocumentoRepo
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Documento> _documentos;
        private readonly IMovimientoRepo _movimientoRepo;

        public DocumentoRepo(PostgresCPEContext dbContext, IMovimientoRepo movimientoRepo)
        {
            _movimientoRepo = movimientoRepo;
            _documentos = dbContext.documentos;
            _dbContext = dbContext;
        }

        public async Task<int> AddDocumentoAndMovimientoAsync(Documento documento, List<Movimiento> movimientos)
        {
            await _documentos.AddAsync(documento);

            await _dbContext.SaveChangesAsync();//save changes to get the id

            //now that we have the document id, add it to the movements
            movimientos.ForEach(m => m.IdPedido = documento.IdInterfaz);

            await _movimientoRepo.AddMovimientosAsync(movimientos);

            await _dbContext.SaveChangesAsync();

            return documento.IdInterfaz;

        }

        public async Task<List<Documento>> GetDocumentosPendientes()
        {
            return await _documentos.AsNoTracking().Where(d => d.Impreso == false && d.IdContpaqiSQL == 0).ToListAsync();
        }

        public async Task UpdateDocumentoAsync(Documento documento)
        {
            if (documento.IdInterfaz == 0)
            {
                throw new Exception("No se puede actualizar un documento sin id de interfaz");
            }
            if (documento.IdContpaqiSQL == 0)
            {
                throw new Exception("No se puede actualizar un documento sin id de contpaqi");
            }
            if (documento.Impreso == false)
            {
                throw new Exception("No se puede actualizar un documento sin marcar como impreso");
            }

            //update the document by its id
            var docToUpdate = await _documentos.FirstOrDefaultAsync(d => d.IdInterfaz == documento.IdInterfaz);
            if (docToUpdate == null)
            {
                throw new Exception("Documento no encontrado en la base de datos");
            }
            _dbContext.Entry(docToUpdate).CurrentValues.SetValues(documento);
            await _dbContext.SaveChangesAsync();
        }
    }
}
