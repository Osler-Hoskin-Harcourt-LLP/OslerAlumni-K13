using ECA.Core.Models;

namespace OslerAlumni.Mvc.Core.Models
{
    public class OslerNetworkConfig
        : IConfig
    {
        public string OslerIpAddresses { get; set; }

        public string[] OslerIpAddressArray => OslerIpAddresses.Split(';');

        public bool LogAllIps { get; set; }

    }
}
