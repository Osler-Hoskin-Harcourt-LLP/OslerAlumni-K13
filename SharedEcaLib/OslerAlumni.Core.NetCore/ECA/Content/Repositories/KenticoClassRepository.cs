using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Definitions;
using ECA.Core.Extensions;

namespace ECA.Content.Repositories
{
    public class KenticoClassRepository
        : IKenticoClassRepository
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;

        #endregion

        public KenticoClassRepository(
            ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        #region "Methods"

        public IEnumerable<DataClassInfo> GetClasses(
            KenticoClassType classType,
            IEnumerable<string> classNames = null,
            IEnumerable<string> columnNames = null)
        {
            var classNameList = classNames?.ToArray();
            var columnNameList = columnNames?.ToArray();

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Classes.ClassesByNamesAndColumns,
                    classNameList.JoinSorted(
                        ECAGlobalConstants.Caching.Separator,
                        ECAGlobalConstants.Caching.All),
                    columnNameList.JoinSorted(
                        ECAGlobalConstants.Caching.Separator,
                        ECAGlobalConstants.Caching.All)),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the requested classes is modified
                CacheDependencies =
                    classNameList?
                        .Select(cn =>
                            $"{classType.ToStringRepresentation()}|byname|{cn}")
                        .ToList()
                    ??
                    new List<string>
                    {
                        $"{classType.ToStringRepresentation()}|all"
                    }
            };

            var result = _cacheService.Get(
                () =>
                {
                    var query = DataClassInfoProvider
                        .GetClasses()
                        .Columns(columnNameList);

                    switch (classType)
                    {
                        case KenticoClassType.CustomTable:
                            query = query
                                .WhereEquals(
                                    nameof(DataClassInfo.ClassIsCustomTable),
                                    classType == KenticoClassType.PageType);

                            break;
                        case KenticoClassType.PageType:
                            query = query
                                .WhereEquals(
                                    nameof(DataClassInfo.ClassIsDocumentType),
                                    classType == KenticoClassType.PageType);

                            break;
                    }

                    if ((classNameList != null) && (classNameList.Length > 0))
                    {
                        query = query
                            .WhereIn(
                                nameof(DataClassInfo.ClassName),
                                classNameList);
                    }

                    return query.ToList();
                },
                cacheParameters);

            return result;
        }

        /// <summary>
        /// Checks if the class is a direct child of another class.
        /// </summary>
        /// <param name="childClassName"></param>
        /// <param name="parentClassName"></param>
        /// <param name="classType"></param>
        /// <returns></returns>
        public bool IsChildClass(
            string childClassName,
            string parentClassName,
            KenticoClassType classType)
        {
            if (string.IsNullOrWhiteSpace(childClassName)
                || string.IsNullOrWhiteSpace(parentClassName))
            {
                return false;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Classes.IsChildClass,
                    childClassName,
                    parentClassName),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever either of the classes is modified
                CacheDependencies = new List<string>
                {
                    $"{classType.ToStringRepresentation()}|byname|{childClassName}",
                    $"{classType.ToStringRepresentation()}|byname|{parentClassName}"
                }
            };

            var result = _cacheService.Get(
                () =>
                {
                    var classes = GetClasses(
                            classType,
                            new List<string>
                            {
                                childClassName,
                                parentClassName
                            },
                            new List<string>
                            {
                                nameof(DataClassInfo.ClassID),
                                nameof(DataClassInfo.ClassName),
                                nameof(DataClassInfo.ClassInheritsFromClassID)
                            })
                        .ToList();

                    if (classes.Count < 2)
                    {
                        return false;
                    }

                    var childClass = classes
                        .FirstOrDefault(c =>
                            string.Equals(
                                c.ClassName,
                                childClassName,
                                StringComparison.OrdinalIgnoreCase));

                    if ((childClass == null)
                        || (childClass.ClassInheritsFromClassID < 1))
                    {
                        return false;
                    }

                    var parentClass = classes
                        .FirstOrDefault(c =>
                            string.Equals(
                                c.ClassName,
                                parentClassName,
                                StringComparison.OrdinalIgnoreCase));

                    if (parentClass == null)
                    {
                        return false;
                    }

                    return
                        childClass.ClassInheritsFromClassID == parentClass.ClassID;
                },
                cacheParameters);

            return result;
        }

        #endregion
    }
}
