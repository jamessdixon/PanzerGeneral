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
using System.Collections.Generic;
using Tff.Panzer.Models;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Linq;
using System.Windows.Media.Imaging;
using Tff.Panzer.Models.Geography;

namespace Tff.Panzer.Factories
{
    public class NationFactory
    {
        public List<Nation> Nations { get; private set; }
        public SideFactory SideFactory { get; private set; }

        public NationFactory()
        {
            Nations = new List<Nation>();
            SideFactory = new SideFactory();

            Uri uri = new Uri(Constants.NationDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from f in applicationXml.Descendants("Nation")
                       select f;

            Nation Nation = null;
            foreach (var d in data)
            {
                Nation = new Nation();
                Nation.NationId = (Int32)d.Element("NationId");
                Nation.NationDescription = (String)d.Element("NationDescription");
                Nation.Side = SideFactory.GetSide((Int32)d.Element("SideId"));
                Nation.ImageXCoordinate = (Int32)d.Element("ImageXCoordinate");
                Nation.ImageYCoordinate = (Int32)d.Element("ImageYCoordinate");
                Nations.Add(Nation);
            }
        }

        public Nation GetNation(int NationId)
        {
            Nation Nation = null;
            if (NationId != -1)
            {
                Nation = (from c in this.Nations
                           where c.NationId == NationId
                           select c).FirstOrDefault();
            }
            return Nation;
        }

    }
}
