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
using Tff.Panzer.Models.Army;


namespace Tff.Panzer.Factories.Army
{
    public class EquipmentGroupFactory
    {
        public List<EquipmentGroup> EquipmentGroup { get; private set; }

        public EquipmentGroupFactory()
        {
            EquipmentGroup = new List<EquipmentGroup>();

            Uri uri = new Uri(Constants.EquipmentGroupDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("EquipmentGroup")
                       select wz;

            EquipmentGroup equipmentGroup = null;
            foreach (var d in data)
            {
                equipmentGroup = new EquipmentGroup();
                equipmentGroup.EquipmentGroupId = (Int32)d.Element("EquipmentGroupId");
                equipmentGroup.EquipmentGroupDescription = (String)d.Element("EquipmentGroupDesc");

                EquipmentGroup.Add(equipmentGroup);
            }
        }

        public EquipmentGroup GetEquipmentGroup(int equipmentGroupId)
        {
            EquipmentGroup equipmentGroup = (from ec in this.EquipmentGroup
                                             where ec.EquipmentGroupId == equipmentGroupId
                                            select ec).First();

            return equipmentGroup;
        }

    }
}
