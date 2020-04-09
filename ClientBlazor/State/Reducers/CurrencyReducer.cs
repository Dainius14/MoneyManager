using MoneyManager.Client.State.Actions;
using MoneyManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyManager.Client.State.Reducers
{
    public class CurrencyReducer
    {
        public static CurrencyState Reduce(CurrencyState state, object action)
        {
            var newState = (CurrencyState)state.Clone();

            switch (action)
            {
                case CurrencyActions.AddRange a:
                    newState.Currencies = state.Currencies.Concat(a.Currencies).ToList();
                    return newState;

                case CurrencyActions.SetProperty a:
                    newState.GetType().GetProperty(a.PropertyName).SetValue(newState, a.NewValue);
                    return newState;

                default:
                    return newState;
            }
        }
    }
}
