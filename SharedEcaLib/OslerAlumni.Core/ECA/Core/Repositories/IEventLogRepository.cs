using System;

namespace ECA.Core.Repositories
{
    public interface IEventLogRepository
        : IRepository
    {
        void LogError(
            string source, 
            string eventCode, 
            Exception ex);

        void LogError(
            Type source, 
            string eventCode,
            Exception ex);

        void LogError(
            Type source,
            string eventCode,
            string errorMessage);

        void LogEvent(
            string source,
            string eventCode,
            int? siteId = null,
            int nodeId = 0,
            string documentName = null,
            string eventType = null,
            string eventDescription = null,
            Exception exception = null);

        void LogInformation(
            string source, 
            string eventCode, 
            string eventDescription);

        void LogInformation(
            Type source, 
            string eventCode, 
            string eventDescription);

        void LogWarning(
            string source, 
            string eventCode, 
            string eventDescription);

        void LogWarning(
            Type source,
            string eventCode,
            string eventDescription);
    }
}
