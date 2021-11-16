using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using ECA.Core.Repositories;
using ECA.Core.Services;
using NetTools;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class IpLocatorService
         : ServiceBase, IIpLocatorService
    {
        private readonly IEventLogRepository _eventLogRepository;

        private readonly OslerNetworkConfig _oslerNetworkConfig;

        private List<IPAddressRange> _oslerNetworkIpAddressRages;

        private List<IPAddressRange> OslerNetworkIpAddressRages
        {
            get
            {
                if (_oslerNetworkIpAddressRages == null)
                {
                    _oslerNetworkIpAddressRages = GetNetworkIpAddressRages(
                        _oslerNetworkConfig.OslerIpAddressArray);
                }

                return _oslerNetworkIpAddressRages;
            }
        }


        public IpLocatorService(IEventLogRepository eventLogRepository, OslerNetworkConfig oslerNetworkConfig)
        {
            _oslerNetworkConfig = oslerNetworkConfig;
            _eventLogRepository = eventLogRepository;
        }

        /// <summary>
        /// Returns the Ip Address of the user.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserIpAddress()
        {
            var xForwardedFor = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];

            if (_oslerNetworkConfig.LogAllIps)
            {
                string ipAddressMsg = $"UserHostAddress ip address: {HttpContext.Current.Request.UserHostAddress}";

                if (!string.IsNullOrWhiteSpace(xForwardedFor))
                {
                    ipAddressMsg = $"{ipAddressMsg}; X_FORWARDED_FOR ip address: {xForwardedFor}";
                }

                _eventLogRepository.LogInformation(GetType(), nameof(GetCurrentUserIpAddress), ipAddressMsg);
            }


            return HttpContext.Current.Request.UserHostAddress;
        }

        public bool IsCurrentUserInOslerNetwork()
        {
            var userIpAddress = IPAddress.Parse(GetCurrentUserIpAddress());

            return OslerNetworkIpAddressRages.Any(ipRange => ipRange.Contains(userIpAddress));
        }

        private List<IPAddressRange> GetNetworkIpAddressRages(string [] ipAddressRanges)
        {
            return ipAddressRanges?.Select(IPAddressRange.Parse).ToList();
        }
    }
}
