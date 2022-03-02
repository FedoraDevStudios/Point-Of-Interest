using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[CreateAssetMenu(fileName = "New Node Web", menuName = "POI/Node Web")]
	public class ScriptableNodeWeb : SerializedScriptableObject, INodeWeb
	{
		public string Name => base.name;

		public INode[] Nodes
		{
			get
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
				if (_nodes == null)
					_nodes = new INode[0];
				return _nodes;
			}
			set
			{
				_nodes = value;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		public Vector2 Offset
		{
			get
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
				return _offset;
			}
			set
			{
				_offset = value;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		public INodeBridge[] Bridges
		{
			get
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
				if (_bridges == null)
					_bridges = new INodeBridge[0];
				return _bridges;
			}
			set
			{
				_bridges = value;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
		}

		[SerializeField, ReadOnly] Vector2 _offset;
		[SerializeField, ReadOnly] INode[] _nodes = new INode[0];
		[SerializeField, ReadOnly] INodeBridge[] _bridges = new INodeBridge[0];

		public IPointOfInterest[] GetShortestPath(IPointOfInterest start, IPointOfInterest end, params IPointOfInterest[] mustVisitPOIs)
		{
			List<IPointOfInterest> pois = new List<IPointOfInterest>();


			return pois.ToArray();
		}

		#region Produce In Editor
#if UNITY_EDITOR
		public static INodeWeb ProduceInEditor(string location)
		{
			string[] locationArray = location.Split('/');
			string folder = "";
			string filename = locationArray[locationArray.Length - 1];


			for (int i = 0; i < locationArray.Length - 1; i++)
			{
				if (locationArray[i] == string.Empty)
					continue;
				folder += $"{locationArray[i]}/";
			}

			ScriptableNodeWeb nodeWeb = CreateInstance<ScriptableNodeWeb>();

			if (!UnityEditor.AssetDatabase.IsValidFolder(folder))
				System.IO.Directory.CreateDirectory(folder);
			if (System.IO.File.Exists($"{folder}/{filename}"))
			{
				Debug.Log($"There's already a '{filename}' at this location!");
				return null;
			}

			UnityEditor.AssetDatabase.CreateAsset(nodeWeb, $"{folder}/{filename}");
			UnityEditor.AssetDatabase.SaveAssets();

			return nodeWeb;
		}
#endif
		#endregion
	}
}
