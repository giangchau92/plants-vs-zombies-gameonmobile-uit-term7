﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantsVsZombies.GameCore.Level;
using System.IO;
using SCSEngine.Serialization.XmlSerialization;
using SCSEngine.Serialization;

namespace PlantsVsZombies.GameCore
{
    public class PZLevelManager
    {
        private static PZLevelManager _instance = null;
        public static PZLevelManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PZLevelManager();
                return _instance;
            }
        }

        private const string level_config = "Xml/Levels/levels.xml";
        private List<Level.Level> _listLevel = new List<Level.Level>();
        private int _currentLevel = 0;

        public int CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
        }


        private PZLevelManager()
        {
        }

        public void initLevel()
        {
            Stream docs = getXml(level_config);
            IDeserializer deser = XmlSerialization.Instance.Deserialize(docs);

            var levelDers = deser.DeserializeAll("Level");
            foreach (var levelDer in levelDers)
            {
                Level.Level level = new Level.Level();
                level.Deserialize(levelDer);
                _listLevel.Add(level);
            }
        }

        private Stream getXml(string config_url)
        {
            return File.Open(config_url, FileMode.Open, FileAccess.Read, FileShare.None);
        }

        public Level.Level GetLevel()
        {
            return _listLevel[_currentLevel].Clone();
        }

        public void UnlockLevel()
        {
            _currentLevel++;
        }

        public void ResetGame()
        {
            _currentLevel = 0;
        }

        public bool IsLevelAvailable
        {
            get
            {
                return this.CurrentLevel < this._listLevel.Count;
            }
        }
    }
}
