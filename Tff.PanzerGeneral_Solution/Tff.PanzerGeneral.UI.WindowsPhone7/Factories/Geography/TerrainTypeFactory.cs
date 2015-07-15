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

namespace Tff.Panzer.Factories.Geography
{
    public class TerrainTypeFactory
    {

        public List<TerrainType> TarrainTypes { get; private set; }
        public TerrainGroupFactory TerrainGroupFactory { get; private set; }

        public TerrainTypeFactory()
        {
            TarrainTypes = new List<TerrainType>();
            TerrainGroupFactory = new TerrainGroupFactory();

            Uri uri = new Uri(Constants.TerrainTypeDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("TerrainType")
                       select t;

            TerrainType terrainType = null;
            foreach (var d in data)
            {
                terrainType = new TerrainType();
                terrainType.TerrainTypeId = (Int32)d.Element("TerrainTypeId");
                terrainType.TerrainTypeDescription = (String)d.Element("TerrainTypeDescription");
                terrainType.InitiativeCap = (Int32)d.Element("InitiativeCap");
                terrainType.TerrainGroup = TerrainGroupFactory.GetTerrainGroup((Int32)d.Element("TerrainGroupId"));
                TarrainTypes.Add(terrainType);
            }
        }

        public TerrainType GetTerrainType(int terrainTypeId) 
        {
            TerrainType terrainType = (from tt in this.TarrainTypes
                      where tt.TerrainTypeId == terrainTypeId
                      select tt).First();

            return Clone(terrainType);
        }

        private TerrainType Clone(TerrainType terrainType)
        {
            TerrainType returnValue = new TerrainType();
            returnValue.ImageXCoordinate = terrainType.ImageXCoordinate;
            returnValue.ImageXScale = terrainType.ImageXScale;
            returnValue.ImageYCoordinate = terrainType.ImageYCoordinate;
            returnValue.ImageYScale = terrainType.ImageYScale;
            returnValue.TerrainTypeDescription = terrainType.TerrainTypeDescription;
            returnValue.TerrainTypeId = terrainType.TerrainTypeId;
            returnValue.TerrainGroup = terrainType.TerrainGroup;
            return returnValue;
        }
    }
}
