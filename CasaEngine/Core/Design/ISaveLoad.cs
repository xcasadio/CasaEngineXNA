using System.Xml;

namespace CasaEngine.Core.Design
{
    public enum SaveOption
    {
        Editor,
        Game
    }

    public interface ISaveLoad
    {
#if EDITOR
        void Save(XmlElement el_, SaveOption option_);
#endif
        void Load(XmlElement el_, SaveOption option_);
    }
}
