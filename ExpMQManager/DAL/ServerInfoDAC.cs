using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace ExpMQManager.DAL
{
    public class ServerInfoDAC : BaseDB
    {
        private static ServerInfoDAC instance;

        
        private ServerInfoDAC()
        { 
            string sql = @"select TOP 1 ServerIP,ServerPort, SenderEmail from ServerInfo where ServerName = 'Email' and GETDATE() between StartDate and EndDate";
            DataTable dbServerInfo = GetSqlDataTable(sql);
            if (dbServerInfo != null)
            {
                try
                {
                    ServerIP = dbServerInfo.Rows[0]["ServerIP"].ToString();
                    ServerPort = dbServerInfo.Rows[0]["ServerPort"].ToString();
                    SenderEmail = dbServerInfo.Rows[0]["SenderEmail"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string SenderEmail { get; set; }
        
        public static ServerInfoDAC Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerInfoDAC();
                }

                return instance;
            }
        }
    }
}
