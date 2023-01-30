﻿using System.Reflection;
using CasaEngineCommon.Design;
using CasaEngineCommon.Logger;
using System.Xml;

namespace CasaEngine.Project
{
    public class PackageItemStreamable
        : PackageItem
    {

        private IPackageable m_Item;

        private long m_FilePosition; //use for binary loading
        private readonly string m_XmlPath; //use for xml loading
        private Type m_ItemType;



        public IPackageable Item => m_Item;

        public string ClassName
        {
            get;
            private set;
        }

        public Type ItemType
        {
            get
            {
                if (m_ItemType == null)
                {
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (Assembly a in assemblies)
                    {
                        m_ItemType = a.GetType(ClassName, false, true);

                        if (m_ItemType != null)
                        {
                            break;
                        }
                    }

                    if (m_ItemType == null)
                    {
                        throw new Exception("Can't retrieve the type " + ClassName);
                    }
                }

                return m_ItemType;
            }
        }



        public PackageItemStreamable(Package package_, int id_, string name_, string className_, IPackageable item_, long filePosition_ = -1)
            : base(package_, id_, name_)
        {
            ClassName = className_;
            m_XmlPath = "Project/Packages/Package[name='" + Package.Name + "']/PackageItem[id='" + ID + "']";
            m_FilePosition = filePosition_;
            m_Item = item_;
        }



        public override T LoadItem<T>()
        {
            //throw new NotImplementedException();

            if (m_Item == null)
            {
                try
                {
                    //if (XML)
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(Package.PackageFileName);
                    XmlElement el = (XmlElement)xmlDoc.SelectSingleNode(m_XmlPath);
                    //else (Binary)

                    m_Item = (IPackageable)Activator.CreateInstance(ItemType,
                    new object[] { el, SaveOption.Editor });
                }
                catch (System.Exception ex)
                {
                    LogManager.Instance.WriteException(ex);
                }
            }

            if ((m_Item is T) == false)
            {
                throw new InvalidOperationException("PackageItemStreamable : item is not a '" + typeof(T).Name + "' object.");
            }

            return (T)m_Item;
        }

    }
}
