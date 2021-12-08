using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;

namespace OslerAlumni.OnePlace.Models
{
    public class OnePlaceQuery
        : IOnePlaceQuery
    {
        #region "Constants"

        public const string ColumnsMacro = "##CUSTOM_COLUMNS##";
        public const string FullColumnsMacro = "##CUSTOM_COLUMNS_FULL##";
        public const string WhereMacro = "##CUSTOM_WHERE##";
        public const string FullWhereMacro = "##CUSTOM_WHERE_FULL##";
        public const string OrderByMacro = "##CUSTOM_ORDER_BY##";
        public const string FullOrderByMacro = "##CUSTOM_ORDER_BY_FULL##";
        public const string FullLimitMacro = "##CUSTOM_LIMIT_FULL##";

        #endregion

        #region "Properties"

        public string QueryText { get; private set; } = string.Empty;

        #endregion

        public OnePlaceQuery(
            string queryText)
        {
            QueryText = queryText;
        }

        public OnePlaceQuery(
            string queryName,
            string className)
        {
            if (string.IsNullOrWhiteSpace(queryName))
            {
                return;
            }

            if (!queryName.Contains('.'))
            {
                queryName = string.Concat(className, '.', queryName);
            }

            var query = new DataQuery(queryName);

            QueryText = query.GetFullQueryText(true);
        }

        #region "Methods"

        public IOnePlaceQuery Columns(
            IList<string> columnNames)
        {
            if ((columnNames == null) || (columnNames.Count < 1))
            {
                if (QueryText.Contains(FullColumnsMacro))
                {
                    throw new ArgumentException("List of column names were not provided for the SOQL query.");
                }

                return this;
            }

            var strColumnNames = string.Join(", ", columnNames);

            QueryText = QueryText
                .Replace(
                    FullColumnsMacro,
                    strColumnNames)
                .Replace(
                    ColumnsMacro,
                    ", " + strColumnNames);

            return this;
        }

        public IOnePlaceQuery OrderBy(
            string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                QueryText = QueryText
                    .Replace(
                        FullOrderByMacro,
                        string.Empty)
                    .Replace(
                        OrderByMacro,
                        string.Empty);
            }
            else
            {
                QueryText = QueryText
                    .Replace(
                        FullOrderByMacro,
                        "ORDER BY " + orderBy)
                    .Replace(
                        OrderByMacro,
                        ", " + orderBy);
            }

            return this;
        }

        public IOnePlaceQuery TopN(
            int count)
        {
            if (count < 1)
            {
                QueryText = QueryText
                    .Replace(
                        FullLimitMacro,
                        string.Empty);
            }
            else
            {
                QueryText = QueryText
                    .Replace(
                        FullLimitMacro,
                        "LIMIT " + count);
            }

            return this;
        }

        public IOnePlaceQuery Where(
            IOnePlaceWhereCondition where)
        {
            var whereCondition = where?.ToString();

            // Handle the macros
            if (string.IsNullOrEmpty(whereCondition))
            {
                QueryText = QueryText
                    .Replace(
                        FullWhereMacro,
                        string.Empty)
                    .Replace(
                        WhereMacro,
                        string.Empty);
            }
            else
            {
                QueryText = QueryText
                    .Replace(
                        FullWhereMacro,
                        "WHERE " + whereCondition)
                    .Replace(
                        WhereMacro,
                        OnePlaceWhereCondition.AndOperator + whereCondition);
            }

            return this;
        }

        #endregion
    }
}
