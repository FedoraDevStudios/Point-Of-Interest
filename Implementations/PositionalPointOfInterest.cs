using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class PositionalPointOfInterest : IPointOfInterest
	{
		public string Name => "Positional POI";

		public void DrawGizmos(Transform transform)
		{
			float armLength = 0.5f;
			Gizmos.DrawLine(transform.position + new Vector3(0, armLength, 0), transform.position + new Vector3(0, -armLength, 0));
			Gizmos.DrawLine(transform.position + new Vector3(armLength, 0, 0), transform.position + new Vector3(-armLength, 0, 0));
			Gizmos.DrawLine(transform.position + new Vector3(0, 0, armLength), transform.position + new Vector3(0, 0, -armLength));
		}
	}
}
