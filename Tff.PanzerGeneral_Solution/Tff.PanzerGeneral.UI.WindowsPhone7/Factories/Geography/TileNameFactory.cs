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
    public class TileNameFactory
    {
        public List<TileName> TileNames { get; private set; }

        public TileNameFactory()
        {
            TileNames = new List<TileName>();

            Uri uri = new Uri(Constants.TileNameDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("TileName")
                       select t;

            TileName tileName = null;
            foreach (var d in data)
            {
                tileName = new TileName();
                tileName.TileNameId = (Int32)d.Element("TileNameId");
                tileName.TileNameDescription = (String)d.Element("TileDescription");
                TileNames.Add(tileName);
            }
        }

        public TileName GetTileName(int tileNameId) 
        {
            TileName tileName = (from tn in this.TileNames
                      where tn.TileNameId == tileNameId
                      select tn).First();

            return tileName;
        }


    }
}
