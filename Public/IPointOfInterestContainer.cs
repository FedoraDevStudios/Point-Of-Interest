using UnityEngine;

namespace FedoraDev.PointOfInterest
{
    public interface IPointOfInterestContainer
    {
        void RegisterPointOfInterest(IPointOfInterest pointOfInterest, Transform transform);
        Transform GetPointOfInterest(IPointOfInterest pointOfInterest);
    }
}
