using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Diagnostics;
using Tff.Panzer.Models.Army;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models;

namespace Tff.Panzer.Controls 
{
    public partial class TestHex : UserControl
    {
        public int HexId { get; set; }
        public int BoardXCoordinate { get; set; }
        public int BoardYCoordinate { get; set; }
        public Point CenterPoint { get; set; }
        
        public TestHex()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return String.Format("{0}", HexId);
        }

        public override int GetHashCode()
        {
            return HexId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Hex otherHex = (Hex)obj;
                if (this.HexId == otherHex.HexId)
                    return true;
                else
                    return false;
            }
        }

        private void ObjectAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            //this.ExplodeCanvas.Opacity = 0;
        }

    }
}
