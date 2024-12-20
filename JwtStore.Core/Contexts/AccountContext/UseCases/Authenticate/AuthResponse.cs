﻿using Flunt.Notifications;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class AuthResponse : SharedContext.UseCases.Response {

    protected AuthResponse() { }

    public AuthResponse(string message, int status,
        IEnumerable<Notification>? notifications = null) {

        Message = message;
        Status = status;
        Notifications = notifications;
    }

    public AuthResponse(string message, ResponseData data) {

        Message = message;
        Status = 201;
        Notifications = null;
        Data = data;
    }

    public ResponseData? Data { get; set; }
}

public record ResponseData{
    public string Token { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string[] Roles { get; set; } = [];
};
