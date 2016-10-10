using Microsoft.AspNetCore.Hosting.Server.Features;
using KafkaBus.Common;

namespace KafkaBus.Server
{

    public interface IServerInformation
        : IServerAddressesFeature
    {

        IKafkaBusSubscriber Subscriber { get; }

        void AddAddress(string address);

    }

}