namespace HeatItOn
{
    public interface ISourceDataManager
    {
        public List<SourceData> ReadSourceData(string fileName);
        public Task<byte[]?> GetEnerginetAPIData(string APIParams);
        public void WriteSourceData(List<SourceData> sourceData, string fileName);
    }
}
