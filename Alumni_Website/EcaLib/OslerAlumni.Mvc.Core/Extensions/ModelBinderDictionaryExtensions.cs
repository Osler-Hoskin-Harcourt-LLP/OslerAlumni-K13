using System.Web.Mvc;
using CMS.DocumentEngine;
using OslerAlumni.Mvc.Core.ModelBinders;

namespace OslerAlumni.Mvc.Core.Extensions
{
    public static class ModelBinderDictionaryExtensions
    {
        public static ModelBinderDictionary AddPageModelBinder<T>(
            this ModelBinderDictionary binders)
            where T: TreeNode, new()
        {
            binders?.Add(
                typeof(T),
                new PageModelBinder<T>());

            return binders;
        }
    }
}
