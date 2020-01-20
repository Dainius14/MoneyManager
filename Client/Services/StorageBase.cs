using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public abstract class StorageBase
    {
        private IJSRuntime _jsRuntime;

        protected virtual string StorageName { get; } = null!;

        public StorageBase(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        public async Task<string> KeyAsync(int number)
        {
            return await _jsRuntime.InvokeAsync<string>("BrowserStorage.key", StorageName, number);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("BrowserStorage.getItem", StorageName, key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json!);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("BrowserStorage.setItem", StorageName, key, json);
        }

        public async Task RemoveAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("BrowserStorage.removeItem", StorageName, key);
        }

        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("BrowserStorage.clear", StorageName);
        }

    }

    public class LocalStorage : StorageBase
    {
        protected override string StorageName => "localStorage";

        public LocalStorage(IJSRuntime jsRuntime)
            : base(jsRuntime)
        {
        }
    }

    public class SessionStorage : StorageBase
    {
        protected override string StorageName => "sessionStorage";

        public SessionStorage(IJSRuntime jsRuntime)
            : base(jsRuntime)
        {
        }
    }
}
