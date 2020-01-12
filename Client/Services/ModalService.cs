using System;

namespace MoneyManager.Client.Services
{
    public class ModalService
    {
        public event Action<object>? OnShow;

        public event Action<bool> OnSetLoading;

        public event Action? OnClose;

        public void Show(object modalOptions)
        {
            OnShow?.Invoke(modalOptions);
        }
        public void Close()
        {
            OnClose?.Invoke();
        }

        public void SetLoading(bool value)
        {
            OnSetLoading?.Invoke(value);
        }
    }
}
