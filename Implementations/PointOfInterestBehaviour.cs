using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
    [HideMonoScript]
    public class PointOfInterestBehaviour : SerializedMonoBehaviour
    {
        [SerializeField, HideLabel, BoxGroup("Point of Interest"), InlineEditor] IPointOfInterest _pointOfInterest;

        public IPointOfInterest PointOfInterest => _pointOfInterest;

		PointOfInterestContainerBehaviour _povContainer;

		private void OnEnable()
		{
			_povContainer = FindObjectOfType<PointOfInterestContainerBehaviour>();
			_povContainer.RegisterPointOfInterest(_pointOfInterest, transform);
		}

		private void OnDisable()
		{
			_povContainer.UnregisterPointOfInterest(_pointOfInterest);
		}

		public void OnDrawGizmos() => _pointOfInterest?.DrawGizmos(transform);
	}
}
