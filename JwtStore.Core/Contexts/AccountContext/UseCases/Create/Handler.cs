using Flunt.Notifications;
using Flunt.Validations;
using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.SharedContext.UseCases;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Create;

public class Handler(IRepository repository, IService service) : IRequestHandler<Request, Response> {

    private readonly IRepository _repository = repository;
    private readonly IService _service = service;

    public async Task<Response> Handle(Request request,
        CancellationToken cancellationToken) {

        #region RequestValidation

        try {
            Contract<Notification> response = Specification.Ensure(request);
            if (!response.IsValid)
                return new Response("Requisição Inválida!", 400, response.Notifications);
        }
        catch {
            return new Response("Não Foi Possível Validar a Requisição!", 500);
        }

        #endregion


        #region ObjectsGenerate

        Email email;
        Password password;
        User user;

        try {
            email = new Email(request.Email);
            password = new Password(request.Password);
            user = new User(request.Name, email, password);
        }
        catch (Exception ex) {
            return new Response(ex.Message, 400);
        }

        #endregion


        #region ChecksIfUserExistsInDatabase

        try {
            bool exists = await _repository.AnyAsync(request.Email, cancellationToken);
            if (exists)
                return new Response("Este E-mail Já Foi Utilizado!", 400);
        }
        catch {
            return new Response("Falha ao Verificar E-Mail Cadastrado", 500);
        }
        #endregion


        #region DataPersists

        try {
            await _repository.SaveAsync(user, cancellationToken);
        }
        catch {
            return new Response("Falha ao Persistir Dados", 500);
        }
        #endregion


        #region SendRequestEmailActivation

        try {
            await _service.SendVerificationEmailAsync(user, cancellationToken);
        }
        catch {
            //return new Response("Falha ao Persistir Dados", 500);
        }
        #endregion

        return new Response("Conta Criada com Sucesso", new ResponseData(
            user.Id, user.Name, user.Email));
    }
}
