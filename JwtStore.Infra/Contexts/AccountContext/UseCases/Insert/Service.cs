﻿using JwtStore.Core;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Insert;

public class Service : IService {

    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken) {
        SendGridClient client = new(Configuration.SendGrid.ApiKey);
        EmailAddress from = new (Configuration.Email.DefaultFromEmail, Configuration.Email.DefaultFromName);
        const string subject = "Verifique sua conta";
        EmailAddress to = new (user.Email, user.Name);
        string content = $"Código {user.Email.Verification.Code}";
        SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        await client.SendEmailAsync(msg, cancellationToken);
    }
}