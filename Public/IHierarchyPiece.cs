using Sirenix.OdinInspector;

namespace FedoraDev.PointOfInterest
{
    public interface IHierarchyPiece
    {
        IHierarchyPiece Parent { get; }
        [InlineEditor]
        IHierarchyPiece[] LocalHierarchy { get; }

        void AssignParents(IHierarchyPiece parent);
    }
}
