using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Helpers;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;

namespace LT.DigitalOffice.MessageService.Broker.Helpers
{
    public abstract class BaseEmailSender
    {
        private readonly IRequestClient<IGetSmtpCredentialsRequest> _rcGetSmtpCredentials;
        private readonly ISmtpCredentialsMapper _mapper;
        protected readonly ILogger _logger;

        private static SmtpCredentials _smtp;

        private bool GetSmtpCredentials()
        {
            string logMessage = "Cannot get smtp credentials.";

            try
            {
                var result = _rcGetSmtpCredentials.GetResponse<IOperationResult<IGetSmtpCredentialsResponse>>(
                    IGetSmtpCredentialsRequest.CreateObj()).Result.Message;

                if (result.IsSuccess)
                {
                    _smtp = _mapper.Map(result.Body);

                    return true;
                }

                _logger.LogWarning(logMessage);

                return false;
            }
            catch(Exception exc)
            {
                _logger.LogError(exc, logMessage);
            }

            return false;
        }

        protected bool Send(DbEmail email)
        {
            if (_smtp == null && !GetSmtpCredentials())
            {
                return false;
            }

            var message = new MailMessage(
                _smtp.Email,
                email.Receiver)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var smtp = new SmtpClient(
                _smtp.Host,
                _smtp.Port)
            {
                Credentials = new NetworkCredential(
                    _smtp.Email,
                    _smtp.Password),
                EnableSsl = _smtp.EnableSsl
            };

            try
            {
                smtp.Send(message);
            }
            catch (Exception exc)
            {
                _logger?.LogWarning(exc,
                    "Errors while sending email to {to} with subject: {subject} and body: {body}. Email replaced to resend queue.",
                    email.Receiver,
                    email.Subject,
                    email.Body);

                return false;
            }

            return true;
        }

        public BaseEmailSender(
            IRequestClient<IGetSmtpCredentialsRequest> rcGetSmtpCredentials,
            ISmtpCredentialsMapper mapper,
            ILogger logger = null)
        {
            _rcGetSmtpCredentials = rcGetSmtpCredentials;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
