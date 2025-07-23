using E_Commerce.Interfaces;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace E_Commerce.Repository
{
    public class SmsService : ISMsService
    {
        private readonly TwilioSettings _twilio;

        public SmsService(IOptions<TwilioSettings> twilio)
        {
            _twilio = twilio.Value;
        }
        public MessageResource Send(string PhoneNumber, string Body)
        {
            TwilioClient.Init(_twilio.AccountSID, _twilio.AuthToken);
            var result = MessageResource.Create(
                 body: Body,
                 from: new Twilio.Types.PhoneNumber(_twilio.FromPhoneNumber),
                 to: new Twilio.Types.PhoneNumber("+20" + PhoneNumber)
                );
            return result;
        }
    }
}
