using System;

namespace OslerAlumni.Mvc.Extensions
{
    /// <summary>Represents a point for extension methods.</summary>
    /// <typeparam name="T">The type of the class that is a target of extension methods.</typeparam>
    public sealed class OslerExtensionPoint<T> where T : class
    {
        /// <summary>
        /// Instance of class that is a target of extension methods.
        /// </summary>
        public T Target { get; }

        internal OslerExtensionPoint(T target)
        {
            if ((object)target == null)
                throw new ArgumentNullException(nameof(target));
            this.Target = target;
        }
    }
}
