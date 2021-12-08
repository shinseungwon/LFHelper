using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HelperDotNet
{
    public sealed class WebHelper
    {
        public Logger logger = null;
        public bool logToDB = false;
        public int timeout = 100000;
        public int readWriteTimeout = 300000;        

        public WebHelper()
        {

        }

        public void SetLogger(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// web request
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="method">post or get</param>
        /// <returns></returns>
        public string Request(string url, string method = "POST")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = timeout;
            request.ReadWriteTimeout = readWriteTimeout;

            string res = "";

            try
            {
                HttpWebResponse wresp = (HttpWebResponse)request.GetResponse();
                Stream wstream = wresp.GetResponseStream();
                byte[] buffer = new byte[1024];

                while (wstream.Read(buffer, 0, 1024) > 0)
                {
                    res += Encoding.Default.GetString(buffer);
                }
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    if (logToDB)
                    {
                        logger.WriteDB("request type : " + method + "/url : " + url, e.ToString());
                    }
                    else
                    {
                        logger.WriteText("request type : " + method + "/url : " + url + "/error : " + e.ToString());
                    }                    
                }
            }
            finally
            {
                if (logToDB)
                {
                    logger.WriteDB("request type : " + method + "/url : " + url, res.Substring(0, res.Length > 100 ? 100 : res.Length) + " ...");
                }
                else
                {
                    logger.WriteText("request type : " + method + "/url : " + url + "/response : " + res.Substring(0, 100) + " ...");
                }
            }

            return res;
        }
    }
}
