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
using System.Xml.Linq;
using System.Linq;
using System.Windows.Resources;
using Tff.Panzer.Models.Army;

namespace Tff.Panzer.Factories.Army
{
    public class TargetTypeFactory
    {
        public List<TargetType> TargetTypes { get; private set; }

        public TargetTypeFactory()
        {
            TargetTypes = new List<TargetType>();

            Uri uri = new Uri(Constants.TargetTypeDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("TargetType")
                       select t;

            TargetType targetType = null;
            foreach (var d in data)
            {
                targetType = new TargetType();
                targetType.TargetTypeId = (Int32)d.Element("TargetTypeId");
                targetType.TargetTypeDescription = (String)d.Element("TargetTypeDescription");
                TargetTypes.Add(targetType);
            }
        }

        public TargetType GetTargetType(int targetTypeId) 
        {
            TargetType targetType = (from mt in this.TargetTypes
                                        where mt.TargetTypeId == targetTypeId
                                        select mt).First();
            return targetType;
        }
    }
}
