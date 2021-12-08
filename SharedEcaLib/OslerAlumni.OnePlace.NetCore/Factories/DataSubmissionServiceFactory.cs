using System;
using System.Linq;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.OnePlace.Factories
{
    public class DataSubmissionServiceFactory
        : IDataSubmissionServiceFactory
    {
        #region "Private fields"

        private readonly IDataSubmissionService[] _dataSubmissionServices;

        #endregion

        public DataSubmissionServiceFactory(
            IDataSubmissionService[] dataSubmissionServices)
        {
            _dataSubmissionServices = dataSubmissionServices;
        }

        #region "Methods"

        /// <inheritdoc />
        public IDataSubmissionService GetDataSubmissionService(
            Type contextType,
            Type type)
        {
            if (contextType == null)
            {
                throw new ArgumentNullException(nameof(contextType));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var service = _dataSubmissionServices
                .FirstOrDefault(s => s.AppliesTo(contextType, type));

            return service;
        }

        #endregion
    }
}
