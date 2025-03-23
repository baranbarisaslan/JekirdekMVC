using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace JekirdekCRM.Helpers
{
    public static class SessionHelper
    {
        private const string UserSessionKey = "User";

        public static User? FindUser(HttpContext httpContext)
        {
            var json = httpContext.Session.GetString(UserSessionKey);
            return string.IsNullOrEmpty(json)
                ? null
                : JsonConvert.DeserializeObject<User>(json);
        }

        public static void SetUserSessionAndViewBag(HttpContext httpContext, User user, ViewDataDictionary viewData)
        {
            var json = JsonConvert.SerializeObject(user);
            httpContext.Session.SetString(UserSessionKey, json);

            viewData["IndexModel"] = new LayoutViewModel
            {
                User = user
            };
        }
    }

}
