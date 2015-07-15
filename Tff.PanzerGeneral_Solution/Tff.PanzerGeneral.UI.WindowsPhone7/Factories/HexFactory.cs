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
using System.Windows.Media.Imaging;
using Tff.Panzer.Models;
using Tff.Panzer.Controls;
using System.Collections.Generic;
using Tff.Panzer.Models.Army;
using System.Diagnostics;
using Tff.Panzer.Models.Geography;
using Tff.Panzer.Models.Army.Unit;
using System.Threading;

namespace Tff.Panzer.Factories
{
    public class HexFactory
    {
        public BitmapImage TerrainImageSource { get; set; }

        public HexFactory()
        {
            TerrainImageSource = Game.ImageFactory.DryTerrainImage;
        }

        public void AssignTerrainImageSourceBasedOnCurrentTurn()
        {
            //TODO: if hexes take too long to load, add a CurrentTerrain to Game
            //Then check it beofire calling this

            switch(Game.CurrentTurn.CurrentTerrainCondition.TerrainConditionEnum)
            {
                case TerrainConditionEnum.Dry:
                    TerrainImageSource = Game.ImageFactory.DryTerrainImage;
                    break;
                case TerrainConditionEnum.Frozen:
                    TerrainImageSource = Game.ImageFactory.FrozenTerrainImage;
                    break;
                case TerrainConditionEnum.Muddy:
                    TerrainImageSource = Game.ImageFactory.MuddyTerrainImage;
                    break;
                default:
                    TerrainImageSource = Game.ImageFactory.DryTerrainImage;
                    break;
            }
        }

        public void AssignBoardCoordinates(Tile tile, Hex hex)
        {
            int boardRowPadding = Constants.BaseBoardRowPadding;
            int boardRowOffset = Constants.BaseBoardRowOffset; 
            int boardColumnPadding = Constants.BaseBoardColumnPadding;
            int boardColumnOffset = Constants.BaseBoardColumnOffset;
            int currentViewLevel = Game.CurrentViewLevel;

            
            if (tile.ColumnNumber % 2 == 0)
            {
                hex.BoardYCoordinate = (boardRowPadding * currentViewLevel * tile.RowNumber)
                    + (boardRowOffset * currentViewLevel);
            }
            else
            {
                hex.BoardYCoordinate = (boardRowPadding * currentViewLevel * tile.RowNumber);
            }

            hex.BoardXCoordinate = (boardColumnPadding * currentViewLevel * tile.ColumnNumber)
                + (boardColumnOffset * currentViewLevel);

            Point point = new Point();
            point.Y = (double)hex.BoardYCoordinate+((boardRowPadding * currentViewLevel) / 2);
            point.X = (double)hex.BoardXCoordinate+((boardColumnPadding * currentViewLevel) / 2);
            hex.CenterPoint = point;

        }

        public Hex GetHex(Tile tile)
        {
            Hex hex = new Hex();
            AddTerrain(tile, hex);
            return hex;
        }

        public List<Hex> GetHexes(List<Tile> tiles)
        {
            AssignTerrainImageSourceBasedOnCurrentTurn();
            List<Hex> hexes = new List<Hex>();
            Hex hex = null;
            int iHexId = 0;
            foreach (Tile tile in tiles)
            {
                hex = new Hex();
                hex.IsHitTestVisible = true;
                hex.HexId = iHexId++;
                hex.TileId = tile.TileId;
                AssignBoardCoordinates(tile, hex);
                hex.SetValue(Canvas.LeftProperty, (Double)hex.BoardXCoordinate);
                hex.SetValue(Canvas.TopProperty, (Double)hex.BoardYCoordinate);
                AddTerrain(tile, hex);
                if (tile.GroundUnit != null && tile.AirUnit == null)
                {
                    if (tile.Nation != null)
                    {
                        AddNation(hex, tile.Nation);
                    }
                    AddUnit(hex, tile.GroundUnit);
                    AddStrength(hex, tile.GroundUnit);
                }
                if (tile.GroundUnit == null && tile.AirUnit != null)
                {
                    if (tile.Nation != null)
                    {
                        AddNation(hex, tile.Nation);
                    }
                    AddUnit(hex, tile.AirUnit);
                    AddStrength(hex, tile.AirUnit);
                }
                if (tile.GroundUnit != null && tile.AirUnit != null)
                {
                    AddUnit(hex, tile.GroundUnit);
                    AddStackedUnit(hex, tile.AirUnit);
                }
                if (tile.GroundUnit == null && tile.AirUnit == null)
                {
                    if (tile.Nation != null)
                    {
                        AddNation(hex, tile.Nation);
                    }
                }
                hexes.Add(hex);
            }
            return hexes;
        }

        private Canvas GetCanvas(Hex hex, string canvasName)
        {
            Canvas canvas = null;
            foreach (UIElement uiElement in hex.MainCanvas.Children)
            {
                canvas = uiElement as Canvas;
                if (canvas != null)
                {
                    if (canvas.Name == canvasName)
                    {
                        return canvas;
                    }
                }
            }
            return null;
        }

        private Path GetPath(Canvas canvas, string pathName)
        {
            Path path = null;
            foreach (UIElement uiElement in canvas.Children)
            {
                path = uiElement as Path;
                if (path != null)
                {
                    if (path.Name == pathName)
                    {
                        return path;
                    }
                }
            }
            return path;
        }

        private void CreatePathOutline(Path path, String pathName)
        {
            path.Name = pathName;
            path.IsHitTestVisible = true;
            //TODO: Performance
            path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel; //100 //Constants.BaseHexHeight * Game.CurrentViewLevel;
            path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel; //120 //Constants.BaseHexWidth * Game.CurrentViewLevel;
            path.Stroke = new SolidColorBrush(Colors.Gray);
            path.StrokeThickness = 1;
            path.HorizontalAlignment = HorizontalAlignment.Stretch;
            path.VerticalAlignment = VerticalAlignment.Stretch;
            path.Margin = new Thickness(0);
            path.Stretch = Stretch.Fill;

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(0, 8.660254);
            pathFigure.Segments.Add(new LineSegment { Point = new Point(5, 17.320508) });
            pathFigure.Segments.Add(new LineSegment { Point = new Point(15, 17.320508) });
            pathFigure.Segments.Add(new LineSegment { Point = new Point(20, 8.660254) });
            pathFigure.Segments.Add(new LineSegment { Point = new Point(15, 0) });
            pathFigure.Segments.Add(new LineSegment { Point = new Point(5, 0) });
            pathFigure.Segments.Add(new LineSegment { Point = new Point(0, 8.660254) });
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            path.Data = pathGeometry;
            path.SetValue(Canvas.TopProperty, 0.0);
            path.SetValue(Canvas.LeftProperty, 0.0);
        }

        private void CreatePathOutline(Path path, String pathName, bool VictoryIndicator)
        {
            CreatePathOutline(path, "TerrainPath");
            if (VictoryIndicator)
            {
                path.StrokeThickness = 5;
                path.Stroke = new SolidColorBrush(Colors.Yellow);
            }
        }

        private void AddImage(Path path, ImageSource imageSource, int xCoordinate, int yCoordinate)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.Stretch = Stretch.None;
            imageBrush.AlignmentX = AlignmentX.Left;
            imageBrush.AlignmentY = AlignmentY.Top;
            imageBrush.ImageSource = imageSource;
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = xCoordinate;
            translateTransform.Y = yCoordinate;
            transformGroup.Children.Add(translateTransform);
            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = Game.CurrentViewLevel;
            scaleTransform.ScaleY = Game.CurrentViewLevel;
            transformGroup.Children.Add(scaleTransform);
            imageBrush.Transform = transformGroup;
            path.Fill = imageBrush;
        }

        public void AddTerrain(Tile tile, Hex hex)
        {
            Canvas canvas = new Canvas();
            canvas.Name = "TerrainCanvas";
            Path path = new Path();
            CreatePathOutline(path, "TerrainPath", tile.VictoryIndicator);
            AddImage(path, TerrainImageSource, tile.Terrain.ImageXCoordinate, tile.Terrain.ImageYCoordinate);
            canvas.Children.Add(path);
            hex.MainCanvas.Children.Add(canvas);
        }

        public void RemoveTerrain(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "TerrainCanvas");
            hex.MainCanvas.Children.Remove(canvas);
        }

        public void ResizeTerrain(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "TerrainCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "TerrainPath");
                ImageBrush imageBrush = (ImageBrush)path.Fill;
                TransformGroup transformGroup = (TransformGroup)imageBrush.Transform;
                ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[1];
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }        
        }

        public void AddColor(Hex hex, Color color)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "ColorCanvas";
                canvas.SetValue(Canvas.ZIndexProperty, 9);
                canvas.Opacity = .5;
                Path path = new Path();
                CreatePathOutline(path, "ColorPath");
                SolidColorBrush colorBrush = new SolidColorBrush();
                colorBrush.Color = color;
                ScaleTransform scaleTransform = new ScaleTransform();
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                colorBrush.Transform = scaleTransform;
                path.Fill = colorBrush;
                canvas.Children.Add(path);
                hex.MainCanvas.Children.Add(canvas);
            });
        }

        public void RemoveColor(Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "ColorCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });
        }

        public void ResizeColor(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "ColorCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "ColorPath");
                SolidColorBrush colorBrush = (SolidColorBrush)path.Fill;
                ScaleTransform scaleTransform = (ScaleTransform)colorBrush.Transform;
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }

        }

        public void AddHexInfo(Tile tile, Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "HexInfoCanvas";
                TextBlock textBlock = new TextBlock();
                textBlock.Name = "HexInfoTextBlock";
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.SetValue(Canvas.LeftProperty, (double)(15 * Game.CurrentViewLevel));
                textBlock.SetValue(Canvas.TopProperty, (double)5 * Game.CurrentViewLevel);
                textBlock.Height = 10 * Game.CurrentViewLevel;
                textBlock.Width = 30 * Game.CurrentViewLevel;
                textBlock.FontFamily = new FontFamily("Arial");
                textBlock.Foreground = new SolidColorBrush(Colors.Black);
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Text = String.Format("{0} {1}:{2}", tile.TileName.TileNameDescription, tile.ColumnNumber, tile.RowNumber);
                canvas.Children.Add(textBlock);
                hex.MainCanvas.Children.Add(canvas);
            });

        }

        public void RemoveHexInfo(Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "HexInfoCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });
        }

        public void ResizeHexInfo(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "HexInfoCanvas");
            if (canvas != null)
            {
                TextBlock textBlock = (TextBlock)canvas.Children[0];
                textBlock.SetValue(Canvas.LeftProperty, (double)(15 * Game.CurrentViewLevel));
                textBlock.SetValue(Canvas.TopProperty, (double)5 * Game.CurrentViewLevel);
                textBlock.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                textBlock.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;

            }
        }

        public void AddUnit(Hex hex, IUnit unit)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "UnitCanvas";
                Path path = new Path();
                CreatePathOutline(path, "UnitPath");
                AddImage(path, Game.ImageFactory.EquipmentImage, unit.Equipment.ImageXCoordinate, unit.Equipment.ImageYCoordinate);
                canvas.Children.Add(path);
                hex.MainCanvas.Children.Add(canvas);
            });

            
        }

        public void RemoveUnit(Hex hex)
        {

            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "UnitCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });
        }

        public void ResizeUnit(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "UnitCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "UnitPath");
                ImageBrush imageBrush = (ImageBrush)path.Fill;
                TransformGroup transformGroup = (TransformGroup)imageBrush.Transform;
                ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[1];
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }

        }

        public void TriggerExplosion(Hex hex)
        {
            Thread.Sleep(TimeSpan.FromSeconds(.2));
            for(int i=0; i < 6; i++)
            {
                hex.Dispatcher.BeginInvoke(() =>
                {
                    AddExplosion(hex, i);
                });
                Thread.Sleep(TimeSpan.FromSeconds(.2));
                hex.Dispatcher.BeginInvoke(() =>
                {
                    RemoveExplosion(hex);
                });
            }
        }

        public void AddExplosion(Hex hex, int sequenceNumber)
        {
            Canvas canvas = new Canvas();
            canvas.Name = "ExplodeCanvas";
            Path path = new Path();
            CreatePathOutline(path, "ExplodePath");
            AddImage(path, Game.ImageFactory.ExplosionImage, sequenceNumber*-60, 0);
            canvas.Children.Add(path);
            hex.MainCanvas.Children.Add(canvas);

        }

        public void RemoveExplosion(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "ExplodeCanvas");
            hex.MainCanvas.Children.Remove(canvas);
        }

        public void ResizeExplosion(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "ExplodeCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "ExplodePath");
                ImageBrush imageBrush = (ImageBrush)path.Fill;
                TransformGroup transformGroup = (TransformGroup)imageBrush.Transform;
                //ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[1];
                //scaleTransform.ScaleX = Game.CurrentViewLevel;
                //scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }

        }

        public void TriggerStrength(Hex hex, IUnit unit)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                RemoveStrength(hex);
                AddStrength(hex, unit);

            });

        }

        public void AddStrength(Hex hex, IUnit unit)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "StrengthCanvas";
                canvas.SetValue(Canvas.ZIndexProperty, 2);
                Path path = new Path();
                CreatePathOutline(path, "StrengthPath");
                AddImage(path, Game.ImageFactory.StrengthImage,
                    Game.StrengthFactory.GetXCoordinate(unit.CurrentStrength),
                    Game.StrengthFactory.GetYCoordinate(unit.Nation.Side.SideId));
                canvas.Children.Add(path);
                hex.MainCanvas.Children.Add(canvas);
            });

            
        }

        public void RemoveStrength(Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "StrengthCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });


        }

        public void ResizeStrength(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "StrengthCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "StrengthPath");
                ImageBrush imageBrush = (ImageBrush)path.Fill;
                TransformGroup transformGroup = (TransformGroup)imageBrush.Transform;
                ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[1];
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }

        }

        public void AddNation(Hex hex, Nation nation)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "NationCanvas";
                canvas.SetValue(Canvas.ZIndexProperty, 1);
                Path path = new Path();
                CreatePathOutline(path, "NationPath");
                AddImage(path, Game.ImageFactory.NationImage, nation.ImageXCoordinate * -1, nation.ImageYCoordinate * -1);
                canvas.Children.Add(path);
                hex.MainCanvas.Children.Add(canvas);
            });
        }

        public void RemoveNation(Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "NationCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });


        }

        public void ResizeNation(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "NationCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "NationPath");
                ImageBrush imageBrush = (ImageBrush)path.Fill;
                TransformGroup transformGroup = (TransformGroup)imageBrush.Transform;
                ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[1];
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }
        }

        public void AddStackedUnit(Hex hex, IUnit unit)
        {

            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "StackedUnitCanvas";
                Path path = new Path();
                CreatePathOutline(path, "StackedUnitPath");
                AddImage(path, Game.ImageFactory.StackedEquipmentImage,
                    unit.Equipment.StackedImageXCoordinate, unit.Equipment.StackedImageYCoordinate);
                canvas.Children.Add(path);
                hex.MainCanvas.Children.Add(canvas);
            });

        }

        public void RemoveStackedUnit(Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "StackedUnitCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });

        }

        public void ResizeStackedUnit(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "StackedUnitCanvas");
            if (canvas != null)
            {
                Path path = GetPath(canvas, "StackedUnitPath");
                ImageBrush imageBrush = (ImageBrush)path.Fill;
                TransformGroup transformGroup = (TransformGroup)imageBrush.Transform;
                ScaleTransform scaleTransform = (ScaleTransform)transformGroup.Children[1];
                scaleTransform.ScaleX = Game.CurrentViewLevel;
                scaleTransform.ScaleY = Game.CurrentViewLevel;
                path.Height = Constants.BaseHexHeight * Game.CurrentViewLevel;
                path.Width = Constants.BaseHexWidth * Game.CurrentViewLevel;
            }


        }

        public void AddTransportUnit(Hex hex, ITransportUnit unit)
        {

            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = new Canvas();
                canvas.Name = "UnitCanvas";
                Path path = new Path();
                CreatePathOutline(path, "UnitPath");
                AddImage(path, Game.ImageFactory.EquipmentImage, unit.Equipment.ImageXCoordinate, unit.Equipment.ImageYCoordinate);
                canvas.Children.Add(path);
                hex.MainCanvas.Children.Add(canvas);
            });

        }

        public void RemoveTransport(Hex hex, IUnit unit)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                Canvas canvas = GetCanvas(hex, "UnitCanvas");
                hex.MainCanvas.Children.Remove(canvas);
            });


        }

        public void TriggerCrosshairs(Hex hex)
        {
            hex.Dispatcher.BeginInvoke(() =>
            {
                AddCrosshairs(hex);
            });
            Thread.Sleep(TimeSpan.FromSeconds(1));
            hex.Dispatcher.BeginInvoke(() =>
            {
                RemoveCrosshairs(hex);
            });
        }

        public void AddCrosshairs(Hex hex)
        {
            Canvas canvas = new Canvas();
            canvas.Name = "CrosshairCanvas";
            Path path = new Path();
            CreatePathOutline(path, "HexsidesPath");
            AddImage(path, Game.ImageFactory.HexsidesImage, Constants.CrosshairsXCoordinate, Constants.CrosshairsYCoordinate);
            canvas.Children.Add(path);
            hex.MainCanvas.Children.Add(canvas);
        }

        public void RemoveCrosshairs(Hex hex)
        {
            Canvas canvas = GetCanvas(hex, "CrosshairCanvas");
            hex.MainCanvas.Children.Remove(canvas);
        }


    }
}
