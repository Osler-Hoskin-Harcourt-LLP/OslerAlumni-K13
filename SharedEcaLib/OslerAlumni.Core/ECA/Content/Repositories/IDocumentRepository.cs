using System;
using System.Collections.Generic;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using ECA.Core.Repositories;

namespace ECA.Content.Repositories
{
    /// <remarks>
    /// This class and its implementations do NOT use caching.
    /// Caching should be applied on the results of corresponding methods,
    /// depending on the individual usage context.
    /// </remarks>
    public interface IDocumentRepository
        : IRepository
    {
        string[] DefaultColumnNames { get; }

        TreeNode GetDocument(
            Guid nodeGuid,
            string pageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false,
            string cultureName = null,
            string siteName = null);

        string GetDocumentClassName(
            int nodeId,
            out string siteName);

        /// <summary>
        /// Looks up documents by a list of Node GUIDs.
        /// NOTE: If column names are not specified,
        /// the method will limit the list of column names to a default set.
        /// </summary>
        /// <param name="nodeGuids"></param>
        /// <param name="pageTypeName"></param>
        /// <param name="ignorePublishedState"></param>
        /// <param name="columnNames"></param>
        /// <param name="includeAllCoupledColumns"></param>
        /// <param name="cultureName"></param>
        /// <param name="siteName"></param>
        /// <returns>
        /// List of <see cref="TreeNode"/> objects, if Node GUIDs were specified.
        /// Null, otherwise.
        /// </returns>
        IEnumerable<TreeNode> GetDocuments(
            IEnumerable<Guid> nodeGuids,
            string pageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false,
            string cultureName = null,
            string siteName = null);

        /// <summary>
        /// Looks up documents
        /// NOTE: If column names are not specified,
        /// the method will limit the list of column names to a default set.
        /// </summary>
        /// <param name="pageTypeName"></param>
        /// <param name="ignorePublishedState"></param>
        /// <param name="columnNames"></param>
        /// <param name="includeAllCoupledColumns"></param>
        /// <param name="cultureName"></param>
        /// <param name="siteName"></param>
        /// <param name="nodeGuids"></param>
        /// <param name="path"></param>
        /// <param name="whereCondition"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        IEnumerable<TreeNode> GetDocuments(
            string pageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false,
            string cultureName = null,
            string siteName = null,
            IEnumerable<Guid> nodeGuids = null,
            string path = null,
            WhereCondition whereCondition = null,
            int? top = null,
            OrderDirection? orderDirection = null,
            string[] orderByColumns = null);

        IEnumerable<TreeNode> GetDocuments(
            string where,
            string pageTypeName,
            IEnumerable<string> cultureNames = null,
            string path = "/%");

        TResult GetChildDocument<TResult>(
            TreeNode page,
            string nodeAlias,
            string childPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new();

        IEnumerable<TResult> GetChildDocuments<TResult>(
            TreeNode page,
            string childPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new();

        IEnumerable<TResult> GetChildDocumentsByPath<TResult>(
            TreeNode page,
            string childPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new();

        IEnumerable<TResult> GetRelatedDocuments<TResult>(
            TreeNode page,
            string relatedFieldName,
            out IList<RelationshipInfo> relationships,
            string relatedPageTypeName = null,
            bool ignorePublishedState = false,
            IEnumerable<string> columnNames = null,
            bool includeAllCoupledColumns = false)
            where TResult : TreeNode, new();

        bool InitilizeDocument<TResult>(
            string className,
            UserInfo user,
            out TResult page)
            where TResult : TreeNode, new();

        bool CreateDocument(
            TreeNode document,
            TreeNode parent);

        bool CreateNewVersion(
            TreeNode document,
            UserInfo user);

        bool InsertCultureVersion(
            TreeNode document,
            string culture);

        bool DeleteAllCultures(
            TreeNode document);

        bool UpdateDocument(
            TreeNode document);

        bool IsWorkflowEnabled(
            TreeNode document,
            UserInfo user,
            out bool isWorkflowEnabled);

        bool ArchiveDocument(
            TreeNode document);

        bool PublishDocument(
            TreeNode document);
    }
}
