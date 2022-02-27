using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class SimpleHierarchy : IHierarchy
	{
		public IHierarchyPiece Root => _root;

		[SerializeField, InlineEditor] IHierarchyPiece _root;

		public IHierarchyPiece[] GetPath(IHierarchyPiece from, IHierarchyPiece to) => throw new System.NotImplementedException();
	}
}
