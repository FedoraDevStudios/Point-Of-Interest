using System;
using System.Collections.Generic;

namespace FedoraDev.PointOfInterest
{
    public interface IFactory
    {
        INodeBridgeConnection ProduceNodeBridgeConnection();
    }
}
