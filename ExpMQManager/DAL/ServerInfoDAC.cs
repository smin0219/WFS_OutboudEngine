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
            string sql = @"select ServerIP, ServerPort, SenderEmail, ServerID, ServerPassword from ServerInfo where ServerName = 'ePicEmail'";
            DataTable dbServerInfo = GetSqlDataTable(sql);
            if (dbServerInfo != null)
            {
                try
                {
                    ServerIP = dbServerInfo.Rows[0]["ServerIP"].ToString();
                    ServerPort = dbServerInfo.Rows[0]["ServerPort"].ToString();
                    SenderEmail = dbServerInfo.Rows[0]["SenderEmail"].ToString();
                    ServerID = dbServerInfo.Rows[0]["ServerID"].ToString();
                    ServerPassword = dbServerInfo.Rows[0]["ServerPassword"].ToString();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string SenderEmail { get; set; }
        public string ServerID { get; set; }
        public string ServerPassword { get; set; }

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
