using System;
using System.Collections.Generic;

namespace OslerAlumni.OnePlace.Models
{
    public interface IOnePlaceWhereCondition
    {
        IOnePlaceWhereCondition And(
            string condition = null,
            bool forceOperator = true);

        IOnePlaceWhereCondition And(
            IOnePlaceWhereCondition where);

        IOnePlaceWhereCondition Or(
            string condition = null,
            bool forceOperator = true);

        IOnePlaceWhereCondition Or(
            IOnePlaceWhereCondition where);

        IOnePlaceWhereCondition WhereEmpty(
            string columnName);

        IOnePlaceWhereCondition WhereNotEmpty(
            string columnName);

        IOnePlaceWhereCondition WhereEquals<T>(
            string columnName,
            T value,
            bool trimValue = true);

        IOnePlaceWhereCondition WhereLike(
            string columnName,
            string value,
            bool trimValue = false);

        IOnePlaceWhereCondition WhereIn<T>(
            string columnName,
            IList<T> values,
            bool trimValues = true);

        IOnePlaceWhereCondition WhereNotIn<T>(
            string columnName,
            IList<T> values,
            bool trimValues = true);

        IOnePlaceWhereCondition WhereGreaterOrEqualThan<T>(
            string columnName,
            T value);

        IOnePlaceWhereCondition WhereLessOrEqualThan<T>(
            string columnName,
            T value);
    }
}
