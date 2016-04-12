using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using WpfApplication2.Controller;
using System.Windows.Threading;
using System.Threading;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace WpfApplication2.Util
{
    public class LogUtil
    {
        /// <summary>
        /// 根据类型，进行日志记录
        /// </summary>
        /// <param name="code">错误代码级别，0：只写入文件，1：弹出对话框</param>
        /// <param name="des">描述</param>
        /// <param name="str">内容</param>
        //public static void Log(ErrorCode errorcode, String des, String str)
        //{
        //    //新建路径
        //    string path = System.Environment.CurrentDirectory + @"\log\";
        //    int i = 0;
        //    try
        //    {
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        String filename = DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //        if (!File.Exists(filename))
        //        {
        //            File.Create(path + filename);
        //        }
        //        //当日志超过大小后，新建文件进行写
        //        long size = new FileInfo(filename).Length;
        //        if (File.Exists(filename) && size >= 10000)
        //        {
        //            String[] filenames = filename.Split('.');
        //            filename = filenames[0] + (i++) + ".txt";
        //            File.Create(filename);
        //        }
        //        FileStream fs = new FileStream(path + filename, FileMode.Append); ;
        //        StreamWriter sw = new StreamWriter(fs, Encoding.Default);
        //        sw.Write(DateTime.Now.ToString("HH:mm:ss") + " " + str + "\r\n");
        //        sw.Close();
        //        fs.Close();
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    if (errorcode == 1)
        //    {
        //        MessageBox.Show(des, str);
        //    }
        //}

        public static void writeErrInFile(object exmsg)
        {
            string exMessage = exmsg as string;
            //新建路径
            string path = System.Environment.CurrentDirectory + @"\log\";
            int i = 0;
            FileStream fs = null; ;
            StreamWriter sw = null;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                String filename = path + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(filename))
                {
                    File.Create(filename);
                }
                //当日志超过大小后，新建文件进行写
                long size = new FileInfo(filename).Length;
                if (File.Exists(filename) && size >= 10000)
                {
                    String[] filenames = filename.Split('.');
                    filename = filenames[0] + ".txt";
                    File.Create(filename);
                }
                fs = new FileStream(filename, FileMode.Append); ;
                sw = new StreamWriter(fs, Encoding.Default);
                //sw.Write(DateTime.Now.ToString("HH:mm:ss") + " " + err.ErrContent + "\r\n");
                sw.Write(exMessage + "\r\n");
            }
            catch (Exception e)
            {
                Log(false, e.Message.ToString() + "(" + DateTime.Now.ToString() + ")" + "\r\n", (int)ErrorCode.ERR_CODE.WRITE_FILE_ERR);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        public static void Log(bool alterMesBox, string exMessage ,int errCode)
        {
            ErrorCode err = new ErrorCode(errCode, exMessage);
            //Thread writeFileThread = new Thread(new ParameterizedThreadStart(writeErrInFile));
            //writeFileThread.Start();
            writeErrInFile(exMessage);
            if (alterMesBox)
            {
                MessageBox.Show(err.ErrContent,err.ErrDescription);
            }
        }
        /*
        /// <summary>
        ///输入日志到Log4Net 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void WriteLog(Type t, Exception ex)
        {
            log4net.ILog log = Log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void WriteLog(Type t, String msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }
         * */
    }
}