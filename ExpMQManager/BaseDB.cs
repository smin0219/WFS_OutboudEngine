﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using System.Configuration;

namespace ExpMQManager
{
    public class BaseDB : IDisposable
    {

        void IDisposable.Dispose() { }

        public static bool isLive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveDB"]);

        SqlConnection dbcn_ExcuteReader;

        public BaseDB()
        {
            dbcn_ExcuteReader = new SqlConnection(getConnectionString());
        }

        public DataTable GetSqlDataTable(string strsql)
        {
            DataTable table = new DataTable();
            SqlConnection dbcn = new SqlConnection(getConnectionString());
            SqlCommand cmd = new SqlCommand(strsql, dbcn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            try
            {
                dbcn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 320;
                sda.Fill(table);

                dbcn.Close();
                dbcn.Dispose();

                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbcn.Dispose();
                sda.Dispose();
                cmd.Dispose();
            }
        }

        public SqlDataReader GetSqlDataReader(string strsql)
        {
            SqlDataReader dr;
            using (SqlConnection dbcn = new SqlConnection(getConnectionString()))
            {
                dbcn.Open();
                SqlCommand command = new SqlCommand(strsql, dbcn);
                dr = command.ExecuteReader();
                dbcn.Close();
                dbcn.Dispose();
                return dr;
            }
        }

        public IDataReader ExecuteReader(string strSql)
        {
            //SqlConnection dbcn = new SqlConnection(getConnectionString());
            //dbcn.Open();
            //SqlCommand command = new SqlCommand(strSql, dbcn);
            
            //return command.ExecuteReader(CommandBehavior.Default);

            IDataReader result = null;
            try
            {
                //if (dbcn_ExcuteReader == null)
                {
                    dbcn_ExcuteReader = new SqlConnection(getConnectionString());
                    dbcn_ExcuteReader.Open();
                }

                SqlCommand command = new SqlCommand(strSql, dbcn_ExcuteReader);

                result = command.ExecuteReader(CommandBehavior.Default);

                //dbcn_ExcuteReader.Close();
                //dbcn_ExcuteReader.Dispose();
                return result;
            }
            catch (Exception e)
            {
                buildLog(0, e.Message, e.StackTrace);
            }
            return result;
            //return command.ExecuteReader();
        }
        public void disConnect_dbcn_ExcuteReader()
        {
            dbcn_ExcuteReader.Close();
            dbcn_ExcuteReader.Dispose();
        }

        public DataTable GetSqlDataTable(SqlCommand command)
        {
            DataTable table = new DataTable();
            SqlDataAdapter sda;
            using (SqlConnection dbcn = new SqlConnection(getConnectionString()))
            {
                dbcn.Open();
                command.Connection = dbcn;
                command.CommandTimeout = 320;
                sda = new SqlDataAdapter(command);
                sda.Fill(table);
                dbcn.Close();
                dbcn.Dispose();
                return table;
            }
        }

        public int ExecCommand(SqlCommand command)
        {
            int i = 0;
            using (SqlConnection dbcn = new SqlConnection(getConnectionString()))
            {
                dbcn.Open();
                command.Connection = dbcn;
                i = command.ExecuteNonQuery();
                dbcn.Close();
                dbcn.Dispose();
            }
            return i;
        }



        public int ExecCommand(string commandString)
        {
            SqlConnection dbcn = new SqlConnection(getConnectionString());
            SqlCommand cmd = new SqlCommand(commandString, dbcn);
            try
            {
                dbcn.Open();
                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbcn.Close();
                dbcn.Dispose();
                cmd.Dispose();
            }
        }

        public static string getConnectionString()
        {
            string connectionString = "";
            if (isLive)
            {
                //connectionString = "Data Source=jfk261db.casusa.com;Initial Catalog=casnet;User ID=casnet;Password=Cas2007;MultipleActiveResultSets=True";
                //connectionString = "Data Source=jfk261db.casusa.com;Initial Catalog=testcasnet;User ID=casnet;Password=Cas2007;MultipleActiveResultSets=True";
                //connectionString = "Data Source=IO3003DBT1;Initial Catalog=casnet;User ID=OutboundEngine;Password=outengine2015;MultipleActiveResultSets=True";
                connectionString = "Data Source=IO3003DBT1;Initial Catalog=casnet;User ID=OutboundEngine;Password=outengine2015;MultipleActiveResultSets=True";
            }
            else
            {
                //connectionString = "Data Source=jfk261db.casusa.com;Initial Catalog=casnet;User ID=casnet;Password=Cas2007;MultipleActiveResultSets=True";
                //connectionString = "Data Source=jfk261db.casusa.com;Initial Catalog=testcasnet;User ID=casnet;Password=Cas2007;MultipleActiveResultSets=True";
                //connectionString = "Data Source=IO3003DBT1;Initial Catalog=casnet;User ID=OutboundEngine;Password=outengine2015;MultipleActiveResultSets=True";
                connectionString = "Data Source=IO3003DBT1;Initial Catalog=testcasnet;User ID=OutboundEngine;Password=outengine2015;MultipleActiveResultSets=True";

            }

            return connectionString;

        }
        
        
        public static int buildLog(int queueId, string errorMsg, string errorDetail)
        {
            string logmsg = "";
            logmsg += "\t Queue ID: " + queueId + "\r\n";
            logmsg += "\t Error msg: " + errorMsg + "\r\n";
            logmsg += "\t Error Detail: " + errorDetail + "\r\n";
            logmsg += "===================================================================================================";
            //Logging to Logger
            try
            { int aa = Logger(logmsg); }
            catch (IOException ex)
            { return 1; }

            //Send Notification Email to Admin
            try
            { //baseMail.mailSend("EpicError@casusa.com", logmsg.Replace("\r\n","<br>"), "ExpMQManager - Error Notification"); 
            }
            catch (Exception e) { }

            return 0;
        }
        static int Logger(string log)
        {
            string nowDate = DateTime.Now.ToString("yyyyMMdd");
            string nowTime = DateTime.Now.ToString("HH':'mm':'ss");
            string fileName = "MQEngineEXP_" + nowDate + ".txt";
            string filePath = @"C:\temp\log\" + fileName;
            try
            {
                if (!File.Exists(filePath))
                {
                    StreamWriter sw = File.CreateText(filePath);
                    sw.Close();
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    log = nowDate + " " + nowTime + " " + log;
                    sw.WriteLine(log);
                    sw.Close();
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return 0;
        }
    }
}
