using System;
using CMS.EventLog;
using ECA.Core.Models;

namespace ECA.Core.Repositories
{
    public class EventLogRepository
        : IEventLogRepository
    {
        #region "Private fields"

        private readonly ContextConfig _context;

        #endregion

        public EventLogRepository(
            ContextConfig context)
        {
            _context = context;
        }

        #region "Methods"


        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="source">Source of the exception.</param>
        /// <param name="eventCode">Event code for the exception.</param>
        /// <param name="ex">Exception instance to log.</param>
        public void LogError(
            string source,
            string eventCode,
            Exception ex)
        {
            LogEvent(source, eventCode, exception: ex);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="source">Source of the exception.</param>
        /// <param name="eventCode">Event code for the exception.</param>
        /// <param name="ex">Exception instance to log.</param>
        public void LogError(
            Type source,
            string eventCode,
            Exception ex)
        {
            if (source != null)
            {
                LogError(source.FullName, eventCode, ex);
            }
        }


        public void LogError(
            Type source,
            string eventCode,
            string errorMessage)
        {
            if (source != null)
            {
                LogError(
                    source.FullName, 
                    eventCode, 
                    new Exception(errorMessage));
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventCode">The event code.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="documentName">Name of the document.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="eventDescription">The event description.</param>
        /// <param name="exception">Error exception.</param>
        public void LogEvent(
            string source,
            string eventCode,
            int? siteId = null,
            int nodeId = 0,
            string documentName = null,
            string eventType = null,
            string eventDescription = null,
            Exception exception = null)
        {
            if (string.IsNullOrEmpty(eventType))
            {
                eventType = (exception == null)
                    ? EventType.INFORMATION
                    : EventType.ERROR;
            }

            var logItem = new EventLogInfo
            {
                EventType = eventType,
                EventTime = DateTime.Now,
                Source = source,
                EventCode = eventCode,
                EventDescription = eventDescription,
                Exception = exception,
                SiteID = (siteId ?? _context.Site?.SiteID) ?? 0,
                NodeID = nodeId,
                DocumentName = documentName,
                UserID = _context.User?.UserID ?? 0,
                UserName = _context.User?.UserName,
                // TODO: [VI] Pass in request and system contexts from Admin and MVC
                //IPAddress = RequestContext.UserHostAddress,
                //EventMachineName = SystemContext.MachineName,
                //EventUrl = RequestContext.RawURL,
                //EventUrlReferrer = RequestContext.URLReferrer,
                //EventUserAgent = RequestContext.UserAgent
            };

            EventLogProvider.LogEvent(logItem);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="source">Source of the event.</param>
        /// <param name="eventCode">Event code for the event.</param>
        /// <param name="eventDescription">Description of the event.</param>
        public void LogInformation(
            string source,
            string eventCode,
            string eventDescription)
        {
            LogEvent(source, eventCode, eventDescription: eventDescription);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="source">Source of the event.</param>
        /// <param name="eventCode">Event code for the event.</param>
        /// <param name="eventDescription">Description of the event.</param>
        public void LogInformation(
            Type source,
            string eventCode,
            string eventDescription)
        {
            if (source != null)
            {
                LogInformation(source.FullName, eventCode, eventDescription);
            }
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="source">Source of the event.</param>
        /// <param name="eventCode">Event code for the event.</param>
        /// <param name="eventDescription">Description of the event.</param>
        public void LogWarning(
            string source,
            string eventCode,
            string eventDescription)
        {
            LogEvent(
                source,
                eventCode,
                eventType: EventType.WARNING,
                eventDescription: eventDescription);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="source">Source of the event.</param>
        /// <param name="eventCode">Event code for the event.</param>
        /// <param name="eventDescription">Description of the event.</param>
        public void LogWarning(
            Type source,
            string eventCode,
            string eventDescription)
        {
            if (source != null)
            {
                LogWarning(
                    source.FullName,
                    eventCode,
                    eventDescription);
            }
        }

        #endregion
    }
}
