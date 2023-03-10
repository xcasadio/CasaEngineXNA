using System.Xml;
using CasaEngine.Core.Design;
using CasaEngine.Core.Extension;

namespace CasaEngine.Framework.FrontEnd.Screen
{
    public class ScreenManager
    {
        private readonly List<UiScreen> _screens = new();

        public void Load(XmlElement el, SaveOption opt)
        {
            var version = int.Parse(el.Attributes["version"].Value);

            var nodeList = el.SelectSingleNode("ScreenList");

            _screens.Clear();

            foreach (XmlNode node in nodeList)
            {
                _screens.Add(new UiScreen((XmlElement)node, opt));
            }
        }

#if EDITOR
        private static readonly int Version = 1;

        public bool IsValidName(string name)
        {
            foreach (var screen in _screens)
            {
                if (screen.Name.Equals(name))
                {
                    return false;
                }
            }

            return true;
        }

        public UiScreen GetScreen(string name)
        {
            foreach (var screen in _screens)
            {
                if (screen.Name.Equals(name))
                {
                    return screen;
                }
            }

            throw new InvalidOperationException("Screenmanager.GetScreen() : can't find the screen " + name);
        }

        public void AddScreen(UiScreen screen)
        {
            _screens.Add(screen);
        }

        public void RemoveScreen(UiScreen screen)
        {
            _screens.Remove(screen);
        }

        public void RemoveScreen(string name)
        {
            UiScreen s = null;

            foreach (var screen in _screens)
            {
                if (screen.Name.Equals(name))
                {
                    RemoveScreen(s);
                    return;
                }
            }

            throw new InvalidOperationException("Screenmanager.RemoveScreen() : can't find the screen " + name);
        }

        public void Save(XmlElement el, SaveOption opt)
        {
            el.OwnerDocument.AddAttribute(el, "version", Version.ToString());

            var nodeList = el.OwnerDocument.CreateElement("ScreenList");
            el.AppendChild(nodeList);

            foreach (var screen in _screens)
            {
                var node = el.OwnerDocument.CreateElement("Screen");
                nodeList.AppendChild(node);

                screen.Save(node, opt);
            }
        }

        public void Save(BinaryWriter bw, SaveOption opt)
        {
            bw.Write(Version);
            bw.Write(_screens.Count);

            foreach (var screen in _screens)
            {
                screen.Save(bw, opt);
            }
        }
#endif
    }
}
