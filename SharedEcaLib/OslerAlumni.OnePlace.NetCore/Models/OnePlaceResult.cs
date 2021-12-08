using Salesforce.Force;

namespace OslerAlumni.OnePlace.Models
{
    public class OnePlaceResult<T>
    {
        public IForceClient Client { get; set; }

        public Salesforce.Common.Models.Json.QueryResult<T> Response { get; set; }
    }
}
