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
    public class EquipmentLossCalculationGroupFactory
    {
        public List<EquipmentLossCalculationGroup> EquipmentLossCalculationGroups { get; private set; }

        public EquipmentLossCalculationGroupFactory()
        {
            EquipmentLossCalculationGroups = new List<EquipmentLossCalculationGroup>();

            Uri uri = new Uri(Constants.EquipmentLossCalculationGroupDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from t in applicationXml.Descendants("EquipmentLossCalculationGroup")
                       select t;

            EquipmentLossCalculationGroup equipmentLossCalculationGroup = null;
            foreach (var d in data)
            {
                equipmentLossCalculationGroup = new EquipmentLossCalculationGroup();
                equipmentLossCalculationGroup.EquipmentLossCalculationGroupId = (Int32)d.Element("EquipmentLossCalculationGroupId");
                equipmentLossCalculationGroup.EquipmentLossCalculationGroupDescription = (String)d.Element("EquipmentLossCalculationGroupDescription");
                EquipmentLossCalculationGroups.Add(equipmentLossCalculationGroup);
            }
        }

        public EquipmentLossCalculationGroup GetEquipmentLossCalculationGroup(int equipmentLossCalculationGroupId) 
        {
            EquipmentLossCalculationGroup equipmentLossCalculationGroup = (from mt in this.EquipmentLossCalculationGroups
                                        where mt.EquipmentLossCalculationGroupId == equipmentLossCalculationGroupId
                                        select mt).First();
            return equipmentLossCalculationGroup;
        }
    }
}
