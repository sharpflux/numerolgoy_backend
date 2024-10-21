
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
using System.Threading.Tasks;

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
        public async Task PredictionCustomerEmailAsync(string toEmailAddress, string pdfName, string CustomerName)
        {
            try
            {
                string contentRootPath = _hostingEnvironment.ContentRootPath;

                string templatePath = Path.Combine(contentRootPath, "EmailTemplates", "prediction-email.html");
                string logoUrl = Path.Combine(contentRootPath, "EmailTemplates", "asset", "Numeromystic-LOGO.png");

                var request = _httpContextAccessor.HttpContext.Request;
                string baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}";

                // Read the email template asynchronously
                string readFile;
                using (StreamReader reader = new StreamReader(templatePath))
                {
                    readFile = await reader.ReadToEndAsync();
                }

                string StrContent = readFile;
                StrContent = StrContent.Replace("[logourl]", logoUrl);
                StrContent = StrContent.Replace("[RecipientName]", CustomerName);

                DateTime thisDay = DateTime.Now.AddDays(-1);
                string Day = thisDay.ToString("dd");
                string Month = thisDay.ToString("MM");
                string Year = thisDay.ToString("yyyy");

                string Date = $"{Year}/{Month}/{Day}";

                string SenderMail = "hr@sharpflux.com";
                string SenderCompanyName = "Sharpflux";
                string EmailPassword = "Arnav@2541";
                string Host = "us2.smtp.mailhostbox.com";
                // int Port = 587;
                int Port = 465;
                var senderEmail = new MailAddress(SenderMail, "Numeromystic - Your Numerology Prediction Report");
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

                var message = new MailMessage(senderEmail, toEmail)
                {
                    Subject = "Numeromystic - Your Numerology Prediction Report",
                    Body = StrContent,
                    IsBodyHtml = true
                };

                string invoicePath = Path.Combine(contentRootPath, "pdf", pdfName + ".pdf");
                Attachment invoiceAttachment = new Attachment(invoicePath, MediaTypeNames.Application.Pdf)
                {
                    Name = "report.pdf"
                };

                // Add attachments to the email
                message.Attachments.Add(invoiceAttachment);


                // Send the email asynchronously
                await smtpClient.SendMailAsync(message);


            }
            catch (Exception ex)
            {

                throw ex;
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