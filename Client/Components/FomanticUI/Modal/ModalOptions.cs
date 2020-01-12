using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MoneyManager.Client.Components.FomanticUI.Modal
{
    public class ModalOptions
    {
        public string Header { get; set; }

        public string Content { get; set; }

        public Action OnYes { get; set; }

        public Action OnNo { get; set; }

        public ModalOptions(string header, string content, Action onYes, Action onNo)
        {
            Header = header;
            Content = content;
            OnYes = onYes;
            OnNo = onNo;
        }
    }
}
