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
using Tff.Panzer.Models;

namespace Tff.Panzer.Factories
{
    public class SideFactory
    {
        public List<Side> Sides { get; private set; }

        public SideFactory()
        {
            Sides = new List<Side>();

            Uri uri = new Uri(Constants.SideDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("Side")
                       select wz;

            Side side = null;
            foreach (var d in data)
            {
                side = new Side();
                side.SideId = (Int32)d.Element("SideId");
                side.SideDescription = (String)d.Element("SideDescription");
                Sides.Add(side);
            }
        }

        public Side GetSide(int sideId)
        {
            Side side = (from wz in this.Sides
                                       where wz.SideId == sideId
                                    select wz).First();

            return side;
        }

        public Side GetSideForANation(int nationId)
        {
            Nation nation = Game.NationFactory.GetNation(nationId);
            return nation.Side;
        }


    }
}
