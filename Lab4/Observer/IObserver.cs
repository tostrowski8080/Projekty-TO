namespace Lab4.Observer
{
    public interface IObserver<T>
    {
        void OnUpdate(T data);
    }
}