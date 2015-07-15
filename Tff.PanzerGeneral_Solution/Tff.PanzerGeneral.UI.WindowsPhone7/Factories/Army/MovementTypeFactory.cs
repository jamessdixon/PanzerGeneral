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
    public class MovementTypeFactory
    {
        public List<MovementType> MovementTypes { get; private set; }

        public MovementTypeFactory()
        {
            MovementTypes = new List<MovementType>();

            Uri uri = new Uri(Constants.MovementTypeDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("MovementType")
                       select t;

            MovementType movementType = null;
            foreach (var d in data)
            {
                movementType = new MovementType();
                movementType.MovementTypeId = (Int32)d.Element("MovementTypeId");
                movementType.MovementTypeDescription = (String)d.Element("MovementTypeDescription");
                movementType.IsMotorized = (Boolean)d.Element("IsMotorized");
                MovementTypes.Add(movementType);
            }
        }

        public MovementType GetMovementType(int movementTypeId) 
        {
            MovementType movementType = (from mt in this.MovementTypes
                                        where mt.MovementTypeId == movementTypeId
                                        select mt).First();
            return movementType;
        }
    }
}
