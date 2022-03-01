using UnityEngine;

namespace FedoraDev.PointOfInterest
{
    public interface INode
    {
        string Name { get; }
        IPointOfInterest PointOfInterest { get; set; }
        INodeBridge[] Bridges { get; set; }
        Rect Position { get; set; }

        bool ProcessEvents(Event currentEvent);
        INode CreateCopy();
    }
}
