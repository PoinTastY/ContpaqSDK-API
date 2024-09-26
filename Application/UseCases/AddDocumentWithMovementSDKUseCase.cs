using Application.DTOs;
using Domain.Entities.Estructuras;
using Domain.Interfaces;

namespace Application.UseCases
{
    public class AddDocumentWithMovementSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        public AddDocumentWithMovementSDKUseCase(ISDKRepo sdkRepo)
        {
            _sdkRepo = sdkRepo;
        }

        public async Task<int> Execute(DocumentDTO documento, tMovimiento movimiento)
        {
            var idDocumento = await _sdkRepo.AddDocumentWithMovement(documento.STRUCTDOCUMENTO, movimiento);

            if(NeedsExtraFields(documento))
            {
                await AddExtraFields(documento, idDocumento);
            }

            return idDocumento;
        }

        private bool NeedsExtraFields(DocumentDTO documento)
        {
            var ListAtributes = new List<string>()
            {
                documento.COBSERVACIONES,
                documento.CTEXTOEXTRA1,
                documento.CTEXTOEXTRA2,
                documento.CTEXTOEXTRA3,
            };

            foreach (var item in ListAtributes)
            {
                if (item != string.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task AddExtraFields(DocumentDTO documento, int idDocumento)
        {
            var dictColumnsToAdd = new Dictionary<string, string>();

            if (documento.COBSERVACIONES != string.Empty)
            {
                dictColumnsToAdd["COBSERVACIONES"] = documento.COBSERVACIONES;
            }
            if (documento.CTEXTOEXTRA1 != string.Empty)
            {
                dictColumnsToAdd["CTEXTOEXTRA1"] = documento.CTEXTOEXTRA1;
            }
            if (documento.CTEXTOEXTRA2 != string.Empty)
            {
                dictColumnsToAdd["CTEXTOEXTRA2"] = documento.CTEXTOEXTRA2;
            }
            if (documento.CTEXTOEXTRA3 != string.Empty)
            {
                dictColumnsToAdd["CTEXTOEXTRA3"] = documento.CTEXTOEXTRA3;
            }

            await _sdkRepo.SetDatoDocumento(dictColumnsToAdd, idDocumento);
        }
    }
}
