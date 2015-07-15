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
using Tff.Panzer.Factories.Army;
using Tff.Panzer.Factories.Geography;

namespace Tff.Panzer.Factories.Movement
{
    public class MovementCostModifierFactory
    {
        public List<MovementCostModifier> MovementCostModifiers { get; private set; }
        public MovementTypeFactory MovementTypeFactory { get; private set; }
        public TerrainConditionFactory TerrainConditionFactory { get; private set; }

        public MovementCostModifierFactory()
        {
            MovementCostModifiers = new List<MovementCostModifier>();
            MovementTypeFactory = new MovementTypeFactory();
            TerrainConditionFactory = new TerrainConditionFactory();


            Uri uri = new Uri(Constants.MovementCostModifierDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("MovementCostModifier")
                       select t;

            MovementCostModifier movementCostModifier = null;
            foreach (var d in data)
            {
                movementCostModifier = new MovementCostModifier();
                movementCostModifier.MovementCostModifierId = (Int32)d.Element("MovementCostModifierId");
                movementCostModifier.MovementType = MovementTypeFactory.GetMovementType((Int32)d.Element("MovementTypeId"));
                movementCostModifier.TerrainCondition = TerrainConditionFactory.GetTerrainCondition((Int32)d.Element("TerrainConditionId"));
                movementCostModifier.RiverPoints = (Int32)d.Element("RiverPoints");
                movementCostModifier.RoadPoints = (Int32)d.Element("RoadPoints");
                MovementCostModifiers.Add(movementCostModifier);
            }
        }

        public MovementCostModifier GetMovementCostModifier(int movementCostModifierId) 
        {
            MovementCostModifier movementCostModifier = (from mt in this.MovementCostModifiers
                                        where mt.MovementCostModifierId == movementCostModifierId
                                        select mt).First();
            return movementCostModifier;
        }
    }
}
