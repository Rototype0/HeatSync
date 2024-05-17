using System.Xml;
using System.Xml.Serialization;

namespace HeatSync
{
    public class XmlAssetManager : IAssetManager
    {
        private static TValue? LoadXmlData<TValue>(string data)
        {
            try
            {
                FileStream fs = new(data, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);
                XmlSerializer serializer = new(typeof(TValue));
                TValue? xmlData;
                xmlData = (TValue?)serializer.Deserialize(reader);
                fs.Close();
                return xmlData;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
        }

        public HeatingGrid LoadHeatingGridData(string data)
        {
            return LoadXmlData<HeatingGrid>(data);
        }
        public ProductionUnit LoadProductionUnitData(string data)
        {
            return LoadXmlData<ProductionUnit>(data);
        }

        // Helper method for getting all valid production units quickly.
        public List<ProductionUnit> GetAllProductionUnits()
        {
            List<ProductionUnit> productionUnits = [];

            string[] prodUnitPaths = Directory.GetFiles("StaticAssets\\ProductionUnits", "*.xml");
            
            foreach (string prodUnitPath in prodUnitPaths)
                productionUnits.Add(LoadProductionUnitData(prodUnitPath));
            
            return productionUnits;
        }
    }
}
