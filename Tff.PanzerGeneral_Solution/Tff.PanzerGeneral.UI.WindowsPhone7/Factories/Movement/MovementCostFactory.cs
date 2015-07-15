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
    public class MovementCostFactory
    {
        public List<MovementCost> MovementCosts { get; private set; }
        public MovementTypeFactory MovementTypeFactory { get; private set; }
        public TerrainConditionFactory TerrainConditionFactory { get; private set; }
        public TerrainTypeFactory TerrainTypeFactory { get; private set; }

        public MovementCostFactory()
        {
            MovementCosts = new List<MovementCost>();
            MovementTypeFactory = new MovementTypeFactory();
            TerrainConditionFactory = new TerrainConditionFactory();
            TerrainTypeFactory = new TerrainTypeFactory();

            Uri uri = new Uri(Constants.MovementCostDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("MovementCost")
                       select t;

            MovementCost movementCost = null;
            foreach (var d in data)
            {
                movementCost = new MovementCost();
                movementCost.MovementCostId = (Int32)d.Element("MovementCostId");
                movementCost.MovementType = MovementTypeFactory.GetMovementType((Int32)d.Element("MovementTypeId"));
                movementCost.TerrainCondition = TerrainConditionFactory.GetTerrainCondition((Int32)d.Element("TerrainConditionId"));
                movementCost.TerrainType = TerrainTypeFactory.GetTerrainType((Int32)d.Element("TerrainTypeId"));
                movementCost.MovementPoints = (Int32)d.Element("MovementPoints");
                MovementCosts.Add(movementCost);
            }
        }

        public MovementCost GetMovementCost(int movementCostId) 
        {
            MovementCost movementCost = (from mt in this.MovementCosts
                                        where mt.MovementCostId == movementCostId
                                        select mt).First();
            return movementCost;
        }

    }
}
