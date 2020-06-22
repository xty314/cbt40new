using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Settings 的摘要说明
/// </summary>
public  class Settings
{
    
  
    public static  string GetSetting(string key)
    {
        string appSettings = System.Web.HttpContext.Current.Server.MapPath("~/") + "/appSettings.json";

        using (System.IO.StreamReader file = System.IO.File.OpenText(appSettings))
        {
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o = (JObject)JToken.ReadFrom(reader);
                var value = o[key].ToString();
                return value;
            }
        }
    }
}