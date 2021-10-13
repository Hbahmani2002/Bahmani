extern alias ITextSharp;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Deployment.Application;
using DesktopPdfSigner.DTO.PDFSignDTO;
using System.Windows.Forms;
using System.Web;
using System.IO;
using ITextSharp::iTextSharp.text.pdf;
using System.Collections.Generic;
using ITextSharp::iTextSharp.text.pdf.security;
using DesktopPdfSigner.utils;
using tr.gov.tubitak.uekae.esya.api.xmlsignature;
using tr.gov.tubitak.uekae.esya.api.xmlsignature.config;
using tr.gov.tubitak.uekae.esya.api.smartcard.pkcs11;
using tr.gov.tubitak.uekae.esya.api.asn.x509;
using tr.gov.tubitak.uekae.esya.api.common.crypto;
using tr.gov.tubitak.uekae.esya.api.smartcard.util;
using tr.gov.tubitak.uekae.esya.api.crypto.alg;
using Constants = DesktopPdfSigner.utils.Constants;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Linq;
using RISE.Service.WEB._TOOL;
using PayFlex.Smartbox.EDesktopPdfSigner;
using PayFlex.Smartbox.EDesktopPdfSigner.Properties;
using tr.gov.tubitak.uekae.esya.api.common.util;
using DesktopPdfSigner.SmartCard;

namespace DesktopPdfSigner
{

    public partial class Form1 : Form
    {
        private PdfRequestDTO requestDTO;
        public static string user;
        public static string pass;
        public static string WsPdfAddress;
        public static string WsXsigAddress;
        private string wssend = "";
        public static string valuecell;
        public static int sayi = 0;
        public static long[] raporkodu1 = new long[1000];
        public static string[] filefile = new string[1000];
        public static DataTable dt = new DataTable();
        public static BindingSource bs = new BindingSource();
        public static DataTable dt1 = new DataTable();
        public static BindingSource bs1 = new BindingSource();

        string line = "";
        long[] id = new long[1000];
        bool checkedall = false;
        long[] RaporKodu = new long[1000];
        string[] adi = new string[1000];
        string[] Fullpath = new string[1000];
        string[] LinkPath = new string[1000];
        RISEReportData[] reports = new RISEReportData[1000];
        CheckBox headerCheckBox = new CheckBox();
        public Form1()
        {
            InitializeComponent();
            requestDTO = new PdfRequestDTO();
            bckWorker.DoWork += bckWorker_doWork;
            bckWorker.RunWorkerCompleted += bckWorker_RunWorkerCompleted;
        }
        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            checkedall = true;
            dataGridView1.EndEdit();
            int i = 0;
            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["column1"] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;
                raporkodu1[i] =
                long.Parse(dataGridView1.Rows[row.Index].Cells[1].Value.ToString());
                filefile[i] = raporkodu1[i] + ".pdf";
                i++;
            }
            // progressBar1.Maximum = i;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //if (Program.versiyonCheck[0] != Assembly.GetEntryAssembly().GetName().Version.ToString())
                //{
                //    MessageBox.Show("Imza Uygulamasini güncellemenız Gerekıyor");
                //    WebsocketServer.Sender("IamDone");
                //    Application.Exit();
                //}
                chkXsig.Checked = false;
                LicenseUtil.setLicenseXml(new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lisans.xml"), FileMode.Open, FileAccess.Read));
                SmartCardManager smartCardManager = SmartCardManager.getInstance();
                var smartCardCertificate = smartCardManager.getSignatureCertificate(true, false);
                DateTime? sertifikatarih = smartCardCertificate.getNotAfter();
                DateTime chek = Convert.ToDateTime(sertifikatarih);
                if(chek < new DateTime())
                {
                    MessageBox.Show("Sertifika süreniz bitmiş lütfen kontrol edin");
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();
                }
                txtSahib.Text = Convert.ToString(smartCardCertificate.getSubject().getCommonNameAttribute());
                
                txtTC.Text = Convert.ToString(smartCardCertificate.getSubject().getSerialNumberAttribute());
                progressBar1.Maximum = Program.raporId.Length;

                // Process line
                filefile = new string[1000];
                raporkodu1 = new long[1000];
                dt.Columns.Add("column1", typeof(bool));

                dt.Columns.Add("ID", typeof(string));

                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                Point headerCellLocation = this.dataGridView1.GetCellDisplayRectangle(0, -1, true).Location;

                //Place the Header CheckBox in the Location of the Header Cell.
                headerCheckBox.Location = new Point(headerCellLocation.X + 35, headerCellLocation.Y + 2);
                headerCheckBox.BackColor = Color.White;
                headerCheckBox.Size = new Size(18, 18);
                headerCheckBox.Click += new EventHandler(HeaderCheckBox_Clicked);

                dataGridView1.Controls.Add(headerCheckBox);
                string subPath = "c:\\RiseReports";
                string subPathundigned = "c:\\RiseReportsUnsigned";// Your code goes here


                bool exists = System.IO.Directory.Exists(subPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(subPath);
                bool exists1 = System.IO.Directory.Exists(subPathundigned);

                if (!exists1)
                    System.IO.Directory.CreateDirectory(subPathundigned);


                //Place the Header CheckBox in the Location of the Header Cell.
                foreach (string sFile in System.IO.Directory.GetFiles(subPath, "*.pdf"))
                {
                    System.IO.File.Delete(sFile);
                }
                foreach (string sFile in System.IO.Directory.GetFiles(subPathundigned, "*.pdf"))
                {
                    System.IO.File.Delete(sFile);
                }

                foreach (string sFile in System.IO.Directory.GetFiles(subPath, "*.xsig"))
                {
                    System.IO.File.Delete(sFile);
                }
                foreach (string sFile in System.IO.Directory.GetFiles(subPathundigned, "*.xsig"))
                {
                    System.IO.File.Delete(sFile);
                }


                // Your code goes here
                dataGridView1.Columns[0].HeaderText = "";
                this.Text = "Protek Imza:" + Convert.ToString(Assembly.GetEntryAssembly().GetName().Version);
                progressBar1.Value = 0;
                //  button1.PerformClick();
                // progressBar1.Maximum = reports.Length;
                bw3.RunWorkerAsync();
                //this.Activated += AfterLoading;

            }
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    Application.Exit();
            //}
            catch (Exception ex)
            {
                writeMe(ex.Message);
                WebsocketServer.Sender("IamDone");
                Application.Exit();

            }
            //InitalizeQuery();
       
        }
        /// <summary>
        /// ClickOnce uygulamasını publish ederseniz queryString ile çağırdğınız da aşağıdaki methodu çağırmalısınız.
        /// </summary>
        private void InitalizeQuery()
        {
            string queryString = "";

            NameValueCollection nameValueTable = new NameValueCollection();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
                nameValueTable = HttpUtility.ParseQueryString(queryString);
            }
        }
        public String AkilliKartlaImzala(string[] dosyaYolu)
        {
           
            string retSignedXmlPath = "";
            string[] retSignedXmlPath1 = new string[dosyaYolu.Length];
            String rest = null;


           
          
            for (int i = 0; i < dosyaYolu.Length; i++)
            {


                Constants.SING_DOCUMENT = dosyaYolu[i];



                retSignedXmlPath = "";
                LisansKontrol.LoadFreeLicense();
                try
                {
                    string currentDirectory11 = Directory.GetCurrentDirectory();
                    var currentDirectory = currentDirectory11 + "\\xmlsignature-config.xml";

                    var context = new Context(currentDirectory) { ValidateCertificates = true };

                    context.Config = new Config(currentDirectory);

                    var signature = new XMLSignature(context) { SigningTime = DateTime.Now };

                    //Transforms tt = new Transforms(context);
                    //Transform tr = new Transform(context, SignatureType.XAdES_BES.ToString());
                    //tt.addTransform(tr);
                    signature.addDocument(dosyaYolu[i], null, true);
                    var terminals = SmartOp.getCardTerminals();
                    if (terminals != null && terminals.Length > 0)
                    {
                        var cardTypeAndSlot = SmartOp.getSlotAndCardType(terminals[0].ToString());
                        var slot = cardTypeAndSlot.getmObj1();
                        var cardType = cardTypeAndSlot.getmObj2();
                        var smartCard = new tr.gov.tubitak.uekae.esya.api.smartcard.pkcs11.SmartCard(cardType);
                        var session = smartCard.openSession(slot);
                        //smartCard.login(session, Constants.SMART_CARD_PIN);

                        var signatureCertificates = smartCard.getSignatureCertificates(session);
                        if (signatureCertificates == null || signatureCertificates.Count == 0)
                        {
                            MessageBox.Show("Kartın içerisinde sertifika bulunamadı.", "Elektronik imza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return null;
                        }
                        ECertificate signingCert = null;
                        foreach (byte[] certByte in signatureCertificates)
                        {
                            var tmpCert = new ECertificate(certByte);
                            if (tmpCert.isQualifiedCertificate())
                            {
                                signingCert = tmpCert;
                                break;
                            }
                        }
                        if (signingCert == null)
                        {
                            MessageBox.Show("Kartın içerisinde sertifika bulunamadı.", "Elektronik imza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return null;
                        }

                        BaseSigner signer = new SCSignerWithCertSerialNo(smartCard, session, slot, signingCert.getSerialNumber().GetData(), SignatureAlg.RSA_SHA256.getName());
                        //if (Constants.SMART_CARD_TC != Constants.SMART_CARD_TC_GECERLI)
                        //{
                        //    var validCertificate = SertifikaKontrol(signingCert);
                        //    if (validCertificate)
                        //    {
                        //        //MessageBox.Show("İmza atılmak istenen sertifika geçerli değil.");
                        //        return null;
                        //    }
                        //}

                        signature.addKeyInfo(signingCert);
                        signature.sign(signer);
                        var sourceFileInfo = new FileInfo(dosyaYolu[i]);
                        if (sourceFileInfo.Directory != null)
                        {
                            var destDirPath = sourceFileInfo.Directory.FullName;
                            retSignedXmlPath = destDirPath + @"\" + sourceFileInfo.Name.Replace(".pdf", ".xsig");
                        }
                        var signatureFileStream = new FileStream(retSignedXmlPath, FileMode.Create);
                        signature.write(signatureFileStream);
                        signatureFileStream.Close();
                        //  MessageBox.Show("Elektronik imza işlemi tamamlandı", "Elektronik imza", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        retSignedXmlPath1[i] = retSignedXmlPath;
                        string filename = Path.GetFileName(retSignedXmlPath);
                        string filewithoutext= Path.GetFileNameWithoutExtension(retSignedXmlPath);
                        //hassan
                        List<HTTPPostClient.HttpUploadFile> listxsig = new List<HTTPPostClient.HttpUploadFile>();
                        
                            byte[] byteRecete = null;
                            using (var reader = new StreamReader(Path.Combine("c:\\RiseReports\\", filename)))
                            {
                                byteRecete = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                            }
                            if (File.Exists(retSignedXmlPath))
                            {
                                File.Copy(retSignedXmlPath, @"c:\RiseReports\" + filewithoutext + ".pdf.xsig", true);
                                File.Delete(retSignedXmlPath);
                               
                            }
                            //      progressBar1.Value++;
                      
                        //    progressBar1.Value = 0;
                       
                          //  var file1 = @"c:\RiseReports\" + filefile[t] + ".xsig";
                            var postData = new
                            {
                                Auth = GetUserInfo(),

                            };
                            var l = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"}}";
                            RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile item = new RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile();
                            item.Filename = filename;
                            item.Name =filename;
                            item.FilePath = retSignedXmlPath;
                            listxsig.Add(item);
                            //int k = 0;
                            //bw2.ReportProgress(t);
                            //  File.Delete("C:\\RiseReports\\" + raporkodu1[t] + ".pdf");

                            //        progressBar1.Value++;

                        
                        foreach (RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile file2 in listxsig)
                        {
                            getPdfsResponse response = JsonConvert.DeserializeObject<getPdfsResponse>(this.SaveXsigReport(file2));
                            if (response.HasError && (response.Type == "FAIL"))
                            {
                               // MessageBox.Show(response.Message);
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Kart okunamadı.", "Elektronik imza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }


                }
                catch (XMLSignatureException exc)
                {
                  //  MessageBox.Show("Hata Oluştu." + exc.Message, "Elektronik imza XmlSignature", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();
                }
                catch (Exception exc)
                {
                 //   MessageBox.Show("Hata Oluştu." + exc.Message, "Elektronik imza Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();
                }

            }
            rest = String.Join(",", retSignedXmlPath1);
            return rest;


        }
        public static void writeMe(string me)
        {
            using (StreamWriter writetext = new StreamWriter(@"C:\\Eimza\\Log.txt"))
            {
                writetext.WriteLine(DateTime.Now.ToString() + "   " + me);
            }
        }
        public void LoadingCircleStart(MRG.Controls.UI.LoadingCircle loCircle, int PrmNspoke, int PrmThickness, int PrmInnerRadius, int PrmOuterRadius, int PrmRotSpeed)
        {
            loCircle.Invoke((MethodInvoker)(() => loCircle.Visible = true));
            loCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.MacOSX;
            loCircle.NumberSpoke = PrmNspoke;
            loCircle.SpokeThickness = PrmThickness;
            loCircle.InnerCircleRadius = PrmInnerRadius;
            loCircle.OuterCircleRadius = PrmOuterRadius;
            loCircle.RotationSpeed = PrmRotSpeed;
            loCircle.Color = System.Drawing.Color.Magenta;
            loCircle.Invoke((MethodInvoker)(() => loCircle.BringToFront()));
            loCircle.Invoke((MethodInvoker)(() => loCircle.Active = true));
        }
        private void btnSign_Click(object sender, EventArgs e)
        {
            loadingCircle.Visible = true;
            LoadingCircleStart(loadingCircle, 12, 6, 5, 53, 100);
            if (!string.IsNullOrEmpty(txtUsbDonglePassword.Text))
            {
                btnSign.Text = "İmzalanıyor , lütfen bekleyiniz.";
                btnSign.Enabled = false;
                this.requestDTO.DonglePassword = txtUsbDonglePassword.Text;
                bckWorker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("USB-Dongle şifrenizi giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadingCircleStop(loadingCircle);
            }
        }
        private void bckWorker_doWork(object sender, DoWorkEventArgs e)
        {
            SignatureManager.SignatureManager signManager = new SignatureManager.SignatureManager();
            signManager.SignPdf(this.requestDTO);
        }
        private void bckWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (e.Error != null)
            {
                MessageBox.Show("Hata oluştu : " + e.Error.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show("Operasyon iptal edildi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // MessageBox.Show("Pdf başarıyla imzalandı ve kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //if (Program.xsigvar == 1)
                //    bckWorkerXsig.RunWorkerAsync();
                //else
                    bwImzalaXsigsiz.RunWorkerAsync();

            }
        }

        private void chBoxPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtUsbDonglePassword.PasswordChar = chBoxPassword.Checked ? '\0' : '*';
        }

        private void btnFileUpload_Click(object sender, EventArgs e)
        {
      

        }
        private string checkSignature(byte[] pdfContent)
        {
            PdfReader reader = new PdfReader(pdfContent);

            AcroFields fields = reader.AcroFields;
            List<String> names = fields.GetSignatureNames();

            // Signature eklenmiş PDF dosyası buraya yollanmalı. Yoksa Verification Gerçekleşemez.

            if (names.Count == 0)
            {
                return "İlgili PDF'e ait imza(lar) bulunamamıştır.";
            }
            string message = string.Empty;
            for (int i = 1; i < names.Count + 1; i++)
            {
                string temp = string.Empty;
                PdfPKCS7 pkcs7 = fields.VerifySignature(names[i - 1]);
                var result = pkcs7.Verify();
                if (result)
                {
                    temp = string.Format("{0}.imza geçerli.", i);
                }
                else
                {
                    temp = string.Format("{0}.imza geçersiz.", i);
                }
                message += temp;
            }
            reader.Close();
            return message;
        }
        private void btnCheckSignature_Click(object sender, EventArgs e)
        {
           // var message = checkSignature(this.requestDTO.pdfContent);
         //   MessageBox.Show(message, "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bckWorkerXsig_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] path = new string[Program.filenameYeni.Length];
            String str1;
            for (int i = 0; i < Program.filenameYeni.Length; i++)
                path[i] = "c:\\RiseReports\\" + Program.filenameYeni[i];
            str1 = AkilliKartlaImzala(path);
            
        }

        private void bckWorkerXsig_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bwImzala.RunWorkerAsync();
           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WebsocketServer.Sender("IamDone");
            Application.Exit();
        }

        private void btnImzala_Click(object sender, EventArgs e)
        {
            bwImzala.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
  
           
        }

        private void bw3_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadingCircleStart(loadingCircle, 12, 6, 5, 53, 100);
            Thread.Sleep(1000);
        }

        private void bw3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bwrungrid.RunWorkerAsync();
         
        }

        private void bw3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void bwrungrid_DoWork(object sender, DoWorkEventArgs e)
        {
            RISEReportData[] reports = new RISEReportData[Program.raporId.Length];
            RISEReportfileData[] reports1 = new RISEReportfileData[Program.raporId.Length];
           
            for (int i = 0; i < Program.raporId.Length; i++)
            {
               
                line = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"},\"ReportIdList\":[{\"Id\":\"" + Program.raporId[i] + "\"}]}";
                id[i] = Convert.ToInt32(Program.raporId[i]);
                //  Fullpath[i] = "https://" + reports[i].ReportFullPath;
                //LinkPath12 = reports[i].reportFullPath.Substring(0, reports[i].reportFullPath.IndexOf("/"));
                //LinkPath12 = "https://" + LinkPath12;
                // LinkPath[i] = Fullpath[i].Substring(0, Fullpath[i].LastIndexOf("/") + 1);
                var data = Encoding.ASCII.GetBytes(line);
                var request1 = (HttpWebRequest)WebRequest.Create(Program.GetirAddress);
                request1.Headers.Add("Authorization", $"Bearer {Program.token}");
                request1.Method = "POST";
                request1.ContentType = "application/json";
                request1.ContentLength = data.Length;

                using (var stream = request1.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response1 = (HttpWebResponse)request1.GetResponse();

                string responseString1 = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                var list1 = new List<string>();


                var item1 = RISEResponseModel.Create<string>();
                if (JsonConvert.DeserializeAnonymousType(responseString1, item1).Data != null)
                {
                    item1 = JsonConvert.DeserializeAnonymousType(responseString1, item1);

                    reports1 = JsonConvert.DeserializeObject<RISEReportfileData[]>(item1.Data);

                    System.IO.File.WriteAllBytes("c:\\RiseReportsUnsigned\\" + id[i] + ".pdf", reports1[0].fileData);
                    progressBar1.Value++;
                }
                else
                {
                    Program.filenameYeni = Program.filenameYeni.Where(c => c != id[i] + ".pdf").ToArray();
                }
                
            }
            if (Program.filenameYeni.Length == 0)
            {
                MessageBox.Show("BU Dosyalar Tümü Imzalanmış Görünüyor");
                WebsocketServer.Sender("IamDone");
                Application.Exit();
            }
            for (int i = 0; i < Program.filenameYeni.Length; i++)
            {



                //  dataGridView1.Rows.Add(false, id[i], reports[i].patientId, reports[i].hospitalName, reports[i].patientName, reports[i].readDoctorUserName, reports[i].CreatedTime, reports[i].ReportTitle, LinkPath[i]);
                // Grab the new row!
                dt.Rows.Add(false, Program.filenameYeni[i].Substring(0, Program.filenameYeni[i].IndexOf(".")));

                // Add the data


            }
            progressBar1.Value = 0;
            //Necessary to end the edit mode of the Cell.
            checkedall = true;
            dataGridView1.EndEdit();
            int ii = 0;
            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["column1"] as DataGridViewCheckBoxCell);
                checkBox.Value = true;
                raporkodu1[ii] =
                long.Parse(dataGridView1.Rows[row.Index].Cells[1].Value.ToString());
                filefile[ii] = raporkodu1[ii] + ".pdf";
                ii++;
            }
            headerCheckBox.Checked = true;

        }

        private void bwrungrid_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bwrungrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingCircle.Visible = false;
            this.requestDTO.pdfContent = new byte[Program.filenameYeni.Length][];
            
            for (int i = 0; i < Program.filenameYeni.Length; i++)
            {
                FileInfo fi = new FileInfo("c:\\RiseReportsUnsigned\\" + Program.filenameYeni[i]);
                if (fi.Length == 0)
                {
                    this.requestDTO.pdfContent[i] = null;
                   
                    Program.filenameYeni[i] = null;
                }
                else
                    this.requestDTO.pdfContent[i] = File.ReadAllBytes("c:\\RiseReportsUnsigned\\" + Program.filenameYeni[i]);
               

            }
            this.requestDTO.pdfContent = this.requestDTO.pdfContent.Where(c => c != null).ToArray();
            Program.filenameYeni = Program.filenameYeni.Where(c => c != null).ToArray();
        }
        public void LoadingCircleStop(MRG.Controls.UI.LoadingCircle loCircle)
        {
            loCircle.Active = false;
            loCircle.Invoke((MethodInvoker)(() => loCircle.Visible = false));
        }

        private void bwImzala_DoWork(object sender, DoWorkEventArgs e)
        {

            bool evet = false;

            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;

                //We don't want a null exception!
                if (cell.Value != null)
                {
                    if (cell.Value.Equals(true))
                    {
                        evet = true;
                    }
                }
                i++;
            }
            if (dataGridView1.Rows.Count == 0 || !evet)
            {
                MessageBox.Show("Raporlarda bir Sorun var");
                WebsocketServer.Sender("IamDone");
                Application.Exit();

            }
            else
            {
                try
                {
                   
                    var dosya1 = "";
                    //  progressBar1.Maximum = filefile.Length;
                    // progressBar1.Value = 0;
                    filefile = filefile.Where(c => c != null).ToArray();
                    raporkodu1 = raporkodu1.Where(c1 => c1 != 0).ToArray();
                    for (int j = 0; j < filefile.Length; j++)
                    {

                        dosya1 = raporkodu1[j] + ".pdf";
                        //byte[] byteRecete = null;
                        //using (var reader = new StreamReader(Path.Combine("c:\\Reports\\", dosya1)))
                        //{
                        //    byteRecete = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                        //}
                        //if (File.Exists(@"c:\ReportsUnsigned\" + dosya1))
                        //{
                        //    //  File.Copy(@"c:\Reports\" + dosya1, @"c:\Reports\" + raporkodu1[j] + ".pdf.xsig", true);
                        //    File.Delete(@"c:\ReportsUnsigned\" + dosya1);
                        //    // dosya1 = raporkodu1[j] + ".pdf.xsig";
                        //}
                        //   progressBar1.Value++;
                    }
                    //  progressBar1.Value = 0;
                    //List<HTTPPostClient.HttpUploadFile> list = new List<HTTPPostClient.HttpUploadFile>();
                    //for (int t = 0; t < filefile.Length; t++)
                    //{
                    //    var file1 = @"c:\Reports\" + filefile[t];
                    //    var postData = new
                    //    {
                    //        Auth = GetUserInfo(),

                    //    };
                    //    var l = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"}}";

                      

                    //    // bw1.ReportProgress(t);
                    //    RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile item = new RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile();
                    //    item.Filename = filefile[t];
                    //    item.Name = filefile[t];
                    //    item.FilePath = file1;
                    //    list.Add(item);

                    //}
                    //foreach (RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile file2 in list)
                    //{
                    //        getPdfsResponse response = JsonConvert.DeserializeObject<getPdfsResponse>(this.SaveSignedReport(file2));
                    //        if (response.HasError && (response.Type == "FAIL"))
                    //        {
                    //        MessageBox.Show(response.Message);
                    //        }
                        
                    //}
                    //List<HTTPPostClient.HttpUploadFile> listxsig = new List<HTTPPostClient.HttpUploadFile>();
                    //for (int j = 0; j < filefile.Length; j++)
                    //{

                    //    dosya1 = raporkodu1[j] + ".xsig";
                    //    byte[] byteRecete = null;
                    //    using (var reader = new StreamReader(Path.Combine("c:\\Reports\\", dosya1)))
                    //    {
                    //        byteRecete = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                    //    }
                    //    if (File.Exists(@"c:\Reports\" + dosya1))
                    //    {
                    //        File.Copy(@"c:\Reports\" + dosya1, @"c:\Reports\" + raporkodu1[j] + ".pdf.xsig", true);
                    //        File.Delete(@"c:\Reports\" + dosya1);
                    //        dosya1 = raporkodu1[j] + ".pdf.xsig";
                    //    }
                    //    //      progressBar1.Value++;
                    //}
                    ////    progressBar1.Value = 0;
                    //for (int t = 0; t < filefile.Length; t++)
                    //{
                    //    var file1 = @"c:\Reports\" + filefile[t] + ".xsig";
                    //    var postData = new
                    //    {
                    //        Auth = GetUserInfo(),

                    //    };
                    //    var l = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"}}";
                    //    RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile item = new RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile();
                    //    item.Filename = filefile[t];
                    //    item.Name = filefile[t];
                    //    item.FilePath = file1;
                    //    listxsig.Add(item);
                    //    //int k = 0;
                    //    //bw2.ReportProgress(t);
                    //  //  File.Delete("C:\\Reports\\" + raporkodu1[t] + ".pdf");

                    //    //        progressBar1.Value++;

                    //}
                    //foreach (RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile file2 in listxsig)
                    //{
                    //    getPdfsResponse response = JsonConvert.DeserializeObject<getPdfsResponse>(this.SaveXsigReport(file2));
                    //    if (response.HasError && (response.Type == "FAIL"))
                    //    {
                    //        MessageBox.Show(response.Message);
                    //    }

                    //}
                   


                }
                catch (Exception ex)
                {
                    writeMe(ex.Message);
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();

                }

            }
        }
        public string SaveSignedReport(RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile filesnm)
        {
            List<RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile> files = new List<RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile> {
                filesnm
            };
            var type = new
            {
                Auth = new
                {
                    UserName = Program.username,
                    Password = Program.password
                }
            };
            return RISE.Service.WEB._TOOL.HTTPPostClient.HTTPPostData((this.wssend == "") ? Program.LinkPdfAddress : this.wssend, files, JsonConvert.SerializeObject(type));
        }
        public string SaveXsigReport(RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile filesnm)
        {
            List<RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile> files = new List<RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile> {
                filesnm
            };
            var type = new
            {
                Auth = new
                {
                    UserName = Program.username,
                    Password = Program.password
                }
            };
            return RISE.Service.WEB._TOOL.HTTPPostClient.HTTPPostData((this.wssend == "") ? Program.LinkAddress : this.wssend, files, JsonConvert.SerializeObject(type));
        }
        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
        private void bwImzala_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WebsocketServer.Sender("IamDone");
          


            Application.Exit();
        }

        private void bwImzala_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        public class HttpForm
        {

            private Dictionary<string, string> _files = new Dictionary<string, string>();
            private Dictionary<string, string> _values = new Dictionary<string, string>();

            public HttpForm(string url)
            {
                this.Url = url;
                this.Method = "POST";
            }

            public string Method { get; set; }
            public string Url { get; set; }

            //return self so that we can chain
            public HttpForm AttachFile(string field, string fileName)
            {
                _files[field] = fileName;
                return this;
            }

            public HttpForm ResetForm()
            {
                _files.Clear();
                _values.Clear();
                return this;
            }

            //return self so that we can chain
            public HttpForm SetValue(string field, string value)
            {
                _values[field] = value;
                return this;
            }

            public HttpWebResponse Submit()
            {
                return this.UploadFiles(_files, _values);
            }


            private HttpWebResponse UploadFiles(Dictionary<string, string> files, Dictionary<string, string> otherValues)
            {
                var req = (HttpWebRequest)WebRequest.Create(this.Url);

                req.AllowWriteStreamBuffering = false;

                //req.ServicePoint.ConnectionLeaseTimeout = 5000;
                //req.ServicePoint.MaxIdleTime = 3000;
                req.Timeout = 50000;
                //req.ReadWriteTimeout = 100000 * 1000;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.AllowAutoRedirect = false;

                var mimeParts = new List<MimePart>();
                try
                {
                    if (otherValues != null)
                    {
                        foreach (var fieldName in otherValues.Keys)
                        {
                            var part = new MimePart();

                            part.Headers["Content-Disposition"] = "form-data; name=\"" + fieldName + "\"";
                            part.Data = new MemoryStream(Encoding.UTF8.GetBytes(otherValues[fieldName]));

                            mimeParts.Add(part);
                        }
                    }

                    if (files != null)
                    {
                        foreach (var fieldName in files.Keys)
                        {
                            var part = new MimePart();

                            part.Headers["Content-Disposition"] = "form-data; name=\"" + fieldName + "\"; filename=\"" + files[fieldName] + "\"";
                            part.Headers["Content-Type"] = "application/octet-stream";
                            part.Data = File.OpenRead(files[fieldName]);

                            mimeParts.Add(part);
                        }
                    }

                    string boundary = "----------" + DateTime.Now.Ticks.ToString("x");

                    req.ContentType = "multipart/form-data; boundary=" + boundary;
                    req.Method = this.Method;

                    long contentLength = 0;

                    byte[] _footer = Encoding.UTF8.GetBytes("--" + boundary + "--\r\n");

                    foreach (MimePart part in mimeParts)
                    {
                        contentLength += part.GenerateHeaderFooterData(boundary);
                    }

                    req.ContentLength = contentLength + _footer.Length;

                    byte[] buffer = new byte[8192];
                    byte[] afterFile = Encoding.UTF8.GetBytes("\r\n");
                    int read;

                    using (Stream s = req.GetRequestStream())
                    {
                        foreach (MimePart part in mimeParts)
                        {
                            s.Write(part.Header, 0, part.Header.Length);

                            while ((read = part.Data.Read(buffer, 0, buffer.Length)) > 0)
                                s.Write(buffer, 0, read);

                            part.Data.Dispose();

                            s.Write(afterFile, 0, afterFile.Length);
                        }

                        s.Write(_footer, 0, _footer.Length);
                    }
                    var webResponse = req.GetResponse();
                    webResponse.Close();
                    return null;
                    //var res = (HttpWebResponse)webResponse;
                    //return res;
                }
                catch (WebException ex)
                {
                    writeMe(ex.Message);
                    foreach (MimePart part in mimeParts)
                        if (part.Data != null)
                            part.Data.Dispose();
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();
                    return (HttpWebResponse)req.GetResponse();
                }
            }

            private class MimePart
            {
                private NameValueCollection _headers = new NameValueCollection();
                public NameValueCollection Headers { get { return _headers; } }

                public byte[] Header { get; protected set; }

                public long GenerateHeaderFooterData(string boundary)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("--");
                    sb.Append(boundary);
                    sb.AppendLine();
                    foreach (string key in _headers.AllKeys)
                    {
                        sb.Append(key);
                        sb.Append(": ");
                        sb.AppendLine(_headers[key]);
                    }
                    sb.AppendLine();

                    Header = Encoding.UTF8.GetBytes(sb.ToString());

                    return Header.Length + Data.Length + 2;
                }

                public Stream Data { get; set; }
            }
        }
        public static object GetUserInfo()
        {
            return new
            {
                UserName = user,
                Password = pass
            };
        }

        private void bckWorker_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
       
        }

        private void loadingCircle_Click(object sender, EventArgs e)
        {

        }

      
        private void bwImzalaXsigsiz_DoWork(object sender, DoWorkEventArgs e)
        {
            bool evet = false;

            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                DataGridViewCheckBoxCell cell = row.Cells[0] as DataGridViewCheckBoxCell;

                //We don't want a null exception!
                if (cell.Value != null)
                {
                    if (cell.Value.Equals(true))
                    {
                        evet = true;
                    }
                }
                i++;
            }
            if (dataGridView1.Rows.Count == 0 || !evet)
            {
                MessageBox.Show("Raporlarda bir Sorun var");
                WebsocketServer.Sender("IamDone");
                Application.Exit();

            }
            else
            {
                try
                {

                    var dosya1 = "";
                    //  progressBar1.Maximum = filefile.Length;
                    // progressBar1.Value = 0;
                    filefile = filefile.Where(c => c != null).ToArray();
                    raporkodu1 = raporkodu1.Where(c1 => c1 != 0).ToArray();
                    for (int j = 0; j < filefile.Length; j++)
                    {

                        dosya1 = raporkodu1[j] + ".pdf";
                        //byte[] byteRecete = null;
                        //using (var reader = new StreamReader(Path.Combine("c:\\Reports\\", dosya1)))
                        //{
                        //    byteRecete = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                        //}
                        //if (File.Exists(@"c:\ReportsUnsigned\" + dosya1))
                        //{
                        //    //  File.Copy(@"c:\Reports\" + dosya1, @"c:\Reports\" + raporkodu1[j] + ".pdf.xsig", true);
                        //    File.Delete(@"c:\ReportsUnsigned\" + dosya1);
                        //    // dosya1 = raporkodu1[j] + ".pdf.xsig";
                        //}
                        //   progressBar1.Value++;
                    }
                    //  progressBar1.Value = 0;
                    //List<HTTPPostClient.HttpUploadFile> list = new List<HTTPPostClient.HttpUploadFile>();
                    //for (int t = 0; t < filefile.Length; t++)
                    //{
                    //    var file1 = @"c:\Reports\" + filefile[t];
                    //    var postData = new
                    //    {
                    //        Auth = GetUserInfo(),

                    //    };
                    //    var l = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"}}";



                    //    // bw1.ReportProgress(t);
                    //    RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile item = new RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile();
                    //    item.Filename = filefile[t];
                    //    item.Name = filefile[t];
                    //    item.FilePath = file1;
                    //    list.Add(item);

                    //}
                    //foreach (RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile file2 in list)
                    //{
                    //    getPdfsResponse response = JsonConvert.DeserializeObject<getPdfsResponse>(this.SaveSignedReport(file2));
                    //    if (response.HasError && (response.Type == "FAIL"))
                    //    {
                    //        MessageBox.Show(response.Message);
                    //    }

                    //}
                 



                }
                catch (Exception ex)
                {
                    writeMe(ex.Message);
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();

                }

            }
        }

        private void bwImzalaXsigsiz_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WebsocketServer.Sender("IamDone");
            string[] files = Directory.GetFiles(@"c:\RiseReports");
            foreach (string file in files)
            {
                FileInfo fl = new FileInfo(file);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(file);
            }
            string[] files1 = Directory.GetFiles(@"c:\RiseReportsUnsigned");
            foreach (string file in files1)
            {
                FileInfo fl = new FileInfo(file);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(file);
            }


            Application.Exit();
        }

        private void chkXsig_CheckedChanged(object sender, EventArgs e)
        {
            if (chkXsig.Checked == false)
                Program.xsigvar = 0;
            else
                Program.xsigvar = 1;
        }
    }
}
