using System.Collections.Generic;
using CMS.DataEngine;
using ECA.Core.Definitions;
using ECA.Core.Repositories;

namespace ECA.Content.Repositories
{
    public interface IKenticoClassRepository
        : IRepository
    {
        IEnumerable<DataClassInfo> GetClasses(
            KenticoClassType classType,
            IEnumerable<string> classNames = null,
            IEnumerable<string> columnNames = null);

        /// <summary>
        /// Checks if the class is a direct child of another class.
        /// </summary>
        /// <param name="childClassName"></param>
        /// <param name="parentClassName"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        bool IsChildClass(
            string childClassName,
            string parentClassName,
            KenticoClassType classType);
    }
}
