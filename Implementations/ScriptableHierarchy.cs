using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[CreateAssetMenu(fileName = "New Hierarchy", menuName = "POI/Scriptable Hierarchy")]
	public class ScriptableHierarchy : SerializedScriptableObject, IHierarchy
	{
		public IHierarchyPiece Root => _hierarchy.Root;

		[SerializeField, InlineEditor] IHierarchy _hierarchy;

		public IHierarchyPiece[] GetPath(IHierarchyPiece from, IHierarchyPiece to) => throw new System.NotImplementedException();

		[Button]
		void AssignParents()
		{
			Root.AssignParents(null);
		}
	}
}
