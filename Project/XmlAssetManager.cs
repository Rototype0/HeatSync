using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HeatSync
{
    public class Boiler
    {
        [XmlElement("Name")]
        public string? Name { get; set; }

        [XmlElement("ImagePath")]
        public string? ImagePath { get; set; }

        [XmlElement("MaxHeat")]
        public decimal MaxHeat { get; set; }

        [XmlElement("MaxElectricity")]
        public decimal MaxElectricity { get; set; }

        [XmlElement("ProductionCosts")]
        public int ProductionCosts { get; set; }

        [XmlElement("CO2Emissions")]
        public int CO2Emissions { get; set; }

        [XmlElement("GasConsumption")]
        public decimal GasConsumption { get; set; }

    }


    /*[Serializable()]
    [System.Xml.Serialization.XmlRoot("Heating")]
    public class Boilers
    {
        [XmlArray("Boilers")]
        [XmlArrayItem("Boiler", typeof(Boiler))]
        public Boiler[] Boiler { get; set; }
    }*/

    public class XmlAssetManager : IAssetManager
    {
        private static TValue? LoadXmlData<TValue>(string data)
        {
            try
            {
                // A FileStream is needed to read the XML document.
                FileStream fs = new(data, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);

                XmlSerializer serializer = new(typeof(TValue));
                TValue xmlData;

                // Use the Deserialize method to restore the object's state.
                xmlData = (TValue)serializer.Deserialize(reader);
                fs.Close();

                return xmlData;
            }
            catch (DirectoryNotFoundException)
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
                productionUnits.Add(LoadProductionUnitData(File.ReadAllText(prodUnitPath)));
            
            return productionUnits;
        }
    }
}

        //////////////////////

/*string[] prodUnitPaths = Directory.GetFiles("StaticAssets\ProductionUnits", "*.xml");
            foreach (string prodUnitPath in prodUnitPaths)
                productionUnits.Add(LoadProductionUnitData(File.ReadAllText(prodUnitPath)));

            return productionUnits;
        }
    }
}
*/