using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	class SearchNode
	{
		//public INode Node { get; set; }
		//public INodeBridge Bridge { get; set; }
		//public float Cost { get; set; }
		//public BitVector32 Mask { get; set; }
		//public bool IsNode => Node != null;
		//public int Neighbors => IsNode ? Node.Connections.Length : Bridge.Connections.Length;

		//public float EdgeCost(int index) => IsNode ? Node.Connections[index].Distance : Bridge.Connections[index].Distance;
		//public INodeBridge GetNeighborBridge(int index) => Node.Connections[index].Bridge;
		//public INode GetNeighborNode(int index) => Bridge.Connections[index].Node;

		//public SearchNode(INode node, float cost, BitVector32 mask)
		//{
		//	Node = node;
		//	Cost = cost;
		//	Mask = mask;
		//}

		//public SearchNode(INodeBridge bridge, float cost, BitVector32 mask)
		//{
		//	Bridge = bridge;
		//	Cost = cost;
		//	Mask = mask;
		//}
	}

	class Queue
	{
		//public List<SearchNode> Nodes { get; set; }

		//public Queue(params SearchNode[] startNodes)
		//{
		//	Nodes = new List<SearchNode>();
		//	for (int i = 0; i < startNodes.Length; i++)
		//	{
		//		Nodes.Add(startNodes[i]);
		//	}
		//}

		//public void Push(SearchNode node)
		//{
		//	Nodes.Add(node);
		//}

		//public bool ContainsNode(SearchNode node)
		//{
		//	for (int i = 0; i < Nodes.Count; i++)
		//	{
		//		if (Nodes[i].IsNode && Nodes[i].Node == node.Node)
		//			return true;
		//		if (Nodes[i].Bridge == node.Bridge)
		//			return true;
		//	}

		//	return false;
		//}

		//public SearchNode GetNode(SearchNode node)
		//{
		//	for (int i = 0; i < Nodes.Count; i++)
		//	{
		//		if (Nodes[i].IsNode && Nodes[i].Node == node.Node)
		//			return node;
		//		if (Nodes[i].Bridge == node.Bridge)
		//			return node;
		//	}

		//	return null;
		//}

		//public SearchNode PopNodeWithLowestCost()
		//{
		//	float lowestCost = float.MaxValue;
		//	int lowestCostIndex = 0;

		//	for (int i = 0; i < Nodes.Count; i++)
		//	{
		//		if (Nodes[i].Cost < lowestCost)
		//		{
		//			lowestCost = Nodes[i].Cost;
		//			lowestCostIndex = i;
		//		}
		//	}

		//	SearchNode retNode = Nodes[lowestCostIndex];
		//	Nodes.RemoveAt(lowestCostIndex);
		//	return retNode;
		//}
	}

	[CreateAssetMenu(fileName = "New Node Web", menuName = "POI/Node Web")]
	public class ScriptableNodeWeb : SerializedScriptableObject, INodeWeb
	{
		public string Name => base.name;

		public INode[] Nodes
		{
			get
			{
				SetSelfDirty();
				if (_nodes == null)
					_nodes = new INode[0];
				return _nodes;
			}
			set
			{
				_nodes = value;
				SetSelfDirty();
			}
		}

		public Vector2 Offset
		{
			get
			{
				SetSelfDirty();
				return _offset;
			}
			set
			{
				_offset = value;
				SetSelfDirty();
			}
		}

		public IConnection[] Connections
		{
			get
			{
				SetSelfDirty();
				if (_connections == null)
					_connections = new IConnection[0];
				return _connections;
			}

			set
			{
				_connections = value;
				SetSelfDirty();
			}
		}

		//		[SerializeField] IPointOfInterest _startNodeTest;
		//		[SerializeField] IPointOfInterest _endNodeTest;
		//		[SerializeField] IPointOfInterest[] _visitNodesTest;

		[SerializeField, ReadOnly] Vector2 _offset;
		[SerializeField, ReadOnly] INode[] _nodes = new INode[0];
		[SerializeField, ReadOnly] IConnection[] _connections = new IConnection[0];

		public void AddNode(INode node)
		{
			INode[] newNodes = new INode[_nodes.Length + 1];

			for (int i = 0; i < _nodes.Length; i++)
				newNodes[i] = _nodes[i];
			newNodes[_nodes.Length] = node;

			_nodes = newNodes;
			SetSelfDirty();
		}

		public void RemoveNode(INode node)
		{
			for (int i = 0; i < node.Connections.Length; i++)
				node.Connections[i].GetOtherNode(node).RemoveConnection(node);

			List<INode> newNodes = new List<INode>();

			for (int i = 0; i < _nodes.Length; i++)
				if (_nodes[i] != node)
					newNodes.Add(_nodes[i]);

			_nodes = newNodes.ToArray();
			SetSelfDirty();
		}

		public void AddConnection(IConnection connection)
		{
			IConnection[] newConnections = new IConnection[_connections.Length + 1];

			for (int i = 0; i < _connections.Length; i++)
				newConnections[i] = _connections[i];
			newConnections[_connections.Length] = connection;

			_connections = newConnections;
			SetSelfDirty();
		}

		public void RemoveConnection(IConnection connection)
		{
			connection.NodeA.RemoveConnection(connection.NodeB);
			connection.NodeB.RemoveConnection(connection.NodeA);

			List<IConnection> newConnections = new List<IConnection>();

			for (int i = 0; i < _connections.Length; i++)
				if (_connections[i] != connection)
					newConnections.Add(_connections[i]);

			_connections = newConnections.ToArray();
			SetSelfDirty();
		}

		public IPointOfInterest[] GetShortestPath(IPointOfInterest start, IPointOfInterest end, params IPointOfInterest[] mustVisitPOIs)
		{
			return null;
			//			List<IPointOfInterest> pois = new List<IPointOfInterest>();
			//			List<IPointOfInterest> mustVisit = new List<IPointOfInterest>(mustVisitPOIs);

			//			if (mustVisit.Count > 8)
			//			{
			//				Debug.Log("We can only visit up to a maximum of 8 other required places. Any additional requirements will be ignored.");
			//				mustVisit = new List<IPointOfInterest>();
			//				for (int i = 0; i < 8; i++)
			//					mustVisit.Add(mustVisitPOIs[i]);
			//			}

			//			INode startNode = Nodes.Single(n => n.PointOfInterest == start);
			//			INode endNode = Nodes.Single(n => n.PointOfInterest == end);
			//			if (startNode == null || endNode == null || start == end)
			//				return new IPointOfInterest[] { start };

			//			Queue queue = new Queue(new SearchNode(startNode, 0, new BitVector32(0)));
			//			Queue completed = new Queue();
			//			Dictionary<INode, float[]> distance = new Dictionary<INode, float[]>();

			//			for (int i = 0; i < Nodes.Length; i++)
			//			{
			//				distance.Add(Nodes[i], new float[byte.MaxValue]);
			//				for (int j = 0; j < distance[Nodes[i]].Length; j++)
			//					distance[Nodes[i]][j] = float.MaxValue;
			//			}

			//			while (queue.Nodes.Count > 0)
			//			{
			//				SearchNode searchNode = queue.PopNodeWithLowestCost();
			//				if (searchNode.Cost == distance[searchNode.Node][searchNode.Mask.Data])
			//					continue;

			//				for (int i = 0; i < searchNode.Neighbors; i++)
			//				{
			//					BitVector32 newMask = searchNode.Mask;
			//					if (mustVisit.Count > 0 && searchNode.IsNode && mustVisit.Contains(searchNode.Node.PointOfInterest))
			//						newMask[1 << mustVisit.IndexOf(searchNode.Node.PointOfInterest)] = true;

			//					float newCost = searchNode.Cost + searchNode.EdgeCost(i);
			//					if (newCost < distance[searchNode.Node][newMask.Data])
			//					{
			//						completed.Push(searchNode);
			//						distance[searchNode.Node][newMask.Data] = newCost;
			//						SearchNode newSearchNode = searchNode.IsNode ? new SearchNode(searchNode.GetNeighborBridge(i), newCost, newMask) : new SearchNode(searchNode.GetNeighborNode(i), newCost, newMask);
			//						if (completed.ContainsNode(newSearchNode))
			//							newSearchNode = completed.GetNode(newSearchNode);

			//						queue.Push(newSearchNode);
			//					}
			//				}
			//			}

			//			// 0 => 0   => 0000 0000
			//			// 1 => 1   => 0000 0001
			//			// 2 => 3   => 0000 0011
			//			// 3 => 7   => 0000 0111
			//			// 4 => 15  => 0000 1111
			//			// 5 => 31  => 0001 1111
			//			// 6 => 63  => 0011 1111
			//			// 7 => 127 => 0111 1111
			//			// 8 => 255 => 1111 1111

			//			int mask = 0;

			//			if (mustVisit.Count > 0)
			//				mask = (int)Mathf.Pow(2, mustVisit.Count) - 1;

			//			Debug.Log($"{endNode.Name}: {distance[endNode][mask]}");

			//			return pois.ToArray();
		}

		void SetSelfDirty()
		{
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		#region Produce In Editor
#if UNITY_EDITOR
		//public static INodeWeb ProduceInEditor(string location)
		//{
		//	string[] locationArray = location.Split('/');
		//	string folder = "";
		//	string filename = locationArray[locationArray.Length - 1];


		//	for (int i = 0; i < locationArray.Length - 1; i++)
		//	{
		//		if (locationArray[i] == string.Empty)
		//			continue;
		//		folder += $"{locationArray[i]}/";
		//	}

		//	ScriptableNodeWeb nodeWeb = CreateInstance<ScriptableNodeWeb>();

		//	if (!UnityEditor.AssetDatabase.IsValidFolder(folder))
		//		System.IO.Directory.CreateDirectory(folder);
		//	if (System.IO.File.Exists($"{folder}/{filename}"))
		//	{
		//		Debug.Log($"There's already a '{filename}' at this location!");
		//		return null;
		//	}

		//	UnityEditor.AssetDatabase.CreateAsset(nodeWeb, $"{folder}/{filename}");
		//	UnityEditor.AssetDatabase.SaveAssets();

		//	return nodeWeb;
		//}
#endif
		#endregion
	}
}
