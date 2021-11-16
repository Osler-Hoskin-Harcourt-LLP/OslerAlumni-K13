using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Relationships;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Definitions;

namespace ECA.Content.Repositories
{
    public class DocumentRelationshipRepository
        : IDocumentRelationshipRepository
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;

        #endregion

        public DocumentRelationshipRepository(
            ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        #region "Methods"

        public IList<RelationshipInfo> GetDocumentRelationships(
            TreeNode page,
            RelationshipNameInfo relationshipNameInfo)
        {
            if ((page == null) || (relationshipNameInfo == null))
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Relationships.RelationshipsByNameAndLeftNodeId,
                    relationshipNameInfo,
                    page.NodeID),
                IsCultureSpecific = false,
                IsSiteSpecific = true,
                // Bust the cache whenever:
                // - the page type schema (e.g. field name) is modified;
                // - the relationship object is modified;
                // - relationships are added or removed for a node.
                CacheDependencies = new List<string>
                {
                    $"{DocumentTypeInfo.OBJECT_TYPE_DOCUMENTTYPE}|byname|{page.NodeClassName}",
                    $"{RelationshipNameInfo.OBJECT_TYPE}|byname|{relationshipNameInfo.RelationshipName}",
                    $"nodeid|{page.NodeID}|relationships"
                }
            };

            var result = _cacheService.Get(
                cp => 
                    RelationshipInfoProvider
                        .GetRelationships()
                        .WhereEquals(
                            nameof(RelationshipInfo.RelationshipNameId),
                            relationshipNameInfo.RelationshipNameId)
                        .WhereEquals(
                            nameof(RelationshipInfo.LeftNodeId),
                            page.NodeID)
                        .ToList(),
                cacheParameters);

            return result;
        }

        public RelationshipNameInfo GetRelationshipNameByPageField(
            TreeNode page,
            string fieldName)
        {
            if ((page == null)
                || string.IsNullOrWhiteSpace(fieldName))
            {
                return null;
            }

            var pageTypeName = page.NodeClassName;

            if (string.IsNullOrWhiteSpace(pageTypeName))
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Classes.RelationshipByClassAndField,
                    pageTypeName,
                    fieldName),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever the page type schema (e.g. field name) is modified
                CacheDependencies = new List<string>
                {
                    $"{DocumentTypeInfo.OBJECT_TYPE_DOCUMENTTYPE}|byname|{pageTypeName}"
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var relationshipFormField = FormHelper
                        .GetFormInfo(pageTypeName, false, true, false)
                        .GetFormField(fieldName);

                    if (relationshipFormField == null)
                    {
                        return null;
                    }

                    var relationshipCodeName = RelationshipNameInfoProvider
                        .GetAdHocRelationshipNameCodeName(
                            pageTypeName,
                            relationshipFormField);

                    // Bust the cache whenever the relationship object is modified
                    cp.CacheDependencies.Add(
                        $"{RelationshipNameInfo.OBJECT_TYPE}|byname|{relationshipCodeName}");

                    return RelationshipNameInfoProvider
                        .GetRelationshipNameInfo(relationshipCodeName);
                },
                cacheParameters);

            return result;
        }

        #endregion
    }
}
