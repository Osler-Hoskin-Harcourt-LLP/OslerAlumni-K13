using OslerAlumni.OnePlace.Definitions;

namespace OslerAlumni.OnePlace.Models
{
    public class DataSubmissionResult
    {
        #region "Properties"

        public int TaskId { get; set; }

        public string ExternalId { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        #endregion

        #region "Methods"

        public string GetObjectId(
            bool checkSuccess = true)
        {
            return ((Success || !checkSuccess)
                    && !string.IsNullOrWhiteSpace(ExternalId))
                ? ExternalId
                : string.Format(
                    DataSubmissionConstants.Placeholders.ID,
                    TaskId);
        }

        #endregion
    }
}
