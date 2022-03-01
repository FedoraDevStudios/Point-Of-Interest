namespace FedoraDev.PointOfInterest
{
	public interface INodeBridgeConnection : IProduce<INodeBridgeConnection>
	{
        INode Node { get; set; }
        float Distance { get; set; }
	}
}
