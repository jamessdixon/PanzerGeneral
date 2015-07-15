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
using Microsoft.Xna.Framework.Audio;
using System.Windows.Resources;
using Tff.Panzer.Models.Army;

namespace Tff.Panzer.Factories
{
    public class SoundFactory
    {
        SoundEffect trackedSound = null;
        SoundEffect wheeledSound = null;
        SoundEffect walkSound = null;
        SoundEffect jetAirplaneSound = null;
        SoundEffect propAirplaneSound = null;
        SoundEffect navalSound = null;
        SoundEffect battleSound = null;

        public SoundFactory()
        {
            Uri soundUri = new Uri(Constants.TrackedSoundPath, UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(soundUri);
            trackedSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            
            soundUri = new Uri(Constants.WheeledSoundPath, UriKind.Relative);
            info = Application.GetResourceStream(soundUri);
            wheeledSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();

            soundUri = new Uri(Constants.WalkSoundPath, UriKind.Relative);
            info = Application.GetResourceStream(soundUri);
            walkSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();

            soundUri = new Uri(Constants.JetAirplaneSoundPath, UriKind.Relative);
            info = Application.GetResourceStream(soundUri);
            jetAirplaneSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();

            soundUri = new Uri(Constants.PropAirplaneSoundPath, UriKind.Relative);
            info = Application.GetResourceStream(soundUri);
            propAirplaneSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();

            soundUri = new Uri(Constants.NavalSoundPath, UriKind.Relative);
            info = Application.GetResourceStream(soundUri);
            navalSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();

            soundUri = new Uri(Constants.BattleSoundPath, UriKind.Relative);
            info = Application.GetResourceStream(soundUri);
            battleSound = SoundEffect.FromStream(info.Stream);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
        }

        public SoundEffect TrackedSound
        {
            get
            {
                return trackedSound;
            }
        }
        public SoundEffect WheeledSound
        {
            get
            {
                return wheeledSound;
            }
        }
        public SoundEffect WalkSound
        {
            get
            {
                return walkSound;
            }
        }
        public SoundEffect JetAirplaneSound
        {
            get
            {
                return jetAirplaneSound;
            }
        }
        public SoundEffect PropAirplaneSound
        {
            get
            {
                return propAirplaneSound;
            }
        }
        public SoundEffect NavalSound
        {
            get
            {
                return navalSound;
            }
        }
        public SoundEffect BattleSound
        {
            get
            {
                return battleSound;
            }
        }
        public void PlayEquipmentSound(Equipment equipment)
        {
            switch (equipment.MovementTypeEnum)
            {
                case MovementTypeEnum.Tracked:
                    this.TrackedSound.Play();
                    break;
                case MovementTypeEnum.HalfTracked: 
                    this.TrackedSound.Play();
                    break;
                case MovementTypeEnum.Wheeled:
                    this.WheeledSound.Play();
                    break;
                case MovementTypeEnum.Walk:
                    this.WalkSound.Play();
                    break;
                case MovementTypeEnum.None:
                    this.WalkSound.Play();
                    break;
                case MovementTypeEnum.Air:
                    if (equipment.JetIndicator == true)
                    {
                        JetAirplaneSound.Play();

                    }
                    else
                    {
                        PropAirplaneSound.Play();
                    }
                    break;
                case MovementTypeEnum.Water:
                    NavalSound.Play();
                    break;
                case MovementTypeEnum.AllTerrain:
                    WheeledSound.Play();
                    break;
            }

        }
        public void PlayBattleSound()
        {
            this.BattleSound.Play();
        }
    }
}
