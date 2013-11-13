using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.Utils.GameObject.Component
{
    public interface IMessage<T>
    {
        T MessageType { get; }
        bool Handled { get; set; }
        ulong DestinationObjectId { get; set; }
        object Sender { get; set; }
    }
}
