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
    public class EquipmentSubClassFactory
    {
        public List<EquipmentSubClass> EquipmentSubClasses { get; private set; }
        public EquipmentClassFactory EquipmentClassFactory { get; private set; }

        public EquipmentSubClassFactory()
        {
            EquipmentSubClasses = new List<EquipmentSubClass>();
            EquipmentClassFactory = new EquipmentClassFactory();

            Uri uri = new Uri(Constants.EquipmentSubClassDataPath, UriKind.Relative);
            XElement applicationXml;
            StreamResourceInfo xmlStream = Application.GetResourceStream(uri);
            applicationXml = XElement.Load(xmlStream.Stream);
            var data = from wz in applicationXml.Descendants("EquipmentSubClass")
                       select wz;

            EquipmentSubClass equipmentSubClass = null;
            foreach (var d in data)
            {
                equipmentSubClass = new EquipmentSubClass();
                equipmentSubClass.EquipmentSubClassId = (Int32)d.Element("EquipmentSubClassId");
                equipmentSubClass.EquipmentSubClassDescription = (String)d.Element("EquipmentSubClassDescription");
                equipmentSubClass.EquipmentClass = EquipmentClassFactory.GetEquipmentClass((Int32)d.Element("EquipmentClassId"));
                EquipmentSubClasses.Add(equipmentSubClass);
            }
        }

        public EquipmentSubClass GetEquipmentSubClass(int equipmentSubClassId)
        {
            EquipmentSubClass equipmentSubClass = (from esc in this.EquipmentSubClasses
                                             where esc.EquipmentSubClassId == equipmentSubClassId
                                            select esc).First();

            return equipmentSubClass;
        }
    }
}
