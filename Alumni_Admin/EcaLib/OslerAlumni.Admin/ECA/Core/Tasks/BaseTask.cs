using System;
using Autofac;
using CMS.Scheduler;
using ECA.Admin.Core.Extensions;

namespace ECA.Admin.Core.Tasks
{
    public abstract class BaseTask
        : ITask, IDisposable
    {
        #region "Private fields"

        private readonly ILifetimeScope _scope;
        private bool _isDisposed = false;

        #endregion

        protected BaseTask()
        {
            _scope = this.InjectDependencies();
        }

        public virtual void Dispose()
        {
            // Dispose of managed and unmanaged resources
            Dispose(true);

            // This is necessary in case the child class has a destructor/finalizer defined on it,
            // to signal to GC that all resources have already been disposed of here and the object
            // shouldn't go into the finalizer queue
            GC.SuppressFinalize(this);
        }

        ~BaseTask()
        {
            // Calling in the destructor, in case a derived class has unmanaged resources
            Dispose(false);
        }

        #region "Methods"

        public abstract string Execute(
            TaskInfo task);

        #endregion

        #region "Helper methods"

        protected virtual void Dispose(
            bool isDisposing)
        {
            if (_isDisposed)
            {
                return;
            }

            // Dispose of managed resources
            if (isDisposing)
            {
                // NOTE: It is unclear if this is still the case, but there are posts out there that claim 
                // that the Autofac lifetime scope in which you resolve components (dependencies) has to stay alive
                // while you are still using those resolved instances. Which is why we are
                // taking responsibility over the scope disposal in this way.
                _scope?.Dispose();
            }

            // Don't have any unmanaged resources in the base class

            _isDisposed = true;
        }

        #endregion
    }
}
