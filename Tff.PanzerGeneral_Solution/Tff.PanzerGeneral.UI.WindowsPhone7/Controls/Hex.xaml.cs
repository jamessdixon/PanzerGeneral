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
using Tff.Panzer.Models;

namespace Tff.Panzer.Controls
{
    public partial class Hex : UserControl
    {
        public int HexId { get; set; }
        public int TileId { get; set; }
        public int BoardXCoordinate { get; set; }
        public int BoardYCoordinate { get; set; }
        public Point CenterPoint { get; set; }

        public Hex()
        {
            InitializeComponent();
        }
    }
}
