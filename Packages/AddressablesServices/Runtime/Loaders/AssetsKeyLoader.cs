using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressableAssets.Loaders
{
    public sealed class AssetsKeyLoader<TAsset> : IAssetsKeyLoader<TAsset> where TAsset : Object
    {
        private readonly Dictionary<object, AsyncOperationHandle<TAsset>> _operationHandles;

        #if UNITY_2020_3_OR_NEWER
        [UnityEngine.Scripting.RequiredMember]
        #endif
        public AssetsKeyLoader() =>
            _operationHandles = new Dictionary<object, AsyncOperationHandle<TAsset>>();

        public UniTask PreloadAssetAsync(string key) => LoadAssetAsync(key);

        public async UniTask<TAsset> LoadAssetAsync(string key)
        {
            var handle = GetLoadHandle(key);

            try
            {
                return await handle;
            }
            catch
            {
                Addressables.Release(handle);
                _operationHandles.Remove(key);
                throw;
            }
        }

        public bool IsAssetLoaded(string key)
        {
            if (_operationHandles.TryGetValue(key, out var handle))
            {
                return handle.IsDone;
            }

            return false;
        }

        public TAsset GetAsset(string key) =>
            _operationHandles[key].Result;

        public bool TryGetAsset(string key, out TAsset asset)
        {
            asset = default;

            if (_operationHandles.TryGetValue(key, out var handle))
            {
                if (handle.IsDone)
                {
                    asset = handle.Result;
                }

                return handle.IsDone;
            }

            return false;
        }

        public void UnloadAsset(string key)
        {
            if (_operationHandles.TryGetValue(key, out var handle))
            {
                Addressables.Release(handle);
                _operationHandles.Remove(key);
            }
        }

        public void UnloadAllAssets()
        {
            foreach (var handle in _operationHandles.Values)
            {
                Addressables.Release(handle);
            }

            _operationHandles.Clear();
        }

        private AsyncOperationHandle<TAsset> GetLoadHandle(string key)
        {
            if (!_operationHandles.TryGetValue(key, out var handle))
            {
                handle = Addressables.LoadAssetAsync<TAsset>(key);
                _operationHandles.Add(key, handle);
            }

            return handle;
        }
    }
}