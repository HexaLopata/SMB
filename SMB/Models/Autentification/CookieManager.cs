using System.Web;

namespace SMB.Models.Autentification
{
    public class CookieManager : ICookieManager
    {
        public const string autorizationCookieName = "SMB_AU";

        public string AutorizationCookieName => autorizationCookieName;

        /// <summary>
        /// Проверяет на наличие куки с авторизацией
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true - если куки найден, false - если нет</returns>
        public bool CheckForAutorisationCookie(HttpRequestBase request)
        {
            var cookie = request.Cookies.Get(autorizationCookieName);
            if (cookie != null)
                return true;
            else
                return false;
        }

        public void CreateAndAddAutorisationCookie(HttpResponseBase response)
        {
            var cookie = new HttpCookie("SMB_AU");
            response.Cookies.Add(cookie);
        }
    }
}