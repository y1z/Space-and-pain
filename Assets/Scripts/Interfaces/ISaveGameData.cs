
namespace interfaces
{
    public interface ISaveGameData
    {
        public string getSaveData();

        public string getMetaData();

        public string[] getMetaDataAndSaveData()
        {
            return new string[2] { getMetaData(), getSaveData() };
        }
    }

}
