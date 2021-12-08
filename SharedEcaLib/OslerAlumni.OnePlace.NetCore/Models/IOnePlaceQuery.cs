using System.Collections.Generic;

namespace OslerAlumni.OnePlace.Models
{
    public interface IOnePlaceQuery
    {
        string QueryText { get; }

        IOnePlaceQuery Columns(
            IList<string> columnNames);

        IOnePlaceQuery OrderBy(
            string orderBy);

        IOnePlaceQuery TopN(
            int count);

        IOnePlaceQuery Where(
            IOnePlaceWhereCondition where);
    }
}
