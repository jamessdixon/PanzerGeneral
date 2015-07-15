using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Tff.Panzer.Models.Geography;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Collections.Generic;



namespace Tff.Panzer.Factories.Geography
{
    public class TerrainFactory
    {
        public List<Terrain> Terrains { get; private set; }
        public TerrainTypeFactory TerrainTypeFactory { get; private set; }

        public TerrainFactory()
        {
            Terrains = new List<Terrain>();
            TerrainTypeFactory = new TerrainTypeFactory();
            PopulateTerrainData();
        }

        private void PopulateTerrainData()
        {
            Uri uri = new Uri(Constants.TerrainDataPath, UriKind.Relative);
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            XElement applicationXml = XElement.Load(xmlStream.Stream);
            var data = (from t in applicationXml.Descendants("Terrain")
                        select t);

            foreach (var d in data)
            {
                Terrain terrain = new Terrain();
                terrain.TerrainId = (Int32)d.Element("TerrainId");
                terrain.TerrainType = this.TerrainTypeFactory.GetTerrainType((Int32)d.Element("TerrainTypeId"));
                terrain.ImageXCoordinate = (Int32)d.Element("ImageXCoordinate");
                terrain.ImageYCoordinate = (Int32)d.Element("ImageYCoordinate");
                terrain.RiverInd = (bool)d.Element("RiverInd");
                terrain.RoadInd = (bool)d.Element("RoadInd");
                Terrains.Add(terrain);
            }
        
        }

        public Terrain GetTerrain(int terrainId)
        {
            return (from t in Terrains
                        where t.TerrainId == terrainId
                        select t).FirstOrDefault();
        }
    }
}
