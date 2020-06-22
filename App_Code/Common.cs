using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Common 的摘要说明
/// 公用的static function ，内容整理取自原common.cs 
/// </summary>
public static class Common
{
    /// <summary>
    /// 获得EnumId
    /// </summary>
    /// <param name="sClass"> enum表class字段 例如：card_type</param>
    /// <param name="sValue"> enum表name字段 例如：dealer</param>
    /// <returns>返回enum id</returns>
    public static string GetEnumID(string sClass, string sValue)
    {
        DataSet dsEnum = new DataSet();
        DBhelper dbhelper = new DBhelper();
        string sc = "SELECT id FROM enum WHERE class=@class AND name=@name";
        SqlCommand cmd = dbhelper.GetSqlStringCommond(sc);
        dbhelper.AddInParameter(cmd, "@class", sClass);
        dbhelper.AddInParameter(cmd, "@name", sValue);
        DataTable enumTable = dbhelper.ExecuteDataTable(cmd);
        string sID = enumTable.Rows[0]["id"].ToString();
        return sID;
    }

   

}