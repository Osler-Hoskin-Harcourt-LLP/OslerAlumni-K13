using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Search.Azure;
using ECA.Core.Extensions;
using Microsoft.Azure.Search.Models;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Api.Definitions;

namespace OslerAlumni.Mvc.Api.Models
{
    public class AzureSearchFilterExpression
    {
        #region "Constants"

        protected const string AndOperator = "and";
        protected const string OrOperator = "or";

        public static readonly Type[] WrappableTypes =
        {
            typeof(string),
            typeof(Guid),
            typeof(DeliveryMethods)
        };

        #endregion

        #region "Properties"

        private string CombineOperator { get; set; } = AndOperator;

        public string ExpressionText { get; private set; } = string.Empty;

        #endregion

        #region "Methods"

        public AzureSearchFilterExpression Compare<T>(
            string fieldName,
            T value,
            FilterOperation operation)
        {
            var tempExpression = new AzureSearchFilterExpression();

            switch (operation)
            {
                case FilterOperation.Equal:
                    return Equals(fieldName, value);
                case FilterOperation.EqualOrEmpty:
                    tempExpression
                        .Equals(fieldName, value)
                        .Or()
                        .Empty(fieldName);
                    break;
                case FilterOperation.NotEqual:
                    return NotEquals(fieldName, value);
                case FilterOperation.LessThan:
                    return LessThan(fieldName, value);
                case FilterOperation.LessOrEqualThan:
                    return LessOrEqualThan(fieldName, value);
                case FilterOperation.GreaterThan:
                    return GreaterThan(fieldName, value);
                case FilterOperation.GreaterOrEqualThan:
                    return GreaterOrEqualThan(fieldName, value);
                case FilterOperation.In:
                    return In(fieldName, value);
                case FilterOperation.NotIn:
                    return NotIn(fieldName, value);
                case FilterOperation.Matches:
                    return Matches(fieldName, value);
                case FilterOperation.LessThanOrEmpty:
                    {
                        tempExpression
                            .LessThan(fieldName, value)
                            .Or()
                            .Empty(fieldName);

                        break;
                    }
                case FilterOperation.LessOrEqualThanOrEmpty:
                    {
                        tempExpression
                            .LessOrEqualThan(fieldName, value)
                            .Or()
                            .Empty(fieldName);

                        break;
                    }
                case FilterOperation.GreaterThanOrEmpty:
                    {
                        tempExpression
                            .GreaterThan(fieldName, value)
                            .Or()
                            .Empty(fieldName);

                        break;
                    }
                case FilterOperation.GreaterOrEqualThanOrEmpty:
                    {
                        tempExpression
                            .GreaterOrEqualThan(fieldName, value)
                            .Or()
                            .Empty(fieldName);

                        break;
                    }
            }

            return Combine(tempExpression.ExpressionText);
        }

        public AzureSearchFilterExpression Empty(
            string fieldName)
        {
            return Equals(fieldName, (object)null);
        }

        public AzureSearchFilterExpression Equals<T>(
            string fieldName,
            T value)
        {
            return Combine(
                GetExpression(fieldName, value, "eq"));
        }

        public AzureSearchFilterExpression NotEquals<T>(
            string fieldName,
            T value)
        {
            return Combine(
                GetExpression(fieldName, value, "ne"));
        }

        public AzureSearchFilterExpression LessThan<T>(
            string fieldName,
            T value)
        {
            return Combine(
                GetExpression(fieldName, value, "lt"));
        }

        public AzureSearchFilterExpression LessOrEqualThan<T>(
            string fieldName,
            T value)
        {
            return Combine(
                GetExpression(fieldName, value, "le"));
        }

        public AzureSearchFilterExpression GreaterThan<T>(
            string fieldName,
            T value)
        {
            return Combine(
                GetExpression(fieldName, value, "gt"));
        }

        public AzureSearchFilterExpression GreaterOrEqualThan<T>(
            string fieldName,
            T value)
        {
            return Combine(
                GetExpression(fieldName, value, "ge"));
        }
        /// <summary>
        /// https://docs.microsoft.com/en-us/azure/search/search-filters#text-filter-fundamentals
        /// https://docs.microsoft.com/en-ca/azure/search/query-odata-filter-orderby-syntax#filter-operators
        /// https://docs.microsoft.com/en-ca/azure/search/query-odata-filter-orderby-syntax#filter-examples
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldNames"></param>
        /// <param name="value"></param>
        /// <param name="queryType"></param>
        /// <param name="searchMode"></param>
        /// <returns></returns>
        public AzureSearchFilterExpression Matches<T>(
            List<string> fieldNames,
            T value,
            QueryType queryType = QueryType.Full,
            SearchMode searchMode = SearchMode.Any)
        {
            var fields = string.Join(",", fieldNames.Select(NamingHelper.GetValidFieldName));

            var expression =
                $@"search.ismatch({GetValueString(value)},'{fields}','{queryType.ToString().ToLower()}','{searchMode.ToString().ToLower()}')";

            return Combine(expression);
        }

        public AzureSearchFilterExpression Matches<T>(
            string fieldName,
            T value,
            QueryType queryType = QueryType.Full,
            SearchMode searchMode = SearchMode.Any)
        {
            return Matches(
                new List<string> { fieldName },
                value,
                queryType,
                searchMode);
        }

        public AzureSearchFilterExpression In<T>(
            string fieldName,
            T value)
        {
            var values = value as IList<dynamic>;

            if (values != null)
            {
                var tempExpression = new AzureSearchFilterExpression()
                    .Or();

                foreach (var listValue in values)
                {
                    tempExpression.Equals(fieldName, listValue);
                }

                Combine(
                    tempExpression.ExpressionText);
            }

            return this;
        }

        public AzureSearchFilterExpression NotIn<T>(
            string fieldName,
            T value)
        {
            var values = value as Array;

            if (values != null)
            {
                var tempExpression = new AzureSearchFilterExpression()
                    .And();

                foreach (var listValue in values)
                {
                    tempExpression.NotEquals(fieldName, listValue);
                }

                Combine(
                    tempExpression.ExpressionText);
            }

            return this;
        }

        public AzureSearchFilterExpression And()
        {
            CombineOperator = AndOperator;

            return this;
        }

        public AzureSearchFilterExpression And(
            string expression)
        {
            return Combine(
                expression,
                AndOperator);
        }

        public AzureSearchFilterExpression And(
            AzureSearchFilterExpression expressionFilter)
        {
            return Combine(
                expressionFilter.ExpressionText,
                AndOperator);
        }

        public AzureSearchFilterExpression Or()
        {
            CombineOperator = OrOperator;

            return this;
        }

        public AzureSearchFilterExpression Or(
            string expression)
        {
            return Combine(
                expression,
                OrOperator);
        }

        public AzureSearchFilterExpression Or(
            AzureSearchFilterExpression expressionFilter)
        {
            return Combine(
                expressionFilter.ExpressionText,
                OrOperator);
        }

        protected AzureSearchFilterExpression Combine(
            string expression,
            string combineOperator = null)
        {
            if (!string.IsNullOrWhiteSpace(expression))
            {
                expression = $"({expression})";

                if (!string.IsNullOrWhiteSpace(ExpressionText))
                {
                    expression = $" {combineOperator.ReplaceIfEmpty(CombineOperator)} {expression}";
                }

                ExpressionText += expression;
            }

            return this;
        }

        #endregion

        #region "Helper methods"

        protected string GetExpression<T>(
            string fieldName,
            T value,
            string comparisonOperator)
        {
            return $"{NamingHelper.GetValidFieldName(fieldName)} {comparisonOperator} {GetValueString(value)}";
        }

        protected string GetValueString<T>(
            T value)
        {
            if (value == null)
            {
                return "null";
            }

            var valueType = value.GetType();

            if (WrappableTypes.Any(t => t == valueType))
            {
                return $"'{value}'";
            }

            if (valueType == typeof(bool))
            {
                return value.ToString().ToLower();
            }

            if (valueType == typeof(DateTime))
            {
                var dateTime = (DateTime) Convert.ChangeType(value, typeof(DateTime));

                //This needs to be done since all kentico fields are improperly attributed as UTC in Azure,
                //So any datetime searches need to have the same in-accuracy for search to work. :)
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

                return dateTime.ToString("o", System.Globalization.CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }

        #endregion
    }
}
