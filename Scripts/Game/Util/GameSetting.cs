using System;
using System.Collections.Generic;
using FrameWork;

namespace Game
{
    public class GameSetting : Singleton<GameSetting>
    {
        private static readonly string SAVE_FILE = "setting";
        public string serverUrl = "127.0.0.1";
        public int port = 8001;
        public void Load()
        {
            Utils.SetByJsonFile(Instance, SAVE_FILE);
        }
    }
}
