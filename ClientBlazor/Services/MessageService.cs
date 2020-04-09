using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace MoneyManager.Client.Services
{
    public class MessageService
    {
        public event Action? MessagesStateChanged;

        public IList<RenderFragment> Messages => _messageOptions.Select(opt => opt.Message).ToList();

        private IList<MessageOptions> _messageOptions = new List<MessageOptions>();

        private NavigationManager _navManager;


        public MessageService(NavigationManager navigationManager)
        {
            _navManager = navigationManager;

            _navManager.LocationChanged += HandleLocationChanged;
        }

        public void Show(RenderFragment message, bool hideOnLocationChanged = true, int? hideAfterMs = null)
        {
            var msgOptions = new MessageOptions(message, hideOnLocationChanged, hideAfterMs);
            _messageOptions.Add(msgOptions);

            if (hideAfterMs != null)
            {
                var timer = new Timer();
                timer.Interval = (int)hideAfterMs;
                timer.Elapsed += (object sender, ElapsedEventArgs e) => HandleMessageTimerElapsed(timer, msgOptions);
                timer.Start();
            }

            MessagesStateChanged?.Invoke();
        }

        private void HandleMessageTimerElapsed(Timer timer, MessageOptions msgOptions)
        {
            timer.Stop();
            timer.Dispose();

            _messageOptions.Remove(msgOptions);
            MessagesStateChanged?.Invoke();
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            _messageOptions = _messageOptions.Where(opt => !opt.HideOnNavigationChanged).ToList();
            MessagesStateChanged?.Invoke();
        }


        private class MessageOptions
        {
            public RenderFragment Message { get; private set; }

            public bool HideOnNavigationChanged { get; private set; } = true;

            public int? HideAfterMs { get; private set; } = null;

            public MessageOptions(RenderFragment message, bool hideOnNavigationChanged, int? hideAfterMs)
            {
                Message = message;
                HideOnNavigationChanged = hideOnNavigationChanged;
                HideAfterMs = hideAfterMs;
            }
        }
    }
}
