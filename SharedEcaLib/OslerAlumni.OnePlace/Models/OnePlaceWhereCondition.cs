using System;
using System.Collections.Generic;
using System.Linq;
using ECA.Core.Extensions;

namespace OslerAlumni.OnePlace.Models
{
    public class OnePlaceWhereCondition
        : IOnePlaceWhereCondition
    {
        #region "Constants"

        public const string AndOperator = " AND ";
        public const string OrOperator = " OR ";

        public static readonly Dictionary<string, string> EscapeSequences =
            new Dictionary<string, string>
            {
                { "\\", "\\\\" },
                { "\"", "\\\"" },
                { "'", "\\'" },
            };

        public static readonly Dictionary<string, string> EscapeLikeSequences =
            new Dictionary<string, string>
            {
                { "_", "\\_" },
                { "%", "\\%" }
            };

        #endregion

        #region "Private fields"

        private string _condition = string.Empty;

        #endregion

        #region "Methods"

        public IOnePlaceWhereCondition And(
            string condition = null,
            bool forceOperator = true)
        {
            if (!_condition.EndsWith(AndOperator)
                && (forceOperator || !_condition.EndsWith(OrOperator)))
            {
                _condition = _condition.TrimEnd(OrOperator) + AndOperator;
            }

            _condition += condition;

            return this;
        }

        public IOnePlaceWhereCondition And(
            IOnePlaceWhereCondition where)
        {
            return And(where?.ToString());
        }

        public IOnePlaceWhereCondition Or(
            string condition = null,
            bool forceOperator = true)
        {
            if (!_condition.EndsWith(OrOperator)
                && (forceOperator || !_condition.EndsWith(AndOperator)))
            {
                _condition = _condition.TrimEnd(AndOperator) + OrOperator;
            }

            _condition += condition;

            return this;
        }

        public IOnePlaceWhereCondition Or(
            IOnePlaceWhereCondition where)
        {
            return Or(where?.ToString());
        }

        public IOnePlaceWhereCondition WhereEmpty(
            string columnName)
        {
            return And(columnName + " = ''", false);
        }

        public IOnePlaceWhereCondition WhereNotEmpty(
            string columnName)
        {
            return And(columnName + " <> ''", false);
        }

        public IOnePlaceWhereCondition WhereEquals<T>(
            string columnName,
            T value,
            bool trimValue = true)
        {
            return And(
                $"{columnName} = {GetValueString(value, trimValue)}",
                false);
        }

        public IOnePlaceWhereCondition WhereLike(
            string columnName,
            string value,
            bool trimValue = false)
        {
            return And(
                $"{columnName} LIKE {GetLikeValueString(value)}",
                false);
        }

        public IOnePlaceWhereCondition WhereIn<T>(
            string columnName,
            IList<T> values,
            bool trimValues = true)
        {
            if ((values == null) || (values.Count < 1))
            {
                return this;
            }

            if (values.Count == 1)
            {
                return WhereEquals(
                    columnName,
                    values.First(),
                    trimValues);
            }

            var strValues = values
                .Select(
                    value => GetValueString(value, trimValues))
                .ToList();

            return And(
                $"{columnName} IN ({string.Join(", ", strValues)})",
                false);
        }

        public IOnePlaceWhereCondition WhereNotIn<T>(
            string columnName,
            IList<T> values,
            bool trimValues = true)
        {
            if ((values == null) || (values.Count < 1))
            {
                return this;
            }

            var strValues = values
                .Select(
                    value => GetValueString(value, trimValues))
                .ToList();

            return And(
                $"{columnName} NOT IN ({string.Join(", ", strValues)})",
                false);
        }

        public IOnePlaceWhereCondition WhereGreaterOrEqualThan<T>(
            string columnName,
            T value)
        {
            return And(
                $"{columnName} >= {GetValueString(value)}",
                false);
        }

        public IOnePlaceWhereCondition WhereLessOrEqualThan<T>(
            string columnName,
            T value)
        {
            return And(
                $"{columnName} <= {GetValueString(value)}",
                false);
        }

        public override string ToString()
        {
            var condition = (_condition ?? string.Empty)
                .TrimStart(AndOperator)
                .TrimEnd(AndOperator)
                .TrimStart(OrOperator)
                .TrimEnd(OrOperator);

            if (string.IsNullOrWhiteSpace(condition)
                || !condition.Contains(OrOperator))
            {
                return condition ?? string.Empty;
            }

            return "(" + condition + ")";
        }

        #endregion

        #region "Helper methods"

        protected string GetLikeValueString(
            string value,
            bool trimString = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "LIKE operator does not support NULL or empty value");
            }

            var strValue = value
                .Replace(EscapeSequences)
                .Replace(EscapeLikeSequences);

            return $"'%{(trimString ? strValue.Trim() : strValue)}%'";
        }

        protected string GetValueString<T>(
            T value,
            bool trimString = true)
        {
            if (value == null)
            {
                return "NULL";
            }

            var valueType = value.GetType();

            if (valueType == typeof(string))
            {
                var strValue = ((string)Convert.ChangeType(value, typeof(string)) ?? string.Empty)
                    .Replace(EscapeSequences);

                return $"'{(trimString ? strValue.Trim() : strValue)}'";
            }

            if (valueType == typeof(DateTime))
            {
                // Converting to Universal Time is important, since OnePlace might be in a different time zone
                var dtValue = ((DateTime)Convert.ChangeType(value, typeof(DateTime))).ToUniversalTime();

                return $"{dtValue.ToString("o", System.Globalization.CultureInfo.InvariantCulture)}";
            }

            return value.ToString();
        }

        #endregion
    }
}
