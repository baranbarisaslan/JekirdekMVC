using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using JekirdekCRM.Models;
using JekirdekCRM.Models.DBModels;

public static class SessionHelper
{
    private const string UserSessionKey = "User";

    public static User? FindUser(HttpContext httpContext)
    {
        if (httpContext == null)
            return null;

        var json = httpContext.Session.GetString(UserSessionKey);
        return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<User>(json);
    }

    public static void SetUserSession(HttpContext httpContext, User user)
    {
        if (user == null || httpContext == null)
            return;

        var json = JsonConvert.SerializeObject(user);
        httpContext.Session.SetString(UserSessionKey, json);
    }

    public static void ClearUserSession(HttpContext httpContext)
    {
        httpContext?.Session?.Remove(UserSessionKey);
    }
}
