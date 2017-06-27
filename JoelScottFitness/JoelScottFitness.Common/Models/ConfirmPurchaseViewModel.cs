using System.Collections.Generic;

namespace JoelScottFitness.Common.Models
{
    public class ConfirmPurchaseViewModel
    {
        public CustomerViewModel CustomerDetails { get; set; }

        public IEnumerable<PlanOptionViewModel> BasketItems { get; set; }
    }
}
