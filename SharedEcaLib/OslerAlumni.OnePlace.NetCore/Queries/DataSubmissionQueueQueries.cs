using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OslerAlumni.OnePlace.Queries
{
    public static class DataSubmissionQueueQueries
    {
        public static string PurgeDataSubmissionQueueItems =>
            @";WITH RecordsToPurgeCTE AS (

                SELECT top (@Count) * FROM 
                [OslerAlumni_CustomTable_DataSubmissionQueue] DSQ1
                WHERE ItemModifiedWhen <= DATEADD(day, -@ModifiedDayCount, GETDATE())
                AND 
                (
	                (
	                --SELECT ALL TASKS WHICH ARE PROCESSED AND HAVE NO TASKS DEPENDING ON THEM
	                DSQ1.IsProcessed = 1
	                AND NOT EXISTS(
		                SELECT * FROM [OslerAlumni_CustomTable_DataSubmissionQueue] DSQ2
		                WHERE DSQ2.IsProcessed = 0
		                AND DSQ2.DependsOnItemIds Is Not NULL
		                AND (DSQ2.DependsOnItemIds = STR(DSQ1.ItemId) 
			                OR 	DSQ2.DependsOnItemIds like '%;' + STR(DSQ1.ItemId) +';%' 
			                OR 	DSQ2.DependsOnItemIds Like STR(DSQ1.ItemId) +';%' 
			                OR DSQ2.DependsOnItemIds Like '%;' + STR(DSQ1.ItemId)) 
		                )
	                )
	                OR 
	                ( 
		                --SELECT ALL TASKS WHOSE PARENTS NO LONGER EXISTS (THESE WILL FAIL REGARDLESS)
		                DSQ1.DependsOnItemIds is not null
		                AND EXISTS ( SELECT * FROM 
					                 Func_SplitString(DSQ1.DependsOnItemIds,';') DependsOnItemId
					                 LEFT OUTER JOIN [OslerAlumni_CustomTable_DataSubmissionQueue] DSQ2
					                 ON DSQ2.ItemId = DependsOnItemId.Name
					                 WHERE DSQ2.ItemId IS NULL)
	                )
	                OR 
	                (
		                --SELECT ALL TASKS WHICH FAILED TOO MANY TIMES
		                DSQ1.TotalAttempts >= @TotalAttempts
	                )
                )
                )

                DELETE Top (@Count) FROM RecordsToPurgeCTE
                SELECT @@ROWCOUNT AS DELETED;";
    }
}
