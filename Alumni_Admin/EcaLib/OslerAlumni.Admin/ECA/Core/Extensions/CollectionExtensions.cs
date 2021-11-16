using System.Data;
using System.Collections.Generic;
using System.ComponentModel;

namespace OslerAlumni.Admin.ECA.Core.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Converts the list to a data set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericList">The generic list.</param>
        /// <returns></returns>
        public static DataSet ConvertListToDataSet<T>(this IList<T> genericList)
        {
            //create DataTable Structure
            var dataTable = CreateTable<T>();
            var entType = typeof(T);
            var properties = TypeDescriptor.GetProperties(entType);

            //get the list item and add into the list
            foreach (var item in genericList)
            {
                var row = dataTable.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                dataTable.Rows.Add(row);
            }

            var ds = new DataSet();
            ds.Tables.Add(dataTable);

            return ds;
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static DataTable CreateTable<T>()
        {
            //T –> ClassName
            var entType = typeof(T);
            //set the datatable name as class name
            var dataTable = new DataTable(entType.Name);
            //get the property list
            var properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }

            return dataTable;
        }
    }
}