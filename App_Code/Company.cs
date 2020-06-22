using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Company
/// </summary>
public static class Company
{
    public static string name = "Gpos";
    public static readonly string m_sCompanyName = Settings.GetSetting("DBname");

    //public Company()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
}