using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBM.WMQ;

namespace ExpMQManager
{
    public class CASMqm
    {
        static MQQueueManager queueManager1;
        static MQQueueManager queueManager2;

        
        MQMessage queueMessage;
        MQPutMessageOptions queuePutMessageOptions;
        MQGetMessageOptions queueGetMessageOptions;

        static string SendQueueName;
        static string ReceiveQueueName;
        //static string QueueManagerName;
        //static string ChannelInfo;
        string channelName;
        string transportType;
        string connectionName;
        string message;

        public CASMqm()
        {
            //Initialisation
            /*
             QueueManagerName = "QM_TEST";
             QueueName = "QM_TEST.LOCAL.ONE";
             ChannelInfo = "SVRCONN/TCP/localhost(1424)";
             */
        }

        public string ConnectMQ(string strQueueManagerName, string strChannelInfo, int queueId)
        {
            //QueueManagerName = strQueueManagerName;
            //ChannelInfo = strChannelInfo;

            char[] separator = { '/' };
            string[] ChannelParams;
            ChannelParams = strChannelInfo.Split(separator);
            channelName = ChannelParams[0];
            transportType = ChannelParams[1];
            connectionName = ChannelParams[2];
            string strReturn = "";

            try
            {
                if (queueId == 1)
                {
                    queueManager1 = new MQQueueManager(strQueueManagerName,
                       channelName, connectionName);
                }
                else
                {
                    queueManager2 = new MQQueueManager(strQueueManagerName,
                       channelName, connectionName);
                }

                strReturn = "Connected Successfully";
                
            }
            catch (MQException exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            catch (Exception exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            return strReturn;
        }

        public string DisconnectMQ(int queueId)
        {

            string strReturn = "";

            MQQueueManager queueMgr;

            if (queueId == 1)
                queueMgr = queueManager1;
            else
                queueMgr = queueManager2;

            try
            {
                queueMgr.Disconnect();
                strReturn = "Disconnected Successfully";
            }
            catch (MQException exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            catch (Exception exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            return strReturn;
        }

        public string WriteLocalQMsg(string strInputMsg, string strQueueName, int queueId, string ChannelInfo, string queueMgrName)
        {
            string strReturn = "";
            SendQueueName = strQueueName;

            MQQueueManager queueMgr;
            MQQueue queue;

            if (queueId == 1)
                queueMgr = queueManager1;
            else
                queueMgr = queueManager2;

            try
            {
                if (queueMgr == null || queueMgr.IsConnected == false)
                {
                    ConnectMQ(queueMgrName, ChannelInfo, queueId);
                    //Reassign After Connection
                    if (queueId == 1)
                        queueMgr = queueManager1;
                    else
                        queueMgr = queueManager2;
                }

                queue = queueMgr.AccessQueue(SendQueueName,
                  MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);
                message = strInputMsg;
                queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;
                queueMessage.CharacterSet = MQC.CODESET_850;
                queueMessage.Encoding = MQC.MQENC_NATIVE;
                queueMessage.WriteString(message);
                queuePutMessageOptions = new MQPutMessageOptions();
                
                queue.Put(queueMessage, queuePutMessageOptions);
                strReturn = "Message sent to the queue successfully";
            }
            catch (MQException MQexp)
            {
                strReturn = "Exception: " + MQexp.Message;
            }
            catch (Exception exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            return strReturn;
        }

        public string ReadLocalQMsg(string strQueueName, int queueId, string ChannelInfo, string queueMgrName)
        {
            string strReturn = "";
            ReceiveQueueName = strQueueName;

            MQQueueManager queueMgr;
            MQQueue queue;

            if (queueId == 1)
                queueMgr = queueManager1;
            else
                queueMgr = queueManager2;

            try
            {
                if (queueMgr == null || queueMgr.IsConnected == false)
                {

                    ConnectMQ(queueMgrName, ChannelInfo, queueId);

                }

                queue = queueMgr.AccessQueue(ReceiveQueueName,
                MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);
                queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;
                queueGetMessageOptions = new MQGetMessageOptions();
                queue.Get(queueMessage, queueGetMessageOptions);
                if (queue.IsOpen == false)
                {


                }
                strReturn = queueMessage.ReadString(queueMessage.MessageLength);
                queue.Close();
            }
            catch (MQException MQexp)
            {
                strReturn = "Exception: " + MQexp.Message;
            }
            catch (Exception exp)
            {
                strReturn = "Exception: " + exp.Message;
            }
            return strReturn;
        }

    }
}
