using System.Web;

namespace SMB.Models.Autentification
{
    interface ICookieManager
    {
        string AutorizationCookieName { get; }
        bool CheckForAutorisationCookie(HttpRequestBase request);
        void CreateAndAddAutorisationCookie(HttpResponseBase response);
    }
}
