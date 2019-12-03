using OneSpirit.Enterprise.Diagnostics;
using OneSpirit.Enterprise.IO;
using OneSpirit.Enterprise.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SET_OTN
{
    class Program
    {
        static string path =  @"./" + DateTime.Now.ToString("yyyy-MM");
        static ExportFile logfile = ExportFile.GetFile(DateTime.Now.ToString("yy-MM-dd"), "txt", path + @"\LOG");
        static void Main(string[] args)
        {
            SEC_OTN otn = new SEC_OTN();
            otn.DBConnect();
            var file = otn.getOnTimeData().ToFileWrite(path);   //ToFileWrite() path parameter input 
            otn.DBdisconnet();

            #region FTP UPLoad
            //string before_path = @"./" + DateTime.Now.AddDays(-1).ToString("yyyy-MM");
            //ExportFile before_file = ExportFile.GetFile("PCBINFOR_A0ND" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd10"), "ff", before_path);

            //FtpServer ftp = new FtpServer() { Host = "stftp.simmtech.com", UserName = "its_simm", Password = "its_simm0000", Path = "/ymsoy" };
            //ftp.ProgressChanged += Ftp_ProgressChanged;
            //ftp.Upload(before_file);

            FtpServer ftp = new FtpServer() { Host = "stftp.simmtech.com", UserName = "its_simm", Password = "its_simm0000", Path = "/ymsoy" };
            ftp.ProgressChanged += Ftp_ProgressChanged;
            ftp.Upload(file);
            #endregion

            #region e-Mail Send
            SmtpServer smtp = new SmtpServer() { Host = "systemmail.simmtech.com", Port = 25, UserName = "ERPSystem@simmtech.com", Password = "" };
            smtp.Attatchments.Add(file.FullName);
            smtp.Attatchments.Add(logfile.FullName);
            smtp.From = smtp.UserName;

            smtp.To.Add("jskwon@simmtech.com");   //권정수 팀장
            smtp.To.Add("parkjh@simmtech.com");   //박진희 차장
            smtp.To.Add("gyuholee@simmtech.com"); //이규호 주임
            smtp.To.Add("hy.kim@simmtech.com");   //김화영 주임
            smtp.CC.Add("soshin@simmtech.com");
            smtp.CC.Add("jipark @simmtech.com");
            smtp.CC.Add("chlee@simmtech.com");
            smtp.CC.Add("lee.kh@simmtech.com");
            smtp.Subject = "[SEC OTN] - " + file.Name + " 첨부 건.";
            smtp.Body.Append("ATTACH SEC OTN DATA FILE AND LOG FILE.");
            smtp.Send();
            #endregion

        }

        private static void Ftp_ProgressChanged(object sender, ProgressEventArgs e)
        {
            TextFileLogWriter logwriter = new TextFileLogWriter(logfile.FullName);
            LogEventArgs logEventArgs = new LogEventArgs(EventLogEntryType.Information, e.ToString());  //defualt Information
            logEventArgs.Message += " ["+ ((ExportFile)sender).Name + "]";
            logwriter.WriteLog(sender, logEventArgs);
        }
    }
}
