using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public abstract class StorageBase
    {
        private IJSInProcessRuntime _jsRuntime;

        protected virtual string StorageName { get; } = null!;


        public StorageBase(IJSInProcessRuntime jsInProcessRuntime)
        {
            _jsRuntime = jsInProcessRuntime;
        }



        public string Key(int number)
        {
            return _jsRuntime.Invoke<string>("BrowserStorage.key", StorageName, number);
        }

        public async Task<string> KeyAsync(int number)
        {
            return await _jsRuntime.InvokeAsync<string>("BrowserStorage.key", StorageName, number);
        }


        public T GetItem<T>(string key)
        {
            var json = _jsRuntime.Invoke<string>("BrowserStorage.getItem", StorageName, key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json!);
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("BrowserStorage.getItem", StorageName, key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json!);
        }


        public void SetItem<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            _jsRuntime.InvokeVoid("BrowserStorage.setItem", StorageName, key, json);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("BrowserStorage.setItem", StorageName, key, json);
        }


        public void Remove(string key)
        {
            _jsRuntime.InvokeVoid("BrowserStorage.removeItem", StorageName, key);
        }

        public async Task RemoveAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("BrowserStorage.removeItem", StorageName, key);
        }


        public void Clear()
        {
            _jsRuntime.InvokeVoid("BrowserStorage.clear", StorageName);
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
            : base((IJSInProcessRuntime)jsRuntime)
        {
        }
    }

    public class SessionStorage : StorageBase
    {
        protected override string StorageName => "sessionStorage";

        public SessionStorage(IJSRuntime jsRuntime)
            : base((IJSInProcessRuntime)jsRuntime)
        {
        }
    }
}
