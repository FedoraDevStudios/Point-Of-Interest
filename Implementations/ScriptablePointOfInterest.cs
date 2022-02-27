using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[HideMonoScript]
	[CreateAssetMenu(fileName = "New Point of Interest", menuName = "POI/Point of Interest")]
	public class ScriptablePointOfInterest : SerializedScriptableObject, IPointOfInterest, IHierarchyPiece
	{
		public IPointOfInterest PointOfInterest => _pointOfInterest;
		public IHierarchyPiece Parent => _parent;
		public IHierarchyPiece[] LocalHierarchy => _localHierarchy;

		[SerializeField] IPointOfInterest _pointOfInterest;
		[SerializeField, InlineEditor] IHierarchyPiece[] _localHierarchy = new IHierarchyPiece[0];

		[SerializeField, ReadOnly] IHierarchyPiece _parent;

		public void AssignParents(IHierarchyPiece parent)
		{
#if UNITY_EDITOR
			_parent = parent;

			for (int i = 0; i < LocalHierarchy?.Length; i++)
				LocalHierarchy[i].AssignParents(this);

			UnityEditor.EditorUtility.SetDirty(this);
#endif
		}

		public void DrawGizmos(Transform transform) => PointOfInterest?.DrawGizmos(transform);
	}
}
