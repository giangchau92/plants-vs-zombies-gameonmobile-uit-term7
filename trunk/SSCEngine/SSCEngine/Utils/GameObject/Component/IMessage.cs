using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSCEngine.Utils.GameObject.Component
{
    public interface IMessage<T>
    {
        T MessageType { get; }
    }
}
