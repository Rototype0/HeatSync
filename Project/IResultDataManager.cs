namespace HeatSync
{
    public interface IResultDataManager
    {
        public List<ResultData> ReadResultData(string fileName);
        public void WriteResultData(List<ResultData> resultData, string fileName);
    }
}