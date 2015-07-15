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
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using System.Collections.Generic;
using Tff.Panzer.Models.Geography;
using System.Windows.Media.Imaging;

namespace Tff.Panzer.Factories.Geography
{
    public class TerrainConditionFactory
    {
        public List<TerrainCondition> TarrainConditions { get; private set; }

        public TerrainConditionFactory()
        {
            TarrainConditions = new List<TerrainCondition>();

            Uri dataUri = new Uri(Constants.TerrainConditionDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(dataUri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("TerrainCondition")
                       select wz;

            TerrainCondition terrainCondition = null;
            foreach (var d in data)
            {
                terrainCondition = new TerrainCondition();
                terrainCondition.TerrainConditionId = (Int32)d.Element("TerrainConditionId");
                terrainCondition.TerrainConditionDescription = (String)d.Element("TerrainConditionDescription");
                TarrainConditions.Add(terrainCondition);
            }
        }

        public TerrainCondition GetTerrainCondition(int terrainConditionId) 
        {
            TerrainCondition terrainCondition = (from tt in this.TarrainConditions
                      where tt.TerrainConditionId == terrainConditionId
                      select tt).First();

            return terrainCondition;
        }
    }
}
