using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Collections.Specialized;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace DesktopPdfSigner
{
    static class Program
    {
        public static string[] filename = null;
        public static string[] filenameYeni = null;
        public static string[] raporId;
        public static string username = "";
        public static string[] versiyonCheck = null;
        public static string password = "";
        public static string token = "";
        public static string LinkAddress = "";
        public static string LinkPdfAddress = "";
        public static int xsigvar = 0;
        public static string GetirAddress = "";
        public static string fileDownload = "";
        public static bool input =true;

       
        public static RISEResponseModel<string> item=null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (PriorProcess() != null)
                {

                    MessageBox.Show("Uygulamanın başka bir örneği zaten çalışıyor.");
                    return;
                }

                var sec = "";
                string[] lines = null;
                args[0] = Uri.UnescapeDataString(args[0]);
                // var lines1 = File.ReadAllLines("C:\\Eimza\\GenelAyarlar.txt");
                //   MessageBox.Show(args[0]);
                args[0] = args[0].Substring(0, args[0].Length - 1);
                sec = args[0].Substring(args[0].Length - 3);
                args[0] = args[0].Substring(0, args[0].Length - 3);
                var file = args[0].Substring(args[0].LastIndexOf("\\") + 1);
                
                       

                WebsocketServer.Aaa();
             
                //Thread.Sleep(100000000);
                if (sec == "nap")
                {
                    lines = File.ReadAllLines("C:\\Eimza\\AyarSeckin.txt");
                   
                }
                else if(sec=="rap")
                {
                    lines = File.ReadAllLines("C:\\Eimza\\Ayar.txt");
                  
                }
                else if (sec == "bet")
                {
                    lines = File.ReadAllLines("C:\\Eimza\\Ayar.txt");

                }
                else if(sec =="tes")
                {
                    lines = File.ReadAllLines("C:\\Eimza\\AyarTest.txt");
                   
                }
               
                    Program.LinkPdfAddress = lines[0];
                    Program.LinkAddress = lines[1];
                    Program.GetirAddress = lines[2];
                    Program.fileDownload = lines[3];
              
                // Process line


                //        var x = "http://raporservis.proteksaglik.com:8282/_Uploads/";

                WebClient web = new WebClient();

           //     web.DownloadFile(x + "VersiyonKontrolu.txt", @"C:\Eimza\VersiyonKontrolu.txt");
           //     versiyonCheck = File.ReadAllLines("C:\\Eimza\\VersiyonKontrolu.txt");


                web.DownloadFile(Program.fileDownload + file, @"C:\Eimza\" + file);
               
                string a = File.ReadAllText(@"C:\Eimza\" + file);

               
                if (!IsBase64String(a))
                    args[0] = args[0].Substring(0, a.Length - 1);
                byte[] data = Convert.FromBase64String(a);
                
                string decodedString = Encoding.UTF8.GetString(data);
              
                Regex regex = new Regex(@"\s");
                string[] bits = regex.Split(decodedString.ToLower());
                //  decodedString = decodedString.Split("");
                raporId = new string[bits.Length - 3];
               
                for (int i = 0; i < bits.Length - 3; i++)
                    raporId[i] = bits[i];
                //raporId[0] = "11362676";
                //raporId[1] = "11362686";
                //raporId[2] = "11223883";
                username = bits[bits.Length - 3];
                password = bits[bits.Length - 2];
                //username = "DR027";
                //password = "123456!";
                token = bits[bits.Length - 1];
                //token = "";
              
                //  raporId[0] = args[0].Replace("eimza://", "");
                filename = new string[raporId.Length];
                for (int i = 0; i < raporId.Length; i++)
                {
                    filename[i] = raporId[i] + ".pdf";

                }
                filenameYeni = filename;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                //bool openProg;
                //Mutex mtx = new Mutex(true, "Form1", out openProg);

                //if (!openProg)
                //{
                //    MessageBox.Show("Çalıştırmak istediğiniz program zaten açık durumda !!");
                //    return;
                //}
                //else
                //{

                //    
                //}
           
                //GC.KeepAlive(mtx);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        public static Process PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }

    }
   
}
