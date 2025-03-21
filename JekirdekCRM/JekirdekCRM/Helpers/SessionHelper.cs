using JekirdekCRM.Models.DBModels;
using JekirdekCRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace JekirdekCRM.Helpers
{
    public class SessionHelper
    {
        public static User? FindUser(HttpContext httpContext)
        {
            string? jsn = httpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(jsn))
            {
                return null;
            }
            else
            {
                User? user = JsonConvert.DeserializeObject<User>(jsn);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }

            }

        }

        public static void SetUserSessionAndViewBag(HttpContext httpContext, User user, ViewDataDictionary viewData)
        {
            string jsn = JsonConvert.SerializeObject(user);
            httpContext.Session.SetString("User", jsn);

            LayoutViewModel layout = new LayoutViewModel
            {
                User = user
            };
            viewData["IndexModel"] = layout;
        }
    }
}
