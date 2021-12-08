using System.Collections.Generic;

namespace OslerAlumni.OnePlace.Delegates
{
    public delegate bool ListFunctionDelegate<T>(
        IList<string> mainReferences,
        IList<string> columnNames,
        out IList<T> records,
        out string errorMessage,
        int? topN)
        where T : class, new();
}
