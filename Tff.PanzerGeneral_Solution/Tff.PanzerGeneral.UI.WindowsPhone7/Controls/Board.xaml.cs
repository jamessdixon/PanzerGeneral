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
using Tff.Panzer.Factories;
using System.Diagnostics;
using Tff.Panzer.Models.Army;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Resources;
using Tff.Panzer.Models.Geography;



namespace Tff.Panzer.Controls
{
    public partial class Board : UserControl
    {
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }
        public Double MainCanvasHeight { get; set; }
        public Double MainCanvasWidth { get; set; }

        public Board()
        {
            InitializeComponent();
        }
 
        private void MainScrollViewer_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (Game.BoardFactory.ActiveTile != null)
            {
                e.Complete();
                e.Handled = true;
            }
        }

        private void GestureListener_PinchCompleted(object sender, Microsoft.Phone.Controls.PinchGestureEventArgs e)
        {
            double distanceRatio = e.DistanceRatio;
            Game.BoardFactory.UpdateBoardViewLevel(distanceRatio);
        }

        private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            if (Game.CurrentViewLevel > 1)
            {
                Hex closestHex = Game.BoardFactory.GetClosestHex(e.GetPosition(this.MainCanvas));
                Game.BoardFactory.HandleTapEvent(closestHex);
            }

        }

        private void GestureListener_DragCompleted(object sender, Microsoft.Phone.Controls.DragCompletedGestureEventArgs e)
        {

            double startXCoordinate = e.GetPosition(this.MainCanvas).X - e.HorizontalChange;
            double startYCoordinate = e.GetPosition(this.MainCanvas).Y - e.VerticalChange;

            Point point = new Point(startXCoordinate, startYCoordinate);
            Hex startHex = Game.BoardFactory.GetClosestHex(point);
            Hex targetHex = Game.BoardFactory.GetClosestHex(e.GetPosition(this.MainCanvas));

            if (Game.BoardFactory.AllowDragEvent(startHex, targetHex))
            {
                Game.BoardFactory.HandleDragEvent(targetHex);
            }
        }


    }
}
