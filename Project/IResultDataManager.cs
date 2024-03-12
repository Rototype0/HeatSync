namespace HeatItOn
{
    public interface IResultDataManager
    {
        public ResultData ReadResultData(string data);
        public void WriteResultData(ResultData resultData);
    }
}