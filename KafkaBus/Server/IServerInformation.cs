using KafkaBus.Common;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace KafkaBus.Server
{
    public interface IServerInformation
        : IServerAddressesFeature
    {
        IKafkaBusSubscriber Subscriber { get; }

        void AddAddress(string address);
    }
}