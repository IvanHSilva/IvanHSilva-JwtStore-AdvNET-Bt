﻿using Flunt.Notifications;
using Flunt.Validations;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Insert;

public static class Specification {

    public static Contract<Notification> Ensure(Request request) =>
        new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Name.Length, 150, "Name", "O nome deve conter menos que 150 caracteres")
            .IsGreaterThan(request.Name.Length, 3, "Name", "O nome deve conter mais que 3 caracteres")
            .IsLowerThan(request.Password.Length, 40, "Password", "A senha deve conter menos que 40 caracteres")
            .IsGreaterThan(request.Password.Length, 8, "Password", "A senha deve conter mais que 8 caracteres")
            .IsEmail(request.Email, "Email", "E-mail inválido");
}
