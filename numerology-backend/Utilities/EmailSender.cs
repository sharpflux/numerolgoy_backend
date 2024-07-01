
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Configuration;
using System.Text;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using System.Net.Mime;

namespace OmsSolution.Utilities
{
    public class EmailSender
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmailSender(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }



        public void DispatchEmail(string toEmailAddress, string Invoicefile,string docketfile, StringBuilder table)
        {

            string contentRootPath = _hostingEnvironment.ContentRootPath;

            string templatePath = Path.Combine(contentRootPath, "EmailTemplates", "dispatch-email.html");
            string logoUrl = Path.Combine(contentRootPath, "EmailTemplates", "asset", "initiative-logo.png");

            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}";



            StreamReader reader = new StreamReader(templatePath);

            string readFile = reader.ReadToEnd();
            string StrContent = "";
            StrContent = readFile;
            StrContent = StrContent.Replace("[logourl]", logoUrl);
            StrContent = StrContent.Replace("[DynamicTableContent]", table.ToString());


            DateTime thisDay = DateTime.Now.AddDays(-1);
            string Day = thisDay.ToString("dd");
            string Month = thisDay.ToString("MM");
            string Year = thisDay.ToString("yyyy");

            string Date = Year + "/" + Month + "/" + Day;

            string SenderMail = "hr@sharpflux.com";
            string SenderCompanyName = "Sharpflux";
            string EmailPassword = "Arnav@2541";
            string Host = "us2.smtp.mailhostbox.com";
            int Port = 587;


            var senderEmail = new MailAddress(SenderMail, "Initiative - Dispatch Update");
            var senderEmailPassword = EmailPassword;
            var smtpClient = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, senderEmailPassword)
            };


            var toEmail = new MailAddress(toEmailAddress);


            MailMessage message = new MailMessage(senderEmail, toEmail);

            // Attachments
            string invoicePath = Path.Combine(contentRootPath, "wwwroot", "invoice", Invoicefile);
            string docketPath = Path.Combine(contentRootPath, "wwwroot", "docet", docketfile);

            Attachment invoiceAttachment = new Attachment(invoicePath, MediaTypeNames.Application.Pdf);
            invoiceAttachment.Name = "invoice.pdf";

            Attachment docketAttachment = new Attachment(docketPath, MediaTypeNames.Application.Pdf);
            docketAttachment.Name = "docket.pdf";

            // Add attachments to the email
            message.Attachments.Add(invoiceAttachment);
            message.Attachments.Add(docketAttachment);



            message.Subject = "Initiative - Dispatch Update";
            message.Body = StrContent.ToString();
            message.IsBodyHtml = true;
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }


        }

        public void DispatchPlanEmailToCustomer(string toEmailAddress, StringBuilder table)
        {

            string contentRootPath = _hostingEnvironment.ContentRootPath;

            string templatePath = Path.Combine(contentRootPath, "EmailTemplates", "dispatch-plan-email.html");
            string logoUrl = Path.Combine(contentRootPath, "EmailTemplates", "asset", "initiative-logo.png");

            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}";



            StreamReader reader = new StreamReader(templatePath);

            string readFile = reader.ReadToEnd();
            string StrContent = "";
            StrContent = readFile;
            StrContent = StrContent.Replace("[logourl]", logoUrl);
            StrContent = StrContent.Replace("[DynamicTableContent]", table.ToString());
 

            DateTime thisDay = DateTime.Now.AddDays(-1);
            string Day = thisDay.ToString("dd");
            string Month = thisDay.ToString("MM");
            string Year = thisDay.ToString("yyyy");

            string Date = Year + "/" + Month + "/" + Day;

            string SenderMail = "hr@sharpflux.com";
            string SenderCompanyName = "Sharpflux";
            string EmailPassword = "Arnav@2541";
            string Host = "us2.smtp.mailhostbox.com";
            int Port = 587;


            var senderEmail = new MailAddress(SenderMail, "Initiative - Dispatch Plan Update");
            var senderEmailPassword = EmailPassword;
            var smtpClient = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, senderEmailPassword)
            };


            var toEmail = new MailAddress(toEmailAddress);


            MailMessage message = new MailMessage(senderEmail, toEmail);



            message.Subject = "Initiative - Dispatch Plan Update";
            message.Body = StrContent.ToString();
            message.IsBodyHtml = true;
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }


        }

        public void SendEmailPaymentNotFound(string toEmailAddress, string verificationLink,string Username,string Passwords,string RecipientName)
        {

            string contentRootPath = _hostingEnvironment.ContentRootPath;

            string templatePath = Path.Combine(contentRootPath, "EmailTemplates", "verify-email.html");
            string logoUrl = Path.Combine(contentRootPath, "EmailTemplates", "asset", "initiative-logo.png");

            var request = _httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}";
 


            StreamReader reader = new StreamReader(templatePath);

            string readFile = reader.ReadToEnd();
            string StrContent = "";
            StrContent = readFile;
            StrContent = StrContent.Replace("[logourl]", logoUrl);
            StrContent = StrContent.Replace("[verifylink]",verificationLink);
            StrContent = StrContent.Replace("[Username]", Username);
            StrContent = StrContent.Replace("[Passwords]", Passwords);
            StrContent = StrContent.Replace("[RecipientName]", RecipientName);

            DateTime thisDay = DateTime.Now.AddDays(-1);
            string Day = thisDay.ToString("dd");
            string Month = thisDay.ToString("MM");
            string Year = thisDay.ToString("yyyy");

            string Date = Year + "/" + Month + "/" + Day;

            string SenderMail = "hr@sharpflux.com";
            string SenderCompanyName = "Sharpflux";
            string EmailPassword = "Arnav@2541";
            string Host = "us2.smtp.mailhostbox.com";
            int Port = 587;


            var senderEmail = new MailAddress(SenderMail, SenderCompanyName + "Welcome Email");
            var senderEmailPassword = EmailPassword;
            var smtpClient = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, senderEmailPassword)
            };


            var toEmail = new MailAddress(toEmailAddress);


            MailMessage message = new MailMessage(senderEmail, toEmail);

  

            message.Subject = "Welcome Email";
            message.Body = StrContent.ToString();
            message.IsBodyHtml = true;
            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }


        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}