using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class RotationalPointOfInterest : IPointOfInterest
	{
		public void DrawGizmos(Transform transform)
		{
			float armLength = 0.5f;

			Vector3 point0 = transform.position;
			Vector3 point1 = point0 + (transform.up * armLength / 2f);
			Vector3 point2 = point0 + (transform.forward * armLength);
			Vector3 point3 = point0 + (transform.right * armLength / 2f);
			Vector3 point4 = point0 + (transform.right * -armLength / 2f);

			Gizmos.DrawLine(point0, point1);
			Gizmos.DrawLine(point1, point2);
			Gizmos.DrawLine(point0, point3);
			Gizmos.DrawLine(point3, point2);
			Gizmos.DrawLine(point0, point4);
			Gizmos.DrawLine(point4, point2);
		}
	}
}
