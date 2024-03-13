namespace HeatItOn
{
    public interface IResultDataManager
    {
        public List<ResultData> ReadResultData(string filePath);
        public void WriteResultData(List<ResultData> resultData);
    }
}