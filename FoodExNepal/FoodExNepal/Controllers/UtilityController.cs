using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using System.Data.Entity;
using System.Net;
using System.Data;
using System.Net.Mail;
using FoodExNepal.Models;
using System.Data.SqlClient;

namespace FoodExNepal.Controllers
{
    public class UtilityController : Controller
    {
        //
        // GET: /Utility/

        public ActionResult Index()
        {
            EmailSetting setting = db.EmailSettings.Find(1);
            if (setting == null)
            {
                return HttpNotFound();
            }
            setting.Password = Decrypt(setting.Password);
            return View(setting);
        }
        [HttpPost]
        public ActionResult Index(EmailSetting setting)
        {
            if (ModelState.IsValid)
            {
                setting.Password = Encrypt(setting.Password);
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("~/Message");
            }
            return View(setting);
        }

        public static string Encrypt(string toEncrypt, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            string key = "DMSKey";
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing = true)
        {
            byte[] keyArray;

            if (cipherString != null)
            {
                //get the byte code of the string
                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //Get your key from config file to open the lock!
                //string key = (string)settingsReader.GetValue("SecurityKey",
                //typeof(String));
                string key = "DMSKey";

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            else
                return null;
        }

        public void SendMail(List<string> mailAddress, string bodyMessage, string subject)
        {
            //EmailSettingBO emailSettigs = EmailSettingDA.GetEmailSetting();
            //SmtpClient smtp = new SmtpClient();
            //bodyMessage += Environment.NewLine + "Go to DMS Web Site from Here" + Environment.NewLine + "<a href='dms.yetiairlines.com'>dms.yetiairlines.com</a>";


            // EmailSetting emailSetting = new EmailSetting();
            // emailSetting=db.EmailSettings.Find(1);
            //    try
            //    {
            //        smtp.Host = emailSetting.SMTPHost;
            //        smtp.Port = Convert.ToInt32(emailSetting.SMTPPort);
            //        smtp.Credentials = new NetworkCredential(emailSetting.UserName, Decrypt(emailSetting.Password));
            //        smtp.EnableSsl = false;
            //        MailMessage mailMessage = new MailMessage();
            //        mailMessage.From = new MailAddress(emailSetting.FromEmail, emailSetting.FromName);
            //        mailMessage.Subject = subject;
            //        mailMessage.Body = bodyMessage;
            //        //mailMessage.B
            //        mailMessage.IsBodyHtml = true;
            //        foreach (string emailId in mailAddress)
            //        {

            //                mailMessage.To.Add(new MailAddress(emailId.Trim()));
            //                //smtp.Send(mailMessage);

            //                   // ErrorLog err = new ErrorLog();
            //                   // err.ProcessException(ex);
            //                    //continue;
            //        }
            //        //smtp.Send(mailMessage);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    smtp.Dispose();
            //}
        }
        


    }
}
