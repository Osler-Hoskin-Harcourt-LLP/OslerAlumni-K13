using ECA.Core.Repositories;
using ECA.Core.Services;
using Microsoft.AspNetCore.Http;
using NetTools;
using OslerAlumni.Mvc.Core.Models;
using System.Net;

namespace OslerAlumni.Mvc.Core.Services
{
    public class IpLocatorService
         : ServiceBase, IIpLocatorService
    {
        private readonly IEventLogRepository _eventLogRepository;

        private readonly OslerNetworkConfig _oslerNetworkConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
            
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


        public IpLocatorService(IEventLogRepository eventLogRepository, OslerNetworkConfig oslerNetworkConfig, IHttpContextAccessor httpContextAccessor)
        {
            _oslerNetworkConfig = oslerNetworkConfig;
            _eventLogRepository = eventLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Returns the Ip Address of the user.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserIpAddress()
        {
            var xForwardedFor = _httpContextAccessor.HttpContext.Request.Headers["X_FORWARDED_FOR"];

            if (_oslerNetworkConfig.LogAllIps)
            {
                string ipAddressMsg = $"UserHostAddress ip address: {_httpContextAccessor.HttpContext.Connection.RemoteIpAddress}";

                if (!string.IsNullOrWhiteSpace(xForwardedFor))
                {
                    ipAddressMsg = $"{ipAddressMsg}; X_FORWARDED_FOR ip address: {xForwardedFor}";
                }

                _eventLogRepository.LogInformation(GetType(), nameof(GetCurrentUserIpAddress), ipAddressMsg);
            }


            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
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
