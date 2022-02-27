using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	[CreateAssetMenu(fileName = "New Hierarchy", menuName = "POI/Scriptable Hierarchy")]
	public class ScriptableHierarchy : SerializedScriptableObject, IHierarchy
	{
		public IHierarchyPiece Root => _hierarchy.Root;

		[SerializeField, InlineEditor] IHierarchy _hierarchy;

		IHierarchyPiece[] IHierarchy.GetPath(IHierarchyPiece from, IHierarchyPiece to) => _hierarchy.GetPath(from, to);
		[Button] void AssignParents() => Root.AssignParents(null);
	}
}
