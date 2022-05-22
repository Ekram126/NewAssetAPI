using Asset.Domain;
using Asset.ViewModels.UserVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            //var message = new MessageVM(new string[] { "pineapple_126@hotmail.com" }, "Test email", "This is the content from our email.");
            //_emailSender.SendEmail(message);


            //const string from = "almostakbaltechnology.dev@gmail.com";
            //const string to = "pineapple_126@hotmail.com";
            //const string subject = "This is subject";
            //const string body = "This is body";
            //const string appSpecificPassword = "fajtjigwpcnxyyuv";

            //var mailMessage = new MailMessage(from, to, subject, body);
            //using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            //{
            //    smtpClient.EnableSsl = true;
            //    smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
            //    smtpClient.Send(mailMessage);
            //}



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
