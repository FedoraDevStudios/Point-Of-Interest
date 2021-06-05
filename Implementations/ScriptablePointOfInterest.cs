using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[HideMonoScript]
	[CreateAssetMenu(fileName = "New Point of Interest", menuName = "Point of Interest")]
	public class ScriptablePointOfInterest : SerializedScriptableObject, IPointOfInterest
	{
		[SerializeField] IPointOfInterest _pointOfInterest;

		public IPointOfInterest PointOfInterest => _pointOfInterest;

		public void DrawGizmos(Transform transform) => PointOfInterest?.DrawGizmos(transform);
	}
}
