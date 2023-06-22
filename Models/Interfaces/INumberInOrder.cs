namespace Models.Interfaces;

public interface INumberInOrder
{
    long Order { get; }
    void SetOrder(long order);
}