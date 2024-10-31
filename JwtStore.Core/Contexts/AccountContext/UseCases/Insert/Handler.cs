using Flunt.Notifications;
using Flunt.Validations;
using JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;
using JwtStore.Core.Contexts.SharedContext.UseCases;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Insert; 

public class Handler {

    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository, IService service) {
        
        _repository = repository;
        _service = service; 
    }

    public async Task<Response> Handle(Request request, 
        CancellationToken cancellation) {

        #region RequestValidation
        try {
            Contract<Notification> response = Specification.Ensure(request);
            if (!response.IsValid) 
                return new Response("Requisição Inválida!", 400, response.Notifications);
        } catch {
            return new Response("Não Foi Possível Validar a Requisição!", 500);
        }
        #endregion
    }
}
