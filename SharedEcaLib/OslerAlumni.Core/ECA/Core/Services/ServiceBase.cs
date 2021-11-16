using System;

namespace ECA.Core.Services
{
    public abstract class ServiceBase
        : IService
    {
        /// <summary>Dispose</summary>
        public void Dispose()
        {
            // Dispose of managed and unmanaged resources
            Dispose(true);

            // This is necessary in case the child class has a destructor/finalizer defined on it,
            // to signal to GC that all resources have already been disposed of here and the object
            // shouldn't go into the finalizer queue
            GC.SuppressFinalize(this);
        }
        
        ~ServiceBase()
        {
            // Calling in the destructor, in case a derived class has unmanaged resources
            Dispose(false);
        }

        #region "Helper methods"

        protected virtual void Dispose(
            bool isDisposing)
        {
            // Don't have any unmanaged resources, so do nothing.
            // If any of the child classes, do have unmanaged resources to be disposed of,
            // they should override this method.
        }

        #endregion
    }
}
