using System;
using Autofac;
using CMS.Core;
using ECA.Admin.Core.Extensions;
using Module = CMS.DataEngine.Module;

namespace ECA.Admin.Core.Modules
{
    public abstract class BaseModule
        : Module, IDisposable
    {
        #region "Private fields"

        private bool _isDisposed = false;
        private ILifetimeScope _scope;

        #endregion

        /// <summary>Constructor</summary>
        /// <param name="metadata">Module metadata</param>
        /// <param name="isInstallable">Indicates if module is designed as installable.</param>
        protected BaseModule(ModuleMetadata metadata, bool isInstallable = false)
            : base(metadata, isInstallable)
        {
        }

        /// <summary>Constructor</summary>
        /// <param name="moduleName">Module name</param>
        /// <param name="isInstallable">Indicates if module is designed as installable.</param>
        protected BaseModule(string moduleName, bool isInstallable = false)
            : base(new ModuleMetadata(moduleName), isInstallable)
        {
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

        ~BaseModule()
        {
            // Calling in the destructor, in case a derived class has unmanaged resources
            Dispose(false);
        }

        #region "Module events"

        protected override void OnInit()
        {
            base.OnInit();

            // N.B. Can't do this in the constructor, as Modules are registered before DI types
            _scope = this.InjectDependencies();
        }

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
