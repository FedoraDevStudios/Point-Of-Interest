namespace FedoraDev.PointOfInterest
{
    public interface IProduce<T>
    {
        T Produce(IFactory _factory);
    }
}
