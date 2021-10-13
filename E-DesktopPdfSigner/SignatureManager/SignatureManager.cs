using DesktopPdfSigner.DTO.PDFSignDTO;
using DesktopPdfSigner.utils;
using Newtonsoft.Json;
using PayFlex.Smartbox.EDesktopPdfSigner;
using RISE.Service.WEB._TOOL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using tr.gov.tubitak.uekae.esya.api.asn.x509;
using tr.gov.tubitak.uekae.esya.api.common.crypto;
using tr.gov.tubitak.uekae.esya.api.crypto.alg;
using tr.gov.tubitak.uekae.esya.api.smartcard.pkcs11;
using Constants = DesktopPdfSigner.utils.Constants;
using tr.gov.tubitak.uekae.esya.api.smartcard.util;
using tr.gov.tubitak.uekae.esya.api.xmlsignature;
using tr.gov.tubitak.uekae.esya.api.xmlsignature.config;
using System.Text;
using System.Diagnostics;

namespace DesktopPdfSigner.SignatureManager
{
    public class SignatureManager
    {
        public static string user;
        public static string pass;
        private string wssend = "";
        private PdfSigner.PdfSigner _pdfSigner;
        public static string w = null;
        public SignatureManager()
        {
            _pdfSigner = new PdfSigner.PdfSigner();

        }
        public void SignPdf(PdfRequestDTO requestDTO)
        {
            try
            {
                for (int i = 0; i < Program.filenameYeni.Length; i++)
                {
                    var pdfContentWithSign = _pdfSigner.SignPDF(requestDTO, requestDTO.pdfContent[i]);
                    System.IO.File.WriteAllBytes("C:\\RiseReports\\" + Program.filenameYeni[i], pdfContentWithSign);
                    var dosya1 = "";
                    //  progressBar1.Maximum = filefile.Length;
                    // progressBar1.Value = 0;

           

                    if (Program.xsigvar == 1)
                    {
                        string retSignedXmlPath = "";
                        var filexsig = @"c:\RiseReports\" + Program.filenameYeni[i];
                        //hassan............................................................................................
                        Constants.SING_DOCUMENT = filexsig;
                        


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
                           
                            signature.addDocument(filexsig, null, true);
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
                                    // return null;
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
                                    //  return null;
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
                                var sourceFileInfo = new FileInfo(filexsig);
                                if (sourceFileInfo.Directory != null)
                                {
                                    var destDirPath = sourceFileInfo.Directory.FullName;
                                    retSignedXmlPath = destDirPath + @"\" + sourceFileInfo.Name.Replace(".pdf", ".xsig");
                                }
                                var signatureFileStream = new FileStream(retSignedXmlPath, FileMode.Create);
                                signature.write(signatureFileStream);
                                signatureFileStream.Close();
                                //  MessageBox.Show("Elektronik imza işlemi tamamlandı", "Elektronik imza", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // retSignedXmlPath1[i] = retSignedXmlPath;
                                string filename = Path.GetFileName(retSignedXmlPath);
                                string filewithoutext = Path.GetFileNameWithoutExtension(retSignedXmlPath);
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
                                var postData1 = new
                                {
                                    Auth = GetUserInfo(),

                                };
                                var l1 = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"}}";
                                RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile item1 = new RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile();
                                item1.Filename = Program.filenameYeni[i];
                                item1.Name = Program.filenameYeni[i];
                                item1.FilePath = @"c:\RiseReports\" + filewithoutext + ".pdf.xsig";
                                listxsig.Add(item1);
                                //int k = 0;
                                //bw2.ReportProgress(t);
                                //  File.Delete("C:\\RiseReports\\" + raporkodu1[t] + ".pdf");

                                //        progressBar1.Value++;


                                foreach (RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile file2 in listxsig)
                                {
                                    getPdfsResponse response = JsonConvert.DeserializeObject<getPdfsResponse>(this.SaveXsigReport(file2));
                                    if (response.HasError && (response.Type == "FAIL"))
                                    {
                                        

                                                                          }

                                }
                            }
                            else
                            {
                                MessageBox.Show("Kart okunamadı.", "Elektronik imza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //  return null;
                            }
                          

                        }
                        catch (XMLSignatureException exc)
                        {
                            //  MessageBox.Show("Hata Oluştu." + exc.Message, "Elektronik imza XmlSignature", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            writeMe(exc.Message);
                            WebsocketServer.Sender("IamDone");
                            Application.Exit();
                        }
                   
                        catch (Exception exc)
                        {
                            //   MessageBox.Show("Hata Oluştu." + exc.Message, "Elektronik imza Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            writeMe(exc.Message);
                            WebsocketServer.Sender("IamDone");
                            Application.Exit();
                        }
                        //hassan...............................................................................................
                    }
                    List<HTTPPostClient.HttpUploadFile> list = new List<HTTPPostClient.HttpUploadFile>();

                    var file1 = @"c:\RiseReports\" + Program.filenameYeni[i];
                    var file3 = @"c:\RiseReportsUnsigned\" + Program.filenameYeni[i];
                    var postData = new
                    {
                        Auth = GetUserInfo(),

                    };
                    var l = "{\"Auth\":{\"UserName\":\"" + Program.username + "\",\"Password\":\"" + Program.password + "\"}}";



                    // bw1.ReportProgress(t);
                    RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile item = new RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile();
                    item.Filename = Program.filenameYeni[i];
                    item.Name = Program.filenameYeni[i];
                    item.FilePath = file1;
                    list.Add(item);


                    foreach (RISE.Service.WEB._TOOL.HTTPPostClient.HttpUploadFile file2 in list)
                    {
                        getPdfsResponse response = JsonConvert.DeserializeObject<getPdfsResponse>(this.SaveSignedReport(file2));
                        if (response.HasError && (response.Type == "FAIL"))
                        {
                           
                        }

                    }

                  
                   

                }

            }
            catch (Exception ex)
            {
                writeMe(ex.Message);
                w = ex.Message;
            }
            finally
            {
                if (w != null)
                {
                  
                    WebsocketServer.Sender("IamDone");
                    Application.Exit();
                }
            }
        }
public static void writeMe(string me)
        {
            using (StreamWriter writetext = new StreamWriter(@"C:\\Eimza\\Log.txt"))
            {
                writetext.WriteLine(DateTime.Now.ToString()+"   "+me);
            }
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
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        public static object GetUserInfo()
        {
            return new
            {
                UserName = user,
                Password = pass
            };
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
    }
}
