namespace FedoraDev.PointOfInterest
{
    public interface IHierarchy
    {
        IHierarchyPiece Root { get; }

        IHierarchyPiece[] GetPath(IHierarchyPiece from, IHierarchyPiece to);
    }
}
