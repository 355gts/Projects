using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class MessageMapper : ITypeMapper<Message, MessageViewModel>, ITypeMapper<MessageViewModel, Message>
    {
        public MessageViewModel Map(Message fromObject, MessageViewModel toObject = null)
        {
            var message = toObject ?? new MessageViewModel();

            message.EmailAddress = fromObject.EmailAddress;
            message.Id = fromObject.Id;
            message.Message = fromObject.Content;
            message.Name = fromObject.Name;
            message.ReceivedDate = fromObject.ReceivedDate;
            message.Response = fromObject.Response;
            message.Responded = fromObject.Responded;
            message.Subject = fromObject.Subject;

            return message;
        }

        public Message Map(MessageViewModel fromObject, Message toObject = null)
        {
            var message = toObject ?? new Message();

            message.Content = fromObject.Message;
            message.EmailAddress = fromObject.EmailAddress;
            message.Id = fromObject.Id;
            message.Name = fromObject.Name;
            message.ReceivedDate = fromObject.ReceivedDate;
            message.Response = fromObject.Response;
            message.Responded = fromObject.Responded;
            message.Subject = fromObject.Subject;

            return message;
        }
    }
}
