using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class PointOfInterestContainerBehaviour : SerializedMonoBehaviour, IPointOfInterestContainer
	{
		[ShowInInspector, ReadOnly] Dictionary<IPointOfInterest, Transform> _pointsOfInterest = new Dictionary<IPointOfInterest, Transform>();

		[Button("Find Points of Interest")]
		private void FindPOIs()
		{
			PointOfInterestBehaviour[] pointsOfInterest = FindObjectsOfType<PointOfInterestBehaviour>();

			foreach (PointOfInterestBehaviour pointOfInterest in pointsOfInterest)
				RegisterPointOfInterest(pointOfInterest.PointOfInterest, pointOfInterest.transform);
		}

		public void RegisterPointOfInterest(IPointOfInterest pointOfInterest, Transform transform)
		{
			if (_pointsOfInterest == null)
				_pointsOfInterest = new Dictionary<IPointOfInterest, Transform>();

			if (!_pointsOfInterest.ContainsKey(pointOfInterest))
				_pointsOfInterest.Add(pointOfInterest, transform);
		}

		public Transform GetPointOfInterest(IPointOfInterest pointOfInterest)
		{
			if (_pointsOfInterest == null || !_pointsOfInterest.ContainsKey(pointOfInterest))
				return null;

			return _pointsOfInterest[pointOfInterest];
		}
	}
}
