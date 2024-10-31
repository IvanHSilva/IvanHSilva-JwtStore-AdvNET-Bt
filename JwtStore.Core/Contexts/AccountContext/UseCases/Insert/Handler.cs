using JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Insert; 

public class Handler {

    private readonly IRepository _repository;
    private readonly IService _service;

    public Handler(IRepository repository, IService service) {
        
        _repository = repository;
        _service = service; 
    }

    public async Task<Response> Handle(Request request, 
        CancellationToken cancellation) {}
}
