using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class SimpleNodeBridgeConnection : INodeBridgeConnection
	{
		public INode Node { get => _node; set => _node = value; }
		public float Distance { get => _distance; set => _distance = value; }

		[SerializeField, ReadOnly] INode _node;
		[SerializeField, ReadOnly] float _distance;

		public INodeBridgeConnection Produce(IFactory _factory)
		{
			SimpleNodeBridgeConnection connection = new SimpleNodeBridgeConnection();
			connection.Node = Node;
			connection.Distance = Distance;
			return connection;
		}
	}
}
