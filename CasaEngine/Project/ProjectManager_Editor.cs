﻿using System.Xml;
using CasaEngineCommon.Extension;
using CasaEngineCommon.Design;
using CasaEngine.Game;


namespace CasaEngine.Project
{
    public partial class ProjectManager
    {
        private static readonly uint Version = 1;

        public event EventHandler ProjectLoaded;
        public event EventHandler ProjectClosed;

        public XmlDocument LastXmlDocument
        {
            get;
            set;
        }
        public string ProjectFileOpened
        {
            get;
            set;
        }

        public void Clear()
        {
            Engine.Instance.AssetManager.Clear();
            Engine.Instance.Asset2DManager.Clear();
            Engine.Instance.ExternalToolManager.Clear();
            ProjectFileOpened = null;

            if (ProjectClosed != null)
            {
                ProjectClosed(this, EventArgs.Empty);
            }
        }

        public void CreateProject(string fileName)
        {
#if !DEBUG
            try
            {
#endif

            Clear();
            CreateProjectDirectoryHierarchy(Path.GetDirectoryName(fileName));
            Save(fileName);

#if !DEBUG
            }
            catch (System.Exception e)
            {
                
            }
#endif
        }

        private void CreateProjectDirectoryHierarchy(string path)
        {
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + AssetDirPath);
            //System.IO.Directory.CreateDirectory(path_ + Path.DirectorySeparatorChar + ImageDirPath);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + ExternalToolsDirPath);
            //System.IO.Directory.CreateDirectory(path_ + Path.DirectorySeparatorChar + SoundDirPath);
            //model
            //video
            //System.IO.Directory.CreateDirectory(path_ + Path.DirectorySeparatorChar + PackageDirPath);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameDirPath);
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + GameDirPath + "\\Content");
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + ConfigDirPath);
        }

        public bool Save(string fileName)
        {
            //Si on sauvegarde et qu'il y a deja une sauvegarde
            //On est obligé de mixer le nouveau fichier avec le nouveau
            //Car on ne sauvegarde a chaque fois que le monde courant
            //(on ne peut pas sauvegarder les autres mondes sinon il faudrait les charger)
            //XmlDocument xmlDocLastFile = null;

            //si le fichier existe deja on le charge
            if (string.IsNullOrEmpty(ProjectFileOpened) == false)
            {
                if (File.Exists(fileName))
                {
                    //ProjectFileOpened = fileName_;
                    //TODO
                }
            }

            ProjectFileOpened = fileName;

            /*if (SourceControlManager.Instance.SourceControl.IsValidConnection() == true)
            {
                if (SourceControlManager.Instance.SourceControl.CheckOut(ProjectFileOpened) == false)
                {
                    //return false;
                }
            }*/

            /*if (string.IsNullOrEmpty(_LastProjectFileName) == false )
            {
                xmlDocLastFile = new XmlDocument();
                xmlDocLastFile.Load(_LastProjectFileName);
            }*/

            //nouveau fichier
            var xmlDoc = new XmlDocument();
            var projectNode = xmlDoc.AddRootNode(NodeRootName);
            xmlDoc.AddAttribute(projectNode, "version", Version.ToString());

            var configNode = xmlDoc.CreateElement(NodeConfigName);
            projectNode.AppendChild(configNode);
            Engine.Instance.ProjectConfig.Save(configNode, SaveOption.Editor);

            //liste des mondes
            /*XmlElement worldListNode = xmlDoc.CreateElement("WorldList");
            projectNode.AppendChild(worldListNode);
            foreach (string str in _WorldList)
            {
                XmlElement worldNode = xmlDoc.CreateElementWithText("World", str);
                worldListNode.AppendChild(worldNode);
            }

            //liste screen
            XmlElement screendListNode = xmlDoc.CreateElement("ScreenList");
            projectNode.AppendChild(screendListNode);
            foreach (KeyValuePair<string, string> pair in _ScreenList)
            {
                XmlElement screenNode = xmlDoc.CreateElement("Screen");
                xmlDoc.AddAttribute(screenNode, "key", pair.Key);
                xmlDoc.AddAttribute(screenNode, "value", pair.Value);
                screendListNode.AppendChild(screenNode);
            }*/

            /*XmlElement assetNode = xmlDoc.CreateElement(NodeAsset2DName);
            projectNode.AppendChild(assetNode);
            GameInfo.Instance.Asset2DManager.Save(assetNode, SaveOption.Editor);*/

            /*XmlElement asset3DNode = xmlDoc.CreateElement("Asset3D");
            projectNode.AppendChild(asset3DNode);
            GameInfo.Instance.Asset3DManager.Save(asset3DNode);*/

            /*XmlElement assetListNode = xmlDoc.CreateElement(NodeAssetListName);
            projectNode.AppendChild(assetListNode);
            GameInfo.Instance.AssetManager.Save(assetListNode, SaveOption.Editor);*/

            /*XmlElement objectRegistryNode = xmlDoc.CreateElement(NodeObjectRegistryName);
            projectNode.AppendChild(objectRegistryNode);
            GameInfo.Instance.ObjectRegistry.Save(objectRegistryNode, SaveOption.Editor);*/

            //XmlElement objectManagerNode = xmlDoc.CreateElement(NodeObjectListName);
            //projectNode.AppendChild(objectManagerNode);
            Engine.Instance.ObjectManager.Save(projectNode, SaveOption.Editor);

            xmlDoc.Save(fileName);

            LastXmlDocument = xmlDoc;

            //Monde
            //on ne peut sauvegarder que le monde courant
            //donc a chaque fois on sauve le projet puis on sauve le monde
            //bool res = SaveCurrentWorld();

            //il faut mixer les deux fichiers
            //non necessaire ??
            /*if (xmlDocLastFile != null)
            {
                res = MixOldProjectFileWithNewProjectFile(xmlDocLastFile, xmlDoc);
            }*/

            return true;
        }
    }
}
