using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
    [HideMonoScript]
    public class PointOfInterestBehaviour : SerializedMonoBehaviour
    {
        [SerializeField, HideLabel, BoxGroup("Point of Interest"), InlineEditor] IPointOfInterest _pointOfInterest;

		private void Start()
		{
			FindObjectOfType<PointOfInterestContainerBehaviour>().RegisterPointOfInterest(_pointOfInterest, transform);
		}

		public void OnDrawGizmos() => _pointOfInterest?.DrawGizmos(transform);
	}
}
