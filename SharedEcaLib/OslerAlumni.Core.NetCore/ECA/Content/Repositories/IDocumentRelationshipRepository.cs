using System.Collections.Generic;
using CMS.DocumentEngine;
using CMS.Relationships;
using ECA.Core.Repositories;

namespace ECA.Content.Repositories
{
    public interface IDocumentRelationshipRepository
        : IRepository
    {
        IList<RelationshipInfo> GetDocumentRelationships(
            TreeNode page,
            RelationshipNameInfo relationshipNameInfo);

        RelationshipNameInfo GetRelationshipNameByPageField(
            TreeNode page,
            string fieldName);
    }
}
