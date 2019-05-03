using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CLRFunction
{
    public class ConvertDataTableToGenericList
    {
        public List<T> ConvertDataTable<T>(DataTable dt,bool allColumnMustBeCastAsString=true)
        {
            return (from DataRow row in dt.Rows select GetItem<T>(row, allColumnMustBeCastAsString)).ToList();
        }

        private T GetItem<T>(DataRow dr,bool allColumnMustBeCastAsString)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? "" : allColumnMustBeCastAsString ? dr[column.ColumnName].ToString() : dr[column.ColumnName], null);
                }
            }
            return obj;
        }

    }
}
