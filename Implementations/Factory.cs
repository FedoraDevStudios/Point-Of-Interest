using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[CreateAssetMenu(fileName = "Point Of Interest Factory", menuName = "POI/Factory")]
	public class Factory : SerializedScriptableObject, IFactory
	{
		[SerializeField] INodeBridgeConnection _nodeBridgeConnectionFab;

		public INodeBridgeConnection ProduceNodeBridgeConnection() => _nodeBridgeConnectionFab.Produce(this);
	}
}
