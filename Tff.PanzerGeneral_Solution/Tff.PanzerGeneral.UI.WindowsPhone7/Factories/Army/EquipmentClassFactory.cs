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
    public class EquipmentClassFactory
    {
        public List<EquipmentClass> EquipmentClasses { get; private set; }
        public EquipmentGroupFactory EquipmentGroupFactory { get; private set; }
        public EquipmentLossCalculationGroupFactory EquipmentLossCalculationGroupFactory { get; private set; }

        public EquipmentClassFactory()
        {
            EquipmentClasses = new List<EquipmentClass>();
            EquipmentGroupFactory = new EquipmentGroupFactory();
            EquipmentLossCalculationGroupFactory = new EquipmentLossCalculationGroupFactory();

            Uri uri = new Uri(Constants.EquipmentClassDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("EquipmentClass")
                       select wz;

            EquipmentClass equipmentClass = null;
            foreach (var d in data)
            {
                equipmentClass = new EquipmentClass();
                equipmentClass.EquipmentClassId = (Int32)d.Element("EquipmentClassId");
                equipmentClass.EquipmentClassDescription = (String)d.Element("EquipmentClassDescription");
                equipmentClass.EquipmentGroup = EquipmentGroupFactory.GetEquipmentGroup((Int32)d.Element("EquipmentGroupId"));
                equipmentClass.EntrenchmentRate = (Int32)d.Element("EntrenchmentRate");
                equipmentClass.EquipmentLossCalculationGroup = EquipmentLossCalculationGroupFactory.GetEquipmentLossCalculationGroup((Int32)d.Element("EquipmentLossCalculationGroupId"));
                equipmentClass.CanCaptureHexes = (Boolean)d.Element("CanCaptureHexes");
                EquipmentClasses.Add(equipmentClass);
            }
        }

        public EquipmentClass GetEquipmentClass(int equipmentClassId)
        {
            EquipmentClass equipmentClass = (from ec in this.EquipmentClasses
                                             where ec.EquipmentClassId == equipmentClassId
                                            select ec).First();

            return equipmentClass;
        }
    }
}
