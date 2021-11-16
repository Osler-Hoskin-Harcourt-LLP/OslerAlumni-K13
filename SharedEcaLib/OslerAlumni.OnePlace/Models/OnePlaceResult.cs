using Salesforce.Common.Models.Json;
using Salesforce.Force;

namespace OslerAlumni.OnePlace.Models
{
    public class OnePlaceResult<T>
    {
        public IForceClient Client { get; set; }

        public QueryResult<T> Response { get; set; }
    }
}
