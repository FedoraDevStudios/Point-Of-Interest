using UnityEngine;

namespace FedoraDev.PointOfInterest
{
    public interface IPointOfInterestContainer
    {
        void RegisterPointOfInterest(IPointOfInterest pointOfInterest, Transform transform);
		void UnregisterPointOfInterest(IPointOfInterest pointOfInterest);
        Transform GetPointOfInterest(IPointOfInterest pointOfInterest);
	}
}
