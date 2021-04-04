using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Mappers.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LT.DigitalOffice.MessageService.Broker.Consumers
{
    public class SendEmailConsumer : IConsumer<ISendEmailRequest>
    {
        private readonly IEmailRepository _repository;
        private readonly IMapper<ISendEmailRequest, DbEmail> _mapper;
        private readonly IOptions<SmtpCredentialsOptions> _options;

        public SendEmailConsumer(
            IEmailRepository repository,
            IMapper<ISendEmailRequest, DbEmail> mapper,
            IOptions<SmtpCredentialsOptions> options)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }

        public async Task Consume(ConsumeContext<ISendEmailRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(SendEmail, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }

        private bool SendEmail(ISendEmailRequest request)
        {
            MailAddress from = new MailAddress(_options.Value.Email);
            MailAddress to = new MailAddress(request.Email);

            var m = new MailMessage(from, to)
            {
                Subject = request.Subject,
                Body = request.Text
            };

            SmtpClient smtp = new SmtpClient(_options.Value.Host, _options.Value.Port)
            {
                Credentials = new NetworkCredential(_options.Value.Email, _options.Value.Password),
                EnableSsl = true
            };

            smtp.Send(m);

            SaveEmail(request);

            return true;
        }

        private void SaveEmail(ISendEmailRequest request)
        {
            // TODO fix
            //_repository.SaveEmail(_mapper.Map(request));
        }
    }
}
