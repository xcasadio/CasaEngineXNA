using Microsoft.Xna.Framework.Graphics;

namespace CasaEngine.Framework.Assets.Loaders
{
    internal class Texture2DLoader : IAssetLoader
    {
        public object LoadAsset(string fileName, GraphicsDevice device)
        {
            return Texture2D.FromStream(device, new FileStream(fileName, FileMode.Open));
        }
    }
}
