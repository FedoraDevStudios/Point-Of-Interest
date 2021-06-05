using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
    [HideMonoScript]
    public class PointOfInterestBehaviour : SerializedMonoBehaviour
    {
        [SerializeField, HideLabel, BoxGroup("Point of Interest"), InlineEditor] IPointOfInterest _pointOfInterest;

        public IPointOfInterest PointOfInterest => _pointOfInterest;

		public void OnDrawGizmos() => _pointOfInterest?.DrawGizmos(transform);
	}
}
