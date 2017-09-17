using Redux;

namespace SpotFinder.Redux
{
    public interface IReducer<T>
    {
        T Reduce(T applicationState, IAction action);
    }
}
