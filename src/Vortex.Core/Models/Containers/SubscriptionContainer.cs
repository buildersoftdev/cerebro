using Vortex.Core.Models.Entities.Entries;

namespace Vortex.Core.Models.Containers
{
    public class SubscriptionContainer
    {
        public string? ApplicationName { get; set; }
        public string? AddressName { get; set; }
        public List<SubscriptionEntry>? SubscriptionEntries { get; set; }

        public SubscriptionContainer()
        {
        }
    }
}
