namespace MoneyManager.Client.State
{
    public interface IReducer<TState>
    {
        public TState Reduce(object action, TState state);
    }
}
