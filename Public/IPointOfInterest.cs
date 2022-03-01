using UnityEngine;

namespace FedoraDev.PointOfInterest
{
    public interface IPointOfInterest
    {
        string Name { get; }

        void DrawGizmos(Transform transform);
    }
}
