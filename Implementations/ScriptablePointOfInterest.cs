using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[HideMonoScript]
	[CreateAssetMenu(fileName = "New Point of Interest", menuName = "POI/Point of Interest")]
	public class ScriptablePointOfInterest : SerializedScriptableObject, IPointOfInterest, IHierarchyPiece
	{
		public string Name => base.name;
		public IPointOfInterest PointOfInterest => _pointOfInterest;
		public IHierarchyPiece Parent => _parent;
		public IHierarchyPiece[] LocalHierarchy => _localHierarchy;

		[SerializeField] IPointOfInterest _pointOfInterest;
		[SerializeField, InlineEditor] IHierarchyPiece[] _localHierarchy = new IHierarchyPiece[0];

		[SerializeField, ReadOnly] IHierarchyPiece _parent;

		public List<IHierarchyPiece> GetPathTo(IHierarchyPiece to)
		{
			if (to.Name == Name)
				return new List<IHierarchyPiece>() { this };

			for (int i = 0; i < LocalHierarchy.Length; i++)
			{
				List<IHierarchyPiece> pieces = LocalHierarchy[i].GetPathTo(to);
				if (pieces != null)
				{
					pieces.Insert(0, this);
					return pieces;
				}
			}

			return null;
		}

		public void AssignParents(IHierarchyPiece parent)
		{
			_parent = parent;

			for (int i = 0; i < LocalHierarchy?.Length; i++)
				LocalHierarchy[i].AssignParents(this);

#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		public void DrawGizmos(Transform transform) => PointOfInterest?.DrawGizmos(transform);
	}
}
