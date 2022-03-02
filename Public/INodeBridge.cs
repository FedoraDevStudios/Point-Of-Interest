using UnityEngine;

namespace FedoraDev.PointOfInterest
{
	public interface INodeBridge
    {
        string Name { get; }
        Vector2 Size { get; }
        INodeBridgeConnection[] Connections { get; set; }
        Rect Position { get; set; }

        bool ProcessEvents(Event currentEvent);
        INodeBridge CreateCopy();
        void Move(Vector2 delta);
        void Place();
    }
}
