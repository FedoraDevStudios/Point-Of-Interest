using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[HideMonoScript]
	[CreateAssetMenu(fileName = "New Point of Interest", menuName = "POI/Point of Interest")]
	public class ScriptablePointOfInterest : SerializedScriptableObject, IPointOfInterest
	{
		public string Name => base.name;
		
		public IPointOfInterest PointOfInterest => _pointOfInterest;

		[SerializeField] IPointOfInterest _pointOfInterest;

		public void DrawGizmos(Transform transform) => PointOfInterest?.DrawGizmos(transform);
	}
}
