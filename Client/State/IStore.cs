using System;

namespace MoneyManager.Client.State
{
    public interface IStore<TState>
    {
        void Dispath(object action);

        event EventHandler<TState> StateChanged;
    }
}
