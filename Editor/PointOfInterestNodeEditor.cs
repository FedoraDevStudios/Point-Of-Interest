using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Editor
{
	public class PointOfInterestNodeEditor : EditorWindow
	{
		#region Properties
		const string FACTORY_PATH = "Assets/Fedora Dev/";
		const string FACTORY_NAME = "Point Of Interest Factory.asset";
		static string FactoryAsset => $"{FACTORY_PATH}{FACTORY_NAME}";

		public static INodeWeb NodeWeb { get; set; }

		IFactory FactoryInstance
		{
			get
			{
				if (_factoryInstance == null)
					_factoryInstance = (IFactory)AssetDatabase.LoadAssetAtPath(FactoryAsset, typeof(IFactory));

				if (_factoryInstance == null)
					throw new NullReferenceException($"No factory reference found at {FactoryAsset}. Please create one!");

				return _factoryInstance;
			}
		}

		IFactory _factoryInstance;
		string _targetLocation = "";
		GUIStyle _nodeStyle;
		GUIStyle _bridgeStyle;
		GUIStyle _textStyle;
		GUIStyle _connectionStyle;
		GUIStyle _uiPositionStyle;
		Type[] _nodeWebClasses;
		Vector2 _nodeSize = new Vector2(100, 100);
		Vector2 _bridgeSize = new Vector2(75, 50);
		INode _connectingFromNode;
		bool _connectingNodes;
		#endregion

		#region Initialization
		[MenuItem("Tools/Point of Interest Editor")]
		public static void OpenWindow()
		{
			PointOfInterestNodeEditor window = GetWindow<PointOfInterestNodeEditor>();
			if (NodeWeb == null)
				window.titleContent = new GUIContent("POI Web - New");
			else
				window.titleContent = new GUIContent($"POI Web - {NodeWeb.Name}");
		}

		void OnEnable()
		{
			_nodeStyle = new GUIStyle();
			//_nodeStyle.normal.background = new Texture2D(1, 1);
			_nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
			_nodeStyle.border = new RectOffset(12, 12, 12, 12);

			_bridgeStyle = new GUIStyle(_nodeStyle);
			_bridgeStyle.alignment = TextAnchor.MiddleCenter;

			_textStyle = new GUIStyle();
			_textStyle.alignment = TextAnchor.MiddleCenter;

			_connectionStyle = new GUIStyle(_textStyle);
			_connectionStyle.normal.background = new Texture2D(1, 1);

			_uiPositionStyle = new GUIStyle();
			_uiPositionStyle.normal.background = new Texture2D(1, 1);
			_uiPositionStyle.alignment = TextAnchor.MiddleCenter;

			_nodeWebClasses = GetAllThatImplement<INodeWeb>();

			if (NodeWeb != null)
			{
				for (int i = 0; i < NodeWeb.Nodes.Length; i++)
					NodeWeb.Nodes[i].Position = new Rect(NodeWeb.Nodes[i].Position.x, NodeWeb.Nodes[i].Position.y, _nodeSize.x, _nodeSize.y);
				for (int i = 0; i < NodeWeb.Bridges.Length; i++)
					NodeWeb.Bridges[i].Position = new Rect(NodeWeb.Bridges[i].Position.x, NodeWeb.Bridges[i].Position.y, _bridgeSize.x, _bridgeSize.y);
			}
		}

		private void OnDisable()
		{
			NodeWeb = null;
		}

		public static Type[] GetAllThatImplement<T>()
		{
			Type interfaceType = typeof(T);
			Type[] classes = AppDomain.CurrentDomain.GetAssemblies()
						  .SelectMany(assembly => assembly.GetTypes())
						  .Where(cls => interfaceType.IsAssignableFrom(cls) && cls.IsClass)
						  .ToArray();

			return classes;
		}
		#endregion

		#region OnGUI
		private void OnGUI()
		{
			if (NodeWeb == null)
				DrawEmptyEditor();
			else
				DrawNodeEditor();

			if (GUI.changed)
				Repaint();
		}

		void DrawEmptyEditor()
		{
			float width = position.width < 500 ? position.width : 500;

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(GUILayout.Width(width));
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Create a new Node Web:");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Assets/");
			_targetLocation = GUILayout.TextField(_targetLocation, GUILayout.MinWidth(100f));
			GUILayout.Label("/Node Web.asset");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			for (int i = 0; i < _nodeWebClasses.Length; i++)
			{
				int index = i;
				string className = ObjectNames.NicifyVariableName(_nodeWebClasses[index].Name);
				if (GUILayout.Button($"{className}"))
				{
					NodeWeb = _nodeWebClasses[index].GetMethod("ProduceInEditor").Invoke(null, new object[] { $"Assets/{_targetLocation}/Node Web.asset" }) as INodeWeb;
					NodeWeb.Offset = new Vector2(position.width / 2, position.height / 2);
					AssetDatabase.SaveAssets();
				}
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		void DrawNodeEditor()
		{
			DrawGrid(20, 0.2f, Color.gray);
			DrawGrid(100, 0.4f, Color.gray);
			DrawZero();
			DrawMouseConnectionLine(Event.current);
			DrawBridges();
			DrawNodes();
			DrawUI();
			ProcessConnectionEvents(Event.current);
			ProcessNodeEvents(Event.current);
			ProcessBridgeEvents(Event.current);
			ProcessEvents(Event.current);
		}
		#endregion

		void DrawGrid(float spacing, float opacity, Color color)
		{
			int widthDivs = Mathf.CeilToInt(position.width / spacing);
			int heightDivs = Mathf.CeilToInt(position.height / spacing);

			Handles.BeginGUI();
			Handles.color = new Color(color.r, color.g, color.b, opacity);

			float offsetX = Mathf.Abs(NodeWeb.Offset.x) % spacing;
			float offsetY = Mathf.Abs(NodeWeb.Offset.y) % spacing;

			if (NodeWeb.Offset.x < 0)
				offsetX = spacing - offsetX;

			if (NodeWeb.Offset.y < 0)
				offsetY = spacing - offsetY;

			Vector3 offset = new Vector3(offsetX, offsetY, 0f);

			for (int i = 0; i < widthDivs; i++)
				Handles.DrawLine(new Vector3(spacing * i, -spacing, 0f) + offset, new Vector3(spacing * i, position.height, 0f) + offset);

			for (int i = 0; i < heightDivs; i++)
				Handles.DrawLine(new Vector3(-spacing, spacing * i, 0f) + offset, new Vector3(position.width, spacing * i, 0f) + offset);

			Handles.EndGUI();
		}

		void DrawZero()
		{
			Handles.BeginGUI();
			Handles.color = Color.blue;

			if (NodeWeb.Offset.x > -5f && NodeWeb.Offset.x < position.width + 5f)
				Handles.DrawLine(new Vector3(NodeWeb.Offset.x, -5, 0), new Vector3(NodeWeb.Offset.x, position.height + 5, 0), 2f); //Vertical Line

			if (NodeWeb.Offset.y > -5f && NodeWeb.Offset.y < position.height + 5f)
				Handles.DrawLine(new Vector3(-5, NodeWeb.Offset.y, 0), new Vector3(position.width + 5, NodeWeb.Offset.y, 0), 2f); //Horizontal Line
			Handles.EndGUI();
		}

		void DrawMouseConnectionLine(Event currentEvent)
		{
			if (_connectingNodes)
			{
				Handles.color = Color.blue;
				Handles.DrawLine(new Vector3(_connectingFromNode.Position.x + (_nodeSize.x / 2), _connectingFromNode.Position.y + (_nodeSize.y / 2), 0),
								 new Vector3(currentEvent.mousePosition.x, currentEvent.mousePosition.y,
							 	 0), 4f);
				GUI.changed = true;
			}
		}

		void DrawNodes()
		{
			for (int i = 0; i < NodeWeb.Nodes.Length; i++)
			{
				INode node = NodeWeb.Nodes[i];
				Rect textBox = node.Position;
				textBox.height -= 10;
				textBox.y += 5;
				Rect objectBox = textBox;
				Rect connectionBox = textBox;
				float boxHeight = textBox.height / 2f;

				objectBox.height = boxHeight;
				connectionBox.height = boxHeight - 12;
				connectionBox.width = textBox.width - 24;

				objectBox.position = new Vector2(objectBox.position.x, objectBox.position.y);
				connectionBox.position = new Vector2(connectionBox.position.x + 12, connectionBox.position.y + boxHeight);

				GUI.Box(node.Position, string.Empty, _nodeStyle);
				GUI.Label(objectBox, node.PointOfInterest == null ? "No PoI" : node.PointOfInterest.Name, _textStyle);
				GUI.Label(connectionBox, "->", _connectionStyle);
			}
		}

		void DrawBridges()
		{
			for (int i = 0; i < NodeWeb.Bridges.Length; i++)
			{
				Handles.BeginGUI();
				Handles.color = Color.white;
				INodeBridge bridge = NodeWeb.Bridges[i];

				Vector3 pos = new Vector3(bridge.Position.x + (_bridgeSize.x / 2), bridge.Position.y + (_bridgeSize.y / 2), 0);
				for (int j = 0; j < bridge.Connections.Length; j++)
				{
					INode node = bridge.Connections[j].Node;
					Rect nodePos = node.Position;
					Vector2 deletePosition = (new Vector2(node.Position.x + (_nodeSize.x / 2), node.Position.y + (_nodeSize.y / 2)) + new Vector2(bridge.Position.x + (_bridgeSize.x / 2), bridge.Position.y + (_bridgeSize.y / 2))) / 2;
					Vector2 floatPosition = deletePosition + new Vector2(-25, 20);

					Handles.DrawLine(new Vector3(nodePos.x + (_nodeSize.x / 2), nodePos.y + (_nodeSize.y / 2), 0), pos, 3f);
					if (GUI.Button(new Rect(deletePosition.x - 10, deletePosition.y - 10, 20, 20), "x"))
					{
						List<INodeBridge> bridges = new List<INodeBridge>(node.Bridges);
						bridges.Remove(bridge);
						node.Bridges = bridges.ToArray();

						List<INodeBridgeConnection> conns = new List<INodeBridgeConnection>(bridge.Connections);
						conns.RemoveAt(j);
						bridge.Connections = conns.ToArray();
						GUI.changed = true;

						return;
					}

					float oldValue = bridge.Connections[j].Distance;
					bridge.Connections[j].Distance = EditorGUI.FloatField(new Rect(floatPosition.x, floatPosition.y, 50, 20), bridge.Connections[j].Distance);
					if (bridge.Connections[j].Distance != oldValue)
					{
						AssetDatabase.SaveAssets();
						GUI.changed = true;
						return;
					}
				}
				Handles.EndGUI();

				GUI.Box(bridge.Position, bridge.Name, _bridgeStyle);
			}
		}

		void DrawUI()
		{
			float width = 100f;
			float btnWidth = 100;
			GUI.Box(new Rect(position.width - width, 0, width, 25), $"({NodeWeb.Offset.x}, {NodeWeb.Offset.y})", _uiPositionStyle);
			if (GUI.Button(new Rect(position.width - width - btnWidth, 0, btnWidth, 25), "Recenter"))
			{
				Vector2 objectCenter = Vector2.zero;

				for (int i = 0; i < NodeWeb.Nodes.Length; i++)
					objectCenter += new Vector2(NodeWeb.Nodes[i].Position.x, NodeWeb.Nodes[i].Position.y);

				for (int i = 0; i < NodeWeb.Bridges.Length; i++)
					objectCenter += new Vector2(NodeWeb.Bridges[i].Position.x, NodeWeb.Bridges[i].Position.y);

				objectCenter /= (NodeWeb.Nodes.Length + NodeWeb.Bridges.Length);
				Vector2 offset = -objectCenter;

				for (int i = 0; i < NodeWeb.Nodes.Length; i++)
				{
					Rect pos = NodeWeb.Nodes[i].Position;
					pos = new Rect(pos.x + offset.x + position.width / 2, pos.y + offset.y + position.height / 2, pos.width, pos.height);
					NodeWeb.Nodes[i].Position = pos;
				}

				for (int i = 0; i < NodeWeb.Bridges.Length; i++)
				{
					Rect pos = NodeWeb.Bridges[i].Position;
					pos = new Rect(pos.x + offset.x + position.width / 2, pos.y + offset.y + position.height / 2, pos.width, pos.height);
					NodeWeb.Bridges[i].Position = pos;
				}

				NodeWeb.Offset = new Vector2(position.width / 2, position.height / 2);

				AssetDatabase.SaveAssets();
			}
		}

		void ProcessConnectionEvents(Event currentEvent)
		{
			switch (currentEvent.type)
			{
				case EventType.MouseDown:
					if (currentEvent.button == 0)
					{
						for (int i = 0; i < NodeWeb.Nodes.Length; i++)
						{
							INode node = NodeWeb.Nodes[i];
							float thirdHeight = node.Position.height / 2f;
							Rect connectionRect = new Rect(node.Position.x, thirdHeight + node.Position.y, node.Position.width, thirdHeight);
							if (connectionRect.Contains(currentEvent.mousePosition))
							{
								_connectingNodes = true;
								_connectingFromNode = node;
								currentEvent.Use();
								GUI.changed = true;
								return;
							}
						}
					}
					break;

				case EventType.MouseUp:
					if (_connectingNodes && currentEvent.button == 0)
					{
						for (int i = 0; i < NodeWeb.Nodes.Length; i++)
						{
							INode node = NodeWeb.Nodes[i];
							if (node.Position.Contains(currentEvent.mousePosition))
							{
								if (_connectingFromNode == node)
									continue;

								INodeBridgeConnection nodeAConnection = FactoryInstance.ProduceNodeBridgeConnection();
								INodeBridgeConnection nodeBConnection = FactoryInstance.ProduceNodeBridgeConnection();
								INodeBridge bridge;

								if (NodeWeb.Bridges.Length == 0)
									bridge = Activator.CreateInstance(GetAllThatImplement<INodeBridge>()[0]) as INodeBridge;
								else
									bridge = NodeWeb.Bridges[0].CreateCopy();

								nodeAConnection.Distance = 0;
								nodeAConnection.Node = node;

								nodeBConnection.Distance = 0;
								nodeBConnection.Node = _connectingFromNode;

								Vector2 bridgePosition = (new Vector2(_connectingFromNode.Position.x, _connectingFromNode.Position.y) + new Vector2(node.Position.x, node.Position.y)) / 2;

								bridge.Position = new Rect(bridgePosition, _bridgeSize);
								bridge.Connections = bridge.Connections.Append(nodeAConnection).Append(nodeBConnection).ToArray();
								NodeWeb.Bridges = NodeWeb.Bridges.Append(bridge).ToArray();
								node.Bridges = node.Bridges.Append(bridge).ToArray();
								_connectingFromNode.Bridges = _connectingFromNode.Bridges.Append(bridge).ToArray();

								AssetDatabase.SaveAssets();
							}
						}

						for (int i = 0; i < NodeWeb.Bridges.Length; i++)
						{
							INodeBridge bridge = NodeWeb.Bridges[i];
							if (bridge.Position.Contains(currentEvent.mousePosition))
							{
								bool cont = false;
								for (int j = 0; j < bridge.Connections.Length; j++)
									if (bridge.Connections[j].Node == _connectingFromNode)
										cont = true;
								if (cont) continue;

								INodeBridgeConnection nodeConnection = FactoryInstance.ProduceNodeBridgeConnection();

								nodeConnection.Distance = 0;
								nodeConnection.Node = _connectingFromNode;

								bridge.Connections = bridge.Connections.Append(nodeConnection).ToArray();
								_connectingFromNode.Bridges = _connectingFromNode.Bridges.Append(bridge).ToArray();
								AssetDatabase.SaveAssets();
							}
						}

						_connectingNodes = false;
						_connectingFromNode = null;
					}
					break;
			}
		}

		void ProcessNodeEvents(Event currentEvent)
		{
			for (int i = 0; i < NodeWeb.Nodes.Length; i++)
			{
				if (NodeWeb.Nodes[i].ProcessEvents(currentEvent))
					GUI.changed = true;
			}
		}

		void ProcessBridgeEvents(Event currentEvent)
		{
			for (int i = 0; i < NodeWeb.Bridges.Length; i++)
			{
				if (NodeWeb.Bridges[i].ProcessEvents(currentEvent))
					GUI.changed = true;
			}
		}

		void ProcessEvents(Event currentEvent)
		{
			switch (currentEvent.type)
			{
				case EventType.MouseDown:
					if (currentEvent.button == 1)
						ProcessContextMenu(currentEvent.mousePosition);
					break;

				case EventType.MouseDrag:
					if (currentEvent.button == 2)
						OnDrag(currentEvent.delta);
					break;

				case EventType.MouseUp:
					if (currentEvent.button == 2)
						AssetDatabase.SaveAssets();
					break;

				case EventType.DragUpdated:
					if ((DragAndDrop.objectReferences[0] as IPointOfInterest) != null)
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
						DragAndDrop.AcceptDrag();
					}
					break;

				case EventType.DragPerform:
					for (int i = 0; i < NodeWeb.Nodes.Length; i++)
					{
						if (NodeWeb.Nodes[i].Position.Contains(currentEvent.mousePosition))
							NodeWeb.Nodes[i].PointOfInterest = DragAndDrop.objectReferences[0] as IPointOfInterest;
						currentEvent.Use();
						GUI.changed = true;
					}

					for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
					{
						IPointOfInterest poi = DragAndDrop.objectReferences[i] as IPointOfInterest;
						if (poi == null)
							continue;

						INode node;

						if (NodeWeb.Nodes.Length == 0)
							node = Activator.CreateInstance(GetAllThatImplement<INode>()[0]) as INode;
						else
							node = NodeWeb.Nodes[NodeWeb.Nodes.Length - 1].CreateCopy();
						node.Position = new Rect(currentEvent.mousePosition.x, currentEvent.mousePosition.y, _nodeSize.x, _nodeSize.y);
						node.PointOfInterest = poi;
						NodeWeb.Nodes = NodeWeb.Nodes.Append(node).ToArray();
						AssetDatabase.SaveAssets();
					}
					break;
			}
		}

		void ProcessContextMenu(Vector2 mousePosition)
		{
			GenericMenu genericMenu = new GenericMenu();

			bool added1 = false;
			for (int i = 0; i < NodeWeb.Nodes.Length; i++)
			{
				INode node = NodeWeb.Nodes[i];
				if (node.Position.Contains(mousePosition))
				{
					genericMenu.AddItem(new GUIContent($"Remove {node.Name}"), false, () => OnClickRemoveNode(node));
					added1 = true;
				}
			}

			if (added1)
				genericMenu.AddSeparator(string.Empty);

			added1 = false;
			for (int i = 0; i < NodeWeb.Bridges.Length; i++)
			{
				INodeBridge bridge = NodeWeb.Bridges[i];
				if (bridge.Position.Contains(mousePosition))
				{
					genericMenu.AddItem(new GUIContent($"Remove {bridge.Name}"), false, () => OnClickRemoveBridge(bridge));
					added1 = true;
				}
			}

			if (added1)
				genericMenu.AddSeparator(string.Empty);

			Type[] iNodes = GetAllThatImplement<INode>();
			for (int i = 0; i < iNodes.Length; i++)
			{
				int index = i;
				genericMenu.AddItem(new GUIContent($"Add {iNodes[index].Name}"), false, () => OnClickAddNode(iNodes[index], mousePosition));
			}

			genericMenu.AddSeparator(string.Empty);

			Type[] iBridges = GetAllThatImplement<INodeBridge>();
			for (int i = 0; i < iBridges.Length; i++)
			{
				int index = i;
				genericMenu.AddItem(new GUIContent($"Add {iBridges[index].Name}"), false, () => OnClickAddBridge(iBridges[index], mousePosition));
			}

			genericMenu.ShowAsContext();
		}

		void OnDrag(Vector2 delta)
		{
			NodeWeb.Offset += delta;

			for (int i = 0; i < NodeWeb.Nodes.Length; i++)
			{
				Rect pos = NodeWeb.Nodes[i].Position;
				pos.x += delta.x;
				pos.y += delta.y;
				NodeWeb.Nodes[i].Position = pos;
			}

			for (int i = 0; i < NodeWeb.Bridges.Length; i++)
			{
				Rect pos = NodeWeb.Bridges[i].Position;
				pos.x += delta.x;
				pos.y += delta.y;
				NodeWeb.Bridges[i].Position = pos;
			}

			GUI.changed = true;
		}

		void OnClickAddNode(Type nodeType, Vector2 mousePosition)
		{
			INode node = Activator.CreateInstance(nodeType) as INode;
			node.Position = new Rect(mousePosition.x, mousePosition.y, _nodeSize.x, _nodeSize.y);

			NodeWeb.Nodes = NodeWeb.Nodes.Append(node).ToArray();
			AssetDatabase.SaveAssets();
		}

		void OnClickAddBridge(Type bridgeType, Vector2 mousePosition)
		{
			INodeBridge bridge = Activator.CreateInstance(bridgeType) as INodeBridge;
			bridge.Position = new Rect(mousePosition.x, mousePosition.y, _bridgeSize.x, _bridgeSize.y);

			NodeWeb.Bridges = NodeWeb.Bridges.Append(bridge).ToArray();
			AssetDatabase.SaveAssets();
		}

		void OnClickRemoveNode(INode node)
		{
			for (int i = 0; i < node.Bridges.Length; i++)
			{
				INodeBridge bridge = node.Bridges[i];
				List<INodeBridgeConnection> nodeBridgeConnections = new List<INodeBridgeConnection>();

				for (int j = 0; j < bridge.Connections.Length; j++)
					if (bridge.Connections[j].Node != node)
						nodeBridgeConnections.Add(bridge.Connections[j]);

				bridge.Connections = nodeBridgeConnections.ToArray();
			}

			List<INode> nodes = new List<INode>();
			for (int i = 0; i < NodeWeb.Nodes.Length; i++)
				if (NodeWeb.Nodes[i] != node)
					nodes.Add(NodeWeb.Nodes[i]);
			NodeWeb.Nodes = nodes.ToArray();
			AssetDatabase.SaveAssets();
		}

		void OnClickRemoveBridge(INodeBridge bridge)
		{
			for (int i = 0; i < bridge.Connections.Length; i++)
			{
				INodeBridgeConnection connection = bridge.Connections[i];
				List<INodeBridge> nodeBridges = new List<INodeBridge>();

				for (int j = 0; j < connection.Node.Bridges.Length; j++)
					if (connection.Node.Bridges[j] != bridge)
						nodeBridges.Add(connection.Node.Bridges[j]);

				connection.Node.Bridges = nodeBridges.ToArray();
			}

			List<INodeBridge> bridges = new List<INodeBridge>();
			for (int i = 0; i < NodeWeb.Bridges.Length; i++)
				if (NodeWeb.Bridges[i] != bridge)
					bridges.Add(NodeWeb.Bridges[i]);
			NodeWeb.Bridges = bridges.ToArray();
			AssetDatabase.SaveAssets();
		}
	}
}
