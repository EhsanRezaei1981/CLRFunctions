using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CLRFunction;
using Microsoft.SqlServer.Server;
public class ExecutingCommand
{
    public class ResultModel
    {
        public int XErrorCode { get; set; }
        public string XErrorMessage{ get; set; }
        public long XRowCount { get; set; }
        public string XScalarValue { get; set; }
    }
    #region GetRowCount
    [SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read, FillRowMethodName = "FillRow")]
    public static IEnumerable GetRowCount(string tableName,string whereCaluse)
    {
        var result = new ResultModel();
        try
        {
            using (var conn = new SqlConnection("context connection=true"))
            {
                var cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText =
                        "Select Count(1) As xRowCount From " + tableName + (
                            string.IsNullOrWhiteSpace(whereCaluse)
                                ? ""
                                : " Where "+ whereCaluse)
                };
                conn.Open();
                var rdr = cmd.ExecuteReader();
                if (SqlContext.Pipe != null) SqlContext.Pipe.Send(rdr);
                var dt = new DataTable();
                dt.Load(rdr);
                result.XRowCount = Convert.ToInt64(dt.Rows[0]["xRowCount"].ToString());
                conn.Close();
            }
        }
        catch (Exception exception)
        {
            result.XErrorCode = 1;
            result.XErrorMessage = exception.Message;
        }
        // ReSharper disable once SuspiciousTypeConversion.Global
        var finalResult = new List<ResultModel> { result };
        return finalResult;
    }

    public static void FillRow(Object obj, out int xErrorCode , out string xErrorMessage, out long xRowCount)
    {
        var eventLogEntry = (ResultModel)obj;
        xErrorCode = eventLogEntry.XErrorCode;
        xErrorMessage = eventLogEntry.XErrorMessage;
        xRowCount = eventLogEntry.XRowCount;
    }
    #endregion

    #region GetScalarValue
    [SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read, FillRowMethodName = "FillScalarValue")]
    public static IEnumerable GetScalarValue(string sqlQuery)
    {
        var result = new ResultModel();
        try
        {
            using (var conn = new SqlConnection("context connection=true"))
            {
                var cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = sqlQuery
                };
                conn.Open();
                var rdr = cmd.ExecuteReader();
                if (SqlContext.Pipe != null) SqlContext.Pipe.Send(rdr);
                var dt = new DataTable();
                dt.Load(rdr);
                result.XScalarValue= dt.Rows[0]["xScalarValue"].ToString();
                conn.Close();
            }
        }
        catch (Exception exception)
        {
            result.XErrorCode = 1;
            result.XErrorMessage = exception.Message;
        }
        // ReSharper disable once SuspiciousTypeConversion.Global
        var finalResult = new List<ResultModel> { result };
        return finalResult;
    }

    public static void FillScalarValue(Object obj, out int xErrorCode, out string xErrorMessage, out string xScalarValue)
    {
        var eventLogEntry = (ResultModel)obj;
        xErrorCode = eventLogEntry.XErrorCode;
        xErrorMessage = eventLogEntry.XErrorMessage;
        xScalarValue = eventLogEntry.XScalarValue;
    }
    #endregion
    #region Retuen Rows
    public class ResultModelRows
    {
        public int XErrorCode { get; set; }
        public string XErrorMessage { get; set; }
        public string COLUMN1 { get; set; }
        public string COLUMN2 { get; set; }
        public string COLUMN3 { get; set; }
        public string COLUMN4 { get; set; }
        public string COLUMN5 { get; set; }
        public string COLUMN6 { get; set; }
        public string COLUMN7 { get; set; }
    }
    [SqlFunction(DataAccess = DataAccessKind.Read, SystemDataAccess = SystemDataAccessKind.Read, FillRowMethodName = "FillRows")]
    public static IEnumerable GetRows(string tableName, string whereCaluse,string columnsName)
    {
        var result = new List<ResultModelRows>();
        try
        {
            using (var conn = new SqlConnection("context connection=true"))
            {
                var cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText =
                        "Select " + columnsName.ToUpper() + " From " + tableName + (
                            string.IsNullOrWhiteSpace(whereCaluse)
                                ? ""
                                : " Where " + whereCaluse)
                };
                conn.Open();
                var rdr = cmd.ExecuteReader();
                if (SqlContext.Pipe != null) SqlContext.Pipe.Send(rdr);
                var dt = new DataTable();
                dt.Load(rdr);
                result = new ConvertDataTableToGenericList().ConvertDataTable<ResultModelRows>(dt);
                conn.Close();
            }
        }
        catch (Exception exception)
        {
            result.Add(new ResultModelRows()
            {
                XErrorCode = 1,
                XErrorMessage = exception.Message
            });
        }
        // ReSharper disable once SuspiciousTypeConversion.Global
        return result;
    }

    public static void FillRows(Object obj, out int xErrorCode, out string xErrorMessage, out string xColumn1,
        out string xColumn2, out string xColumn3, out string xColumn4, out string xColumn5, out string xColumn6,
        out string xColumn7
        )
    {
        var eventLogEntry = (ResultModelRows)obj;
        xErrorCode = eventLogEntry.XErrorCode;
        xErrorMessage = eventLogEntry.XErrorMessage;
        xColumn1 = eventLogEntry.COLUMN1;
        xColumn2 = eventLogEntry.COLUMN2;
        xColumn3 = eventLogEntry.COLUMN3;
        xColumn4 = eventLogEntry.COLUMN4;
        xColumn5 = eventLogEntry.COLUMN5;
        xColumn6 = eventLogEntry.COLUMN6;
        xColumn7 = eventLogEntry.COLUMN7;
    }
  
    #endregion
}
