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
using Tff.Panzer.Models.VictoryCondition;

namespace Tff.Panzer.Factories.VictoryCondition
{
    public class ObjectiveTypeFactory
    {
        public List<ObjectiveType> ObjectiveTypes { get; private set; }

        public ObjectiveTypeFactory()
        {
            ObjectiveTypes = new List<ObjectiveType>();

            Uri uri = new Uri(Constants.ObjectiveTypeDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("ObjectiveType")
                       select t;

            ObjectiveType objectiveType = null;
            foreach (var d in data)
            {
                objectiveType = new ObjectiveType();
                objectiveType.ObjectiveTypeId = (Int32)d.Element("ObjectiveTypeId");
                objectiveType.ObjectiveTypeDescription = (String)d.Element("ObjectiveTypeDescription");
                ObjectiveTypes.Add(objectiveType);
            }
        }

        public ObjectiveType GetObjectiveType(int objectiveTypeId) 
        {
            ObjectiveType objectiveType = (from mt in this.ObjectiveTypes
                                        where mt.ObjectiveTypeId == objectiveTypeId
                                        select mt).First();
            return objectiveType;
        }

    }
}
