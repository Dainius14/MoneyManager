using System;

namespace MoneyManager.Client.State
{
    public class Store<TState> : IStore<TState>
    {
        private readonly Reducer<TState> _rootReducer;

        public TState State { get; private set; }
        public event EventHandler<TState>? StateChanged;

        public Store(Reducer<TState> rootReducer, TState initialState)
        {
            _rootReducer = rootReducer;

            State = initialState;
        }


        public void Dispath(object action)
        {
            State = _rootReducer(State, action);
            StateChanged?.Invoke(this, State);
        }
    }
}
