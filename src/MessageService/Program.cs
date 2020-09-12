using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace LT.DigitalOffice.MessageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MailAddress from = new MailAddress("lalagvanan@gmail.com");
            MailAddress to = new MailAddress("st069125@student.spbu.ru");
            var m = new MailMessage(from, to)
            {
                Subject = "Test",
                Body = "Shhhiiiet"
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("lalagvanan@gmail.com", "19102000lll");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(log =>
                {
                    log.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
