using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Services.Interface;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NET1705_FService.Services.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public async Task<ResponseModel> SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                //Nếu có attachment gửi kèm
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                //Build body html của email
                builder.HtmlBody = mailRequest.Body;
                //Thêm attachment vào email
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                //Gửi mail
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return new ResponseModel
                {
                    Status = "Success",
                    Message = "Send mail successfully!"
                };
            } catch
            {
                return new ResponseModel
                {
                    Status = "Error",
                    Message = "Something went wrong, please try again!"
                };
            }
        }
    }
}
