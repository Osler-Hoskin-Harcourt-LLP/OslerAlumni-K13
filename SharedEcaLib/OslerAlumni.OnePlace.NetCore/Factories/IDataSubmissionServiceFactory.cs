using System;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.OnePlace.Factories
{
    public interface IDataSubmissionServiceFactory
    {
        /// <summary>
        /// Retrieves an implementation of <see cref="IDataSubmissionService"/>, which applies to
        /// the specific type of a context object and a specific type of data submission object.
        /// </summary>
        /// <param name="contextType">
        /// Type of the Kentico object (e.g. User or Contact), in whose "context"
        /// a OnePlace object should be created/updated/deleted.
        /// </param>
        /// <param name="type">
        /// Type of the OnePlace object that should be created/updated/deleted.
        /// </param>
        /// <returns></returns>
        IDataSubmissionService GetDataSubmissionService(
            Type contextType,
            Type type);
    }
}
