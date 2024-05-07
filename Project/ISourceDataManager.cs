namespace HeatSync
{
    public interface ISourceDataManager
    {
        public List<SourceData> ReadSourceData(string fileName);
        public Task<List<SourceData>> ReadAPISourceData(string fileName);
        public void WriteSourceData(List<SourceData> sourceData, string fileName);
    }
}
