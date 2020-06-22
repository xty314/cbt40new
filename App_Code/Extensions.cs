using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Extensions 的摘要说明
/// 项目的拓展方法
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 匿名对象序列化，使用该方法可取的匿名对象的属性值
    /// </summary>
    /// <typeparam name="T">匿名类型</typeparam>
    /// <param name="obj">需要转化的对象</param>
    /// <param name="sample">一个匿名对象</param>
    /// <returns></returns>
    public static T objCast<T>(this object obj, T sample)
    {
        return (T)obj;
    }
    public static string Dtb2Json(this DataTable dtb)
    {
    JavaScriptSerializer jss = new JavaScriptSerializer();
        jss.MaxJsonLength = int.MaxValue;
        System.Collections.ArrayList dic = new System.Collections.ArrayList();
    foreach (DataRow dr in dtb.Rows)
    {
        System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
        foreach (DataColumn dc in dtb.Columns)
        {
            drow.Add(dc.ColumnName, dr[dc.ColumnName]);
        }
        dic.Add(drow);

        }
     
        //序列化  
        return jss.Serialize(dic);
}
    /// <summary>
    /// 拓展方法 将Datatable转化位json 例：DataTable dt= new DataTable(); dt.ToJSON()=>json对象;
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string ToJson(this DataTable dt)
    {
        StringBuilder jsonBuilder = new StringBuilder();
        jsonBuilder.Append("[");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(dt.Rows[i][j].ToString());
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");
        }
        if (dt.Rows.Count > 0)
        {
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
        }
        jsonBuilder.Append("]");
        return jsonBuilder.ToString();
    }
    /// <summary>
    /// Hashtable表转JSON
    /// </summary>
    /// <param name="data"></param>
    /// <returns>var postData = new Hashtable();postData.Add("openid", "55");postData.Add("card_id", "55");</returns>
    public static string ToJson( this Hashtable data)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (object key in data.Keys)
            {
                object value = data[key];
                
                sb.Append("\"");
                sb.Append(key);
                if (value.ToString().Substring(0, 1) != "[")
                {
                    sb.Append("\":\"");
                }
                else
                {
                    sb.Append("\":");
                }
                    
                if (!String.IsNullOrEmpty(value.ToString()) && value != DBNull.Value)
                {
                    sb.Append(value).Replace("\\", "/");
                }
                else
                {
                    sb.Append(" ");
                }
                if (value.ToString().Substring(value.ToString().Length-1, 1) != "]")
                {
                    sb.Append("\",");
                }
                else
                {
                    sb.Append(",");
                }
                   
            }
            sb = sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
        catch (Exception ex)
        {

            return "";
        }
    }

}