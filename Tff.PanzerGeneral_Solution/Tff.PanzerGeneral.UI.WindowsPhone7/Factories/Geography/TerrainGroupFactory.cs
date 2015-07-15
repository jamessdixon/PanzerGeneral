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
    public class TerrainGroupFactory
    {
        public List<TerrainGroup> TerrainGroup { get; private set; }

        public TerrainGroupFactory()
        {
            TerrainGroup = new List<TerrainGroup>();

            Uri uri = new Uri(Constants.TerrainGroupDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("TerrainGroup")
                       select wz;

            TerrainGroup terrainGroup = null;
            foreach (var d in data)
            {
                terrainGroup = new TerrainGroup();
                terrainGroup.TerrainGroupId = (Int32)d.Element("TerrainGroupId");
                terrainGroup.TerrainGroupDescription = (String)d.Element("TerrainGroupDescription");

                TerrainGroup.Add(terrainGroup);
            }
        }

        public TerrainGroup GetTerrainGroup(int terrainGroupId)
        {
            TerrainGroup terrainGroup = (from ec in this.TerrainGroup
                                             where ec.TerrainGroupId == terrainGroupId
                                            select ec).First();

            return terrainGroup;
        }
    }
}
