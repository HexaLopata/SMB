using System.Web.Hosting;

namespace SMB.Models
{
    public static class VirtualPathCorrector
    {
        public static string CorrectVirtualPath => HostingEnvironment.ApplicationVirtualPath == "/" ?
                                                   HostingEnvironment.ApplicationVirtualPath :
                                                   HostingEnvironment.ApplicationVirtualPath + "/";
    }
}