namespace Models.Collections
{
    public interface INumberInOrder
    {
        long Order { get; }
        void SetOrder(long order);
    }
}
