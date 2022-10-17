namespace AddressableAssets.Loaders
{
    public interface IAssetsKeyLoader<TAsset> : IAssetsLoader<string, TAsset> where TAsset : UnityEngine.Object { }
}