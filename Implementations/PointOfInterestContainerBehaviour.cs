using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class PointOfInterestContainerBehaviour : SerializedMonoBehaviour, IPointOfInterestContainer
	{
		[ShowInInspector, ReadOnly] Dictionary<IPointOfInterest, Transform> _pointsOfInterest = new Dictionary<IPointOfInterest, Transform>();

		public void RegisterPointOfInterest(IPointOfInterest pointOfInterest, Transform transform)
		{
			if (_pointsOfInterest == null)
				_pointsOfInterest = new Dictionary<IPointOfInterest, Transform>();

			if (!_pointsOfInterest.ContainsKey(pointOfInterest))
				_pointsOfInterest.Add(pointOfInterest, transform);
		}

		public void UnregisterPointOfInterest(IPointOfInterest pointOfInterest)
		{
			if (_pointsOfInterest == null)
				return;

			if (_pointsOfInterest.ContainsKey(pointOfInterest))
				_pointsOfInterest.Remove(pointOfInterest);
		}

		public Transform GetPointOfInterest(IPointOfInterest pointOfInterest)
		{
			if (_pointsOfInterest == null || !_pointsOfInterest.ContainsKey(pointOfInterest))
				return null;

			return _pointsOfInterest[pointOfInterest];
		}
	}
}
