namespace MoneyManager.Client.State
{
    public delegate void Dispatcher(object action);
    public delegate TState Reducer<TState>(TState state, object action);
}
