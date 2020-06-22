<script runat = server >
readonly static string m_sDataSource = ";data source=" + Settings.GetSetting("DataSource") + ";";
readonly static string m_sSecurityString = "User id=" + Settings.GetSetting("DBUser") + ";Password=" + Settings.GetSetting("DBpwd") + ";Integrated Security=false;";
readonly static string m_emailAlertTo = Settings.GetSetting("AlertEmail");
readonly static string m_sCompanyName = Settings.GetSetting("DBname");	//site identifer, used for cache, sql db name etc, highest priority
</script>