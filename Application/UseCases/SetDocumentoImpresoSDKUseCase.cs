using Domain.Interfaces;

namespace Application.UseCases
{
    public class SetDocumentoImpresoSDKUseCase
    {
        private readonly ISDKRepo _sdkRepo;
        public SetDocumentoImpresoSDKUseCase(ISDKRepo sdkRepo)
        {
            _sdkRepo = sdkRepo;
        }

        public async Task Execute(int idDocumento)
        {
            await _sdkRepo.SetImpreso(idDocumento, true);
        }
    }
}
