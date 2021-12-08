//using System;
//using System.Data;
//using System.IO;
//using System.Net;
//using System.Security.Cryptography;

//namespace HelperDotNet
//{
//    /// <summary>
//    /// Class helps connect ftp server and upload and download files
//    /// </summary>
//    public sealed class FileHelper
//    {
//        public string rootpath;        
//        public string userid;
//        public string password;

//        public DbHelper dbHelper;
//        public Logger logger;

//        public bool logToDB = false;

//        /// <summary>
//        /// Constructor ( IV = Initial Vector that user should define for encrypting )
//        /// </summary>
//        /// <param name="rootpath">Ftp path</param>
//        /// <param name="userid">Ftp authenication id</param>
//        /// <param name="password">Ftp authenication password</param>
//        /// <param name="iv">Initial vector</param>
//        /// <param name="dbHelper">DbHelper instance</param>
//        /// <param name="logger">Logger instance</param>
//        public FileHelper(string rootpath, string userid, string password, DbHelper dbHelper, Logger logger)
//        {
//            this.dbHelper = dbHelper;
//            this.logger = logger;
//            this.rootpath = rootpath;
//            this.userid = userid;
//            this.password = password;
//            //this.iv = Encoding.Default.GetBytes(iv);
//        }

//        /// <summary>
//        /// Upload to ftp
//        /// </summary>
//        /// <param name="localPath">full path</param>
//        /// <param name="ftpPath">ftp path</param>
//        public void Upload(string localPath, string ftpPath)
//        {
//            try
//            {
//                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(rootpath + "/" + ftpPath);
//                req.Method = WebRequestMethods.Ftp.UploadFile;
//                req.Credentials = new NetworkCredential(userid, password);
//                req.KeepAlive = false;
//                req.UseBinary = true;

//                byte[] filebytes = File.ReadAllBytes(localPath);
//                using Stream stream = req.GetRequestStream();
//                stream.Write(filebytes, 0, filebytes.Length);                
//            }
//            catch (Exception e)
//            {
//                if (logToDB)
//                {
//                    logger.WriteDB("Ftp Upload Error", "Path : " + rootpath + "/" + ftpPath + " /ErrorMsg : " + e.ToString());
//                }
//                else
//                {
//                    logger.WriteText("Ftp Upload Error - Path : " + rootpath + "/" + ftpPath + " /ErrorMsg : " + e.ToString());
//                }
//            }
//            finally
//            {
//                if (logToDB)
//                {
//                    logger.WriteDB("Ftp Upload", "Path : " + rootpath + "/" + ftpPath);
//                }
//                else
//                {
//                    logger.WriteText("Ftp Upload - Path : " + rootpath + "/" + ftpPath);
//                }
//            }
//        }

//        /// <summary>
//        /// Download to ftp
//        /// </summary>
//        /// <param name="localPath">full path</param>
//        /// <param name="ftpPath">full path</param>
//        public void Download(string localPath, string ftpPath)
//        {
//            try
//            {
//                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(rootpath + "/" + ftpPath);
//                req.Method = WebRequestMethods.Ftp.DownloadFile;
//                req.Credentials = new NetworkCredential(userid, password);
//                req.KeepAlive = false;
//                req.UseBinary = true;

//                using FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
//                using Stream stream = resp.GetResponseStream();
//                using MemoryStream ms = new MemoryStream();
                
//                byte[] buf;
//                int count = 0;

//                do
//                {
//                    buf = new byte[1024];
//                    count = stream.Read(buf, 0, 1024);
//                    ms.Write(buf, 0, count);
//                } while (stream.CanRead && count > 0);

//                byte[] data = ms.ToArray();

//                File.WriteAllBytes(localPath, data);
//            }
//            catch (Exception e)
//            {
//                if (logToDB)
//                {
//                    logger.WriteDB("Ftp Download Error", "Path : " + rootpath + "/" + ftpPath + " /ErrorMsg : " + e.ToString());
//                }
//                else
//                {
//                    logger.WriteText("Ftp Download Error - Path : " + rootpath + "/" + ftpPath + " /ErrorMsg : " + e.ToString());
//                }
//            }
//            finally
//            {
//                if (logToDB)
//                {
//                    logger.WriteDB("Ftp Download", "Path : " + rootpath + "/" + ftpPath);
//                }
//                else
//                {
//                    logger.WriteText("Ftp Download - Path : " + rootpath + "/" + ftpPath);
//                }
//            }
//        }
//    }
//}
