﻿using Flunt.Notifications;
using Flunt.Validations;
using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;
using JwtStore.Core.Contexts.AccountContext.ValueObjects;
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

        #region ObjectsGenerate

        Email email;
        Password password;
        User user;

        try {
            email = new Email(request.Email);
            password = new Password(request.Password);
            user = new User(request.Name, email, password);
        }
        catch(Exception ex) {
            return new Response(ex.Message, 400);
        }

        #endregion
    }
}