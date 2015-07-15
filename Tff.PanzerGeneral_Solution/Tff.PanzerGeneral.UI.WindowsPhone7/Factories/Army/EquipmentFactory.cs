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
using System.Windows.Media.Imaging;
using Tff.Panzer.Models.Army;
using Microsoft.Xna.Framework.Audio;

namespace Tff.Panzer.Factories.Army
{
    public class EquipmentFactory
    {
        public List<Equipment> Equipments { get; private set; }
        public MovementTypeFactory MovementTypeFactory { get; private set; }
        public EquipmentSubClassFactory EquipmentSubClassFactory { get; private set; }
        public TargetTypeFactory TargetTypeFactory { get; private set; }

        public EquipmentFactory()
        {
            Equipments = new List<Equipment>();
            MovementTypeFactory = new MovementTypeFactory();
            EquipmentSubClassFactory = new EquipmentSubClassFactory();
            TargetTypeFactory = new TargetTypeFactory();

            Uri uri = new Uri(Constants.EquipmentDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from e in applicationXml.Descendants("Equipment")
                       select e;

            Equipment equipment = null;
            foreach (var d in data)
            {
                equipment = new Equipment();
                equipment.EquipmentId = (Int32)d.Element("EquipmentId");
                equipment.EquipmentDescription = (String)d.Element("EquipmentDescription");
                equipment.EquipmentSubClass = EquipmentSubClassFactory.GetEquipmentSubClass((Int32)d.Element("EquipmentSubClassId"));
                equipment.ImageXCoordinate = (Int32)d.Element("ImageXCoordinate");
                equipment.ImageYCoordinate = (Int32)d.Element("ImageYCoordinate");
                equipment.StackedImageXCoordinate = (Int32)d.Element("StackedImageXCoordinate");
                equipment.StackedImageYCoordinate = (Int32)d.Element("StackedImageYCoordinate");
                equipment.StartService = new DateTime((Int32)d.Element("Year") + 1900, (Int32)d.Element("Month"), 1);
                equipment.EndService = new DateTime((Int32)d.Element("LastYear") + 1900, (Int32)d.Element("Month"), 1);
                equipment.Nation = Game.NationFactory.GetNation((Int32)d.Element("NationId"));
                equipment.MovementType = this.MovementTypeFactory.GetMovementType((Int32)d.Element("MovementTypeId"));
                equipment.BaseMovement = (Int32)d.Element("Movement");
                equipment.BaseStrength = 10;
                equipment.Initative = (Int32)d.Element("Initiative");
                equipment.UnitCost = (Int32)d.Element("Cost");
                equipment.Spotting = (Int32)d.Element("Spotting");
                equipment.IconId = (Int32)d.Element("Icon");
                equipment.StackedIconId = (Int32)d.Element("StackedIcon");
                equipment.TargetType = this.TargetTypeFactory.GetTargetType((Int32)d.Element("TargetTypeId"));
                equipment.SoftAttack = (Int32)d.Element("SoftAttack");
                equipment.HardAttack = (Int32)d.Element("HardAttack");
                equipment.AirAttack = (Int32)d.Element("AirAttack");
                equipment.NavalAttack = (Int32)d.Element("NavalAttack");
                equipment.GroundDefense = (Int32)d.Element("GroundDefense");
                equipment.AirDefense = (Int32)d.Element("AirDefense");
                equipment.CloseDefense = (Int32)d.Element("CloseDefense");
                equipment.MaxAmmo = (Int32)d.Element("MaxAmmo");
                equipment.MaxFuel = (Int32)d.Element("MaxFuel");
                equipment.Range = (Int32)d.Element("Range");
                equipment.CanBridgeRivers = (Boolean)d.Element("CanBridgeRivers");
                equipment.JetIndicator = (Boolean)d.Element("Jet");
                equipment.IgnoresEntrenchment = (Boolean)d.Element("IgnoresEntrenchment");
                equipment.CanParadrop = (Boolean)d.Element("CanParadrop");
                equipment.CanHaveAirTransport = (Boolean)d.Element("CanHaveAirTransport");
                equipment.CanHaveSeaTransport = (Boolean)d.Element("CanHaveSeaTransport");
                equipment.BomberSpecial = (Int32)d.Element("BomberSpecial");
                equipment.CanHaveOrganicTransport = (Boolean)d.Element("CanHaveOrganicTransport");

                Equipments.Add(equipment);
            }
        }

        public Equipment GetEquipment(int equipmentId)
        {
            Equipment equipment = (from e in this.Equipments
                                             where e.EquipmentId == equipmentId
                                            select e).First();

            return equipment;
        }
    }
}
