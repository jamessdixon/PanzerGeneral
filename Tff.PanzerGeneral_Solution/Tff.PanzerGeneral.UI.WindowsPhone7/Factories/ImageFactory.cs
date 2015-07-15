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

namespace Tff.Panzer.Factories
{
    public class ImageFactory
    {
        BitmapImage dryTerrainImage = null;
        BitmapImage muddyTerrainImage = null;
        BitmapImage frozenTerrainImage = null;
        BitmapImage equipmentImage = null;
        BitmapImage strengthImage = null;
        BitmapImage nationImage = null;
        BitmapImage stackedEquipmentImage = null;
        BitmapImage explosionImage = null;
        BitmapImage hexsidesImage = null;

        public ImageFactory()
        {
            Uri pictureUri = new Uri(Constants.DryTerrainImagePath, UriKind.Relative);
            dryTerrainImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.MuddyTerrainImagePath, UriKind.Relative);
            muddyTerrainImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.FrozenTerrainImagePath, UriKind.Relative);
            frozenTerrainImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.EquipmentImagePath, UriKind.Relative);
            equipmentImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.StrengthImagePath, UriKind.Relative);
            strengthImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.NationImagePath, UriKind.Relative);
            nationImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.StackedUnitImagePath, UriKind.Relative);
            stackedEquipmentImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.ExplodeImagePath, UriKind.Relative);
            explosionImage = new BitmapImage(pictureUri);
            pictureUri = new Uri(Constants.HexsidesImagePath, UriKind.Relative);
            hexsidesImage = new BitmapImage(pictureUri);

        }

        public BitmapImage DryTerrainImage
        {
            get
            {
                return dryTerrainImage;
            }
        }

        public BitmapImage MuddyTerrainImage
        {
            get
            {
                return muddyTerrainImage;
            }
        }
        public BitmapImage FrozenTerrainImage
        {
            get
            {
                return frozenTerrainImage;
            }
        }

        public BitmapImage EquipmentImage
        {
            get
            {
                return equipmentImage;
            }
        }

        public BitmapImage StrengthImage
        {
            get
            {
                return strengthImage;
            }
        }

        public BitmapImage NationImage
        {
            get
            {
                return nationImage;
            }
        }

        public BitmapImage StackedEquipmentImage
        {
            get
            {
                return stackedEquipmentImage;
            }
        }


        public ImageSource ExplosionImage
        {
            get
            {
                return explosionImage;
            }
        }

        public ImageSource HexsidesImage
        {
            get
            {
                return hexsidesImage;
            }
        }

    }
}
