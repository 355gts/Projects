using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateMessageMapper : ITypeMapper<CreateMessageViewModel, Message>
    {
        public Message Map(CreateMessageViewModel fromObject, Message toObject = null)
        {
            var message = toObject ?? new Message();

            message.Content = fromObject.Message;
            message.EmailAddress = fromObject.EmailAddress;
            message.Name = fromObject.Name;
            message.ReceivedDate = DateTime.UtcNow;
            message.Subject = fromObject.Subject;

            return message;
        }
    }
}
