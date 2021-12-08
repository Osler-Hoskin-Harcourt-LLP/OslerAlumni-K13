using System;
using System.Linq;
using Autofac;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using ECA.Core.Macros;
using ECA.PageURL.Services;

namespace ECA.PageURL.Macros
{
    public abstract class PageUrlMacroMethodsBase
        : MacroMethodsBase
    {
        #region "Properties"

        public virtual IPageUrlService PageUrlService { get; set; }

        #endregion

        #region "Methods"

        [MacroMethod(typeof(string), "Obtains the main URL for a page in the content tree.", 3)]
        [MacroMethodParam(0, "nodeGuid", typeof(Guid), "Node GUID of the page in the content tree.")]
        [MacroMethodParam(1, "cultureName", typeof(string),
            "Culture version of the page. Should contain the culture code, e.g. \"fr-CA\".")]
        [MacroMethodParam(2, "siteName", typeof(string), "Name of the site that contains the page.")]
        public virtual object GetMainPageUrl(
            EvaluationContext context,
            params object[] parameters)
        {
            var scope = ResolveDependencies();

            try
            {
                parameters = GetParameters(parameters);

                // Branches according to the number of the method's parameters
                switch (parameters.Length)
                {
                    case 3:
                        {
                            // Overload with three parameters
                            var nodeGuid = ValidationHelper.GetGuid(
                                parameters[0],
                                Guid.Empty);

                            var cultureName = ValidationHelper.GetString(
                                parameters[1],
                                string.Empty);

                            var siteName = ValidationHelper.GetString(
                                parameters[2],
                                string.Empty);

                            string url;

                            PageUrlService.TryGetPageMainUrl(
                                nodeGuid,
                                cultureName,
                                out url,
                                siteName);

                            return url;
                        }
                    default:
                        // No other overloads are supported
                        throw new NotSupportedException();
                }
            }
            finally
            {
                // NOTE: It is unclear if this is still the case, but there are posts out there that claim 
                // that the Autofac lifetime scope in which you resolve components (dependencies) has to stay alive
                // while you are still using those resolved instances. Which is why we are
                // taking responsibility over the scope disposal in this way.
                scope?.Dispose();
            }
        }

        [MacroMethod(typeof(bool), "Determines if a page in the content tree is a navigable page", 1)]
        [MacroMethodParam(0, "node", typeof(TreeNode), "Current node in the content tree.")]
        public virtual object IsNavigablePage(
            EvaluationContext context,
            params object[] parameters)
        {
            var scope = ResolveDependencies();

            try
            {
                ResolveDependencies();

                parameters = GetParameters(parameters);

                // Branches according to the number of the method's parameters
                switch (parameters.Length)
                {
                    case 1:
                        {
                            // Overload with one parameter
                            var treeNode = parameters[0] as TreeNode;


                            if (treeNode != null)
                            {
                                return PageUrlService.IsNavigable(treeNode);
                            }

                            return null;
                        }
                    default:
                        // No other overloads are supported
                        throw new NotSupportedException();
                }
            }
            finally
            {
                // NOTE: It is unclear if this is still the case, but there are posts out there that claim 
                // that the Autofac lifetime scope in which you resolve components (dependencies) has to stay alive
                // while you are still using those resolved instances. Which is why we are
                // taking responsibility over the scope disposal in this way.
                scope?.Dispose();
            }
        }

        #endregion

        #region "Helper methods"

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected virtual object[] GetParameters(
            object[] parameters)
        {
            // N.B.: Kentico will pass in one additional parameter
            // (my guess is because of the namespace and dot-notation),
            // hence the increment on the number of parameters.
            return parameters.Length > 1 ? parameters.Skip(1).ToArray() : parameters;
        }

        /// <summary>
        /// Creates and returns an Autofac lifetime scope, that is nested either
        /// in the request (if request context is available) or in the root lifetime scope.
        /// Resolves the dependencies of this class within the newly created scope.
        /// NOTE 1: The scope HAS TO stay alive while the resolved dependencies are being used.
        /// NOTE 2: The scope HAS TO be manualy disposed of, once it is safe to do so.
        /// </summary>
        protected abstract ILifetimeScope ResolveDependencies();

        #endregion
    }
}
