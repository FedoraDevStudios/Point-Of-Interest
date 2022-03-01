using UnityEngine;

namespace FedoraDev.PointOfInterest
{
	public interface INodeBridge
    {
        string Name { get; }
        INodeBridgeConnection[] Connections { get; set; }
        Rect Position { get; set; }

        bool ProcessEvents(Event currentEvent);
        INodeBridge CreateCopy();
    }
}
