using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DBhelper
/// </summary>
public class DBhelper
{
    
    private SqlConnection DBhelperConnection;
    
   private SqlConnectionStringBuilder connbuilder = new SqlConnectionStringBuilder();
    

    public DBhelper()
    {
        connbuilder.DataSource = Settings.GetSetting("DataSource");
        connbuilder.UserID = Settings.GetSetting("DBUser");
        connbuilder.Password = Settings.GetSetting("DBpwd");
        connbuilder.InitialCatalog = Settings.GetSetting("DBname");
        this.DBhelperConnection = new SqlConnection(connbuilder.ConnectionString);

    }
    public DBhelper(SqlConnection _DBhelperConnection)
    {
        this.DBhelperConnection = _DBhelperConnection;
    }


    HttpResponse Response;
    //构造时传入Response可在网页显示DEBUG；
    public DBhelper(HttpResponse _Response)
    {
        this.Response = _Response;
    }
    //构造时不传入Response，将会抛出异常，但无法在网页显示DEBUG信息；

    public SqlCommand GetSqlStringCommond(string sqlQuery)
    {
        SqlCommand sqlCommand = DBhelperConnection.CreateCommand();
        sqlCommand.CommandText = sqlQuery;
        sqlCommand.CommandType = CommandType.Text;
        return sqlCommand;
    }
    public void AddInParameter(SqlCommand cmd, string parameterName, DbType dbType, object value)
    {
        SqlParameter dbParameter = cmd.CreateParameter();
        dbParameter.DbType = dbType;
        dbParameter.ParameterName = parameterName;
        dbParameter.Value = value;
        dbParameter.Direction = ParameterDirection.Input;
        cmd.Parameters.Add(dbParameter);
    }
    public SqlCommand AddInParameter(string sqlQuery, string parameterName, object value)
    {
        SqlCommand sqlCommand = DBhelperConnection.CreateCommand();
        sqlCommand.CommandText = sqlQuery;
        sqlCommand.CommandType = CommandType.Text;
        SqlParameter dbParameter = sqlCommand.CreateParameter();
        dbParameter.ParameterName = parameterName;
        dbParameter.Value = value;
        dbParameter.Direction = ParameterDirection.Input;
        sqlCommand.Parameters.Add(dbParameter);
        return sqlCommand;
    }
    public void AddInParameter(SqlCommand cmd, string parameterName,  object value)
    {
        SqlParameter dbParameter = cmd.CreateParameter();
        dbParameter.ParameterName = parameterName;
        dbParameter.Value = value;
        dbParameter.Direction = ParameterDirection.Input;
        cmd.Parameters.Add(dbParameter);
    }
    public void AddParameterCollection(SqlCommand cmd, params SqlParameter[] pars)
    {
        foreach (SqlParameter par in pars)
        {
            cmd.Parameters.Add(par);
        }
    }




    /// <summary>
    /// 返回DataTable...【Sql语句访问】
    /// </summary>
    /// <param name="cmd">sqlcommand</param>
    /// <returns></returns>
    public DataTable ExecuteDataTable(SqlCommand cmd)
    {
     
        DataTable dt = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        {
            try
            {

                sda.Fill(dt);
            }
            catch (Exception e)
            {
                ShowExp(cmd.CommandText, e);
            }

            return dt;
        }
        
    }

    public DataTable ExecuteDataTable(string sql, params SqlParameter[] pars)
    {
        DataTable dt = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(sql, DBhelperConnection))
        {
            try
            {
                if (pars != null && pars.Length > 0)
                {
                    sda.SelectCommand.Parameters.AddRange(pars);
                }

                sda.Fill(dt);
            }
            catch (Exception e)
            {
                ShowExp(sql, e);
            }

            return dt;
        }
    }

    public DataTable ExecutePageTable(string sql,int pageSize,int currentPage, params SqlParameter[] pars)
    {
        DataTable dt = new DataTable();
        using (SqlDataAdapter sda = new SqlDataAdapter(sql, DBhelperConnection))
        {
            try
            {
                if (pars != null && pars.Length > 0)
                {
                    sda.SelectCommand.Parameters.AddRange(pars);
                }

                sda.Fill( (currentPage - 1) *pageSize, pageSize, dt);
            }
            catch (Exception e)
            {
                ShowExp(sql, e);
            }

            return dt;
        }
    }
    /// <summary>
    /// 查询第一行一列值...【Sql语句访问】
    /// </summary>
    /// <param name="sql">sqk语句</param>
    /// <param name="pars">参数列表。选填！</param>
    /// <returns></returns>
    public object ExecuteScalar(string sql, params SqlParameter[] pars)
    {
        object result = null;
        using (SqlCommand cmd = new SqlCommand(sql, DBhelperConnection))
        {
            try
            {
                if (pars != null && pars.Length > 0)
                {
                    cmd.Parameters.AddRange(pars);
                }
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                result = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                ShowExp(sql, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return result;
        }

    }

    public object ExecuteScalar(SqlCommand cmd)
    {
        object result = null;
            try
            {
              
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                result = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                ShowExp(cmd.CommandText, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return result;
        

    }
    /// <summary>
    /// 在线查询，返回SqlDataReader，存储过程
    /// </summary>
    /// <param name="procname">存储过程名称</param>s
    /// <param name="pars">参数列表。选填！<</param>
    /// <returns></returns>
    public SqlDataReader ExecuteReaderProc(string procname, params SqlParameter[] pars)
    {
        SqlDataReader sdr = null;
        using (SqlCommand cmd = new SqlCommand(procname, DBhelperConnection))
        {
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (pars != null && pars.Length > 0)
                {
                    cmd.Parameters.AddRange(pars);
                }
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                ShowExp(procname, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }

            return sdr;

        }
    }
    /// <summary>
    /// 在线查询，返回SqlDataReader
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="pars"></param>
    /// <returns></returns>
    public SqlDataReader ExecuteReader(string sql, params SqlParameter[] pars)
    {

        SqlDataReader sdr = null;
        using (SqlCommand cmd = new SqlCommand(sql, DBhelperConnection))
        {
            try
            {
                if (pars != null && pars.Length > 0)
                {
                    cmd.Parameters.AddRange(pars);
                }
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception e)
            {
                ShowExp(sql, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return sdr;
        }
    }
    /// <summary>
    /// 增删改方法
    /// </summary>
    /// <param name="sql"></param>
    public int ExecuteNonQuery(string sql, params SqlParameter[] pars)
    {
        int mark = 0;
        using (SqlCommand cmd = new SqlCommand(sql, DBhelperConnection))
        {
            try
            {
                if (pars != null && pars.Length > 0)
                {
                    cmd.Parameters.AddRange(pars);
                }
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                mark = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                ShowExp(sql, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return mark;
        }



    }
    /// <summary>
    /// 增删改方法
    /// </summary>
    /// <param name="sql"></param>
    public int ExecuteNonQuery(SqlCommand cmd)
    {
        int mark = 0;
        using (cmd)
        {
            try
            {
              
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                mark = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                ShowExp(cmd.CommandText, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return mark;
        }



    }
    /// <summary>
    /// 增删改方法，存储过程
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="pars"></param>
    /// <returns></returns>
    public int ExecuteNonQueryProc(string sql, params SqlParameter[] pars)
    {

        int mark = 0;
        using (SqlCommand cmd = new SqlCommand(sql, DBhelperConnection))
        {

            try
            {
                cmd.CommandTimeout = 60;
                cmd.CommandType = CommandType.StoredProcedure;
                if (pars != null && pars.Length > 0)
                {
                    cmd.Parameters.AddRange(pars);
                }
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                mark = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ShowExp(sql, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return mark;



        }

    }
    //执行insert 然后获得最新添加的id
    public int InsertAndGetId(string sql, params SqlParameter[] pars)
    {

        string getIdentity = sql + ";select @IdentityId=@@IDENTITY";
        //int result = 0;
        SqlParameter IdentityPara = new SqlParameter("@IdentityId", SqlDbType.Int);
        IdentityPara.Direction = ParameterDirection.Output;
        using (SqlCommand cmd = new SqlCommand(getIdentity, DBhelperConnection))
        {
            try
            {
                cmd.Parameters.Add(IdentityPara);
                if (pars != null && pars.Length > 0)
                {
                    cmd.Parameters.AddRange(pars);
                }
                if (DBhelperConnection.State != ConnectionState.Open)
                {
                    DBhelperConnection.Open();
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                ShowExp(sql, e);
            }
            finally
            {
                if (DBhelperConnection.State != ConnectionState.Closed)
                    DBhelperConnection.Close();
            }
            return (int)cmd.Parameters["@IdentityId"].Value;
        }

    }
    //查询数据库是否存在某个表
    public bool IsExistTable(string tableName)
    {
        string sc = "if objectproperty(object_id(@tableName), 'IsUserTable') = 1 select 1 else select 0";
        SqlParameter[] pars = new SqlParameter[]{
                    new SqlParameter("@tableName",tableName)
                };
        bool result;
        int result_int = (int)ExecuteScalar(sc, pars);

        if (result_int == 1)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
    //查询数据库某个表中是否存在某个字段
    public bool IsExistColumnInTable(string columnName, string tableName)
    {
        string sc = "if exists(select * from syscolumns where id=object_id(@tableName) and name=@columnName) select 1 else select 0";
        SqlParameter[] pars = new SqlParameter[]{
                    new SqlParameter("@tableName",tableName),
                    new SqlParameter("@columnName",columnName)
                };
        bool result;
        int result_int = (int)ExecuteScalar(sc, pars);

        if (result_int == 1)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
    private void ShowExp(string query, Exception e)
    {
        if (Response != null)
        {
            Response.Write("Execute SQL Query Error.<br>\r\nQuery = ");
            Response.Write(query);
            Response.Write("<br>\r\n Error: ");
            Response.Write(e);
            Response.Write("<br>\r\n");
        }
        else
        {
            throw new Exception("Execute SQL Query Error.\n Query=" + query + "\n" + e.Message);
        }


    }
}