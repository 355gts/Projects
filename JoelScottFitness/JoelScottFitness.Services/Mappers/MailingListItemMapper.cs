using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class MailingListItemMapper : ITypeMapper<MailingListItem, MailingListItemViewModel>, ITypeMapper<MailingListItemViewModel, MailingListItem>
    {
        public MailingListItem Map(MailingListItemViewModel fromObject, MailingListItem toObject = null)
        {
            var mailingListItem = toObject ?? new MailingListItem();

            mailingListItem.Active = fromObject.Active;
            mailingListItem.Email = fromObject.Email;
            mailingListItem.Id = fromObject.Id;

            return mailingListItem;
        }

        public MailingListItemViewModel Map(MailingListItem fromObject, MailingListItemViewModel toObject = null)
        {
            var mailingListItem = toObject ?? new MailingListItemViewModel();

            mailingListItem.Active = fromObject.Active;
            mailingListItem.Email = fromObject.Email;
            mailingListItem.Id = fromObject.Id;

            return mailingListItem;
        }
    }
}
