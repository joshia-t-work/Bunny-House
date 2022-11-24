using UnityEngine;
using BunnyHouse.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BunnyHouse.Core
{
    public class DataSystem : MonoSingleton
    {
        private const string FOLDER_DATA_PATH = "/Saves/";
        public static GameData GameData;
        public static SettingData SettingData;

        public override void MonoAwake()
        {
            GameData = LoadGameData<GameData>(GameData.FileName);
            if (GameData == null)
            {
                GameData = new GameData();
            }
            SettingData = LoadGameData<SettingData>(SettingData.FileName);
            if (SettingData == null)
            {
                SettingData = new SettingData();
            }
        }

        public override void MonoOnApplicationPause()
        {
            GameData.OnPause();
        }
        public static void NewGame()
        {
            GameData = new GameData();
            SettingData = new SettingData();
            SaveGame();
            SceneSystem.LoadScene(0);
        }

        public static void SaveGame()
        {
            GameData.OnPause();
            SaveGameData(GameData);
        }

        public static void SaveSettings()
        {
            SaveGameData(SettingData);
        }




        private static void ensureFoldersExist()
        {
            if (!Directory.Exists(Path(FOLDER_DATA_PATH)))
            {
                Directory.CreateDirectory(Path(FOLDER_DATA_PATH));
            }
        }

        /// <summary>
        /// Saves the IData object into a file located in Saves folder
        /// </summary>
        /// <param name="data">Any serializable object</param>
        /// <returns>true if success</returns>
        private static bool SaveGameData(IData data)
        {
            return SaveGameData(data, Path(FOLDER_DATA_PATH + data.DataFileName));
        }

        /// <summary>
        /// Saves the IData object into a file
        /// </summary>
        /// <param name="data">Any serializable object</param>
        /// <returns>true if success</returns>
        private static bool SaveGameData(IData data, string path)
        {
            ensureFoldersExist();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            bf.Serialize(stream, data);
            stream.Close();
            return true;
        }

        /// <summary>
        /// Loads the file from path into a casted object
        /// </summary>
        /// <param name="dataFileName">Filename located in Saves folder</param>
        /// <returns>Data object or null</returns>
        public static IData LoadGameData<IData>(string dataFileName)
        {
            return LoadGameData<IData>(Path(FOLDER_DATA_PATH), dataFileName);
        }
        /// <summary>
        /// Loads the file from path into a casted object
        /// </summary>
        /// <param name="dataFileName">Filename located in Saves folder</param>
        /// <returns>Data object or null</returns>
        public static IData LoadGameData<IData>(string folderPath = "", string dataFileName = "")
        {
            ensureFoldersExist();
            string fpath = folderPath + dataFileName;
            if (File.Exists(fpath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(fpath, FileMode.Open);

                IData data = (IData)bf.Deserialize(stream);
                stream.Close();
                return data;
            }
            else
            {
                return default(IData);
            }
        }
        private static string Path(string path)
        {
            return Application.persistentDataPath + path;
        }
    }
}