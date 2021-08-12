using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using System;
using System.Numerics;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class AvatarUploadImage : UserControl
    {
        private Compositor compositor = Window.Current.Compositor;
        private SpriteVisual effectVisual;
        private CompositionEffectFactory effectFactory;
        private CompositionEffectBrush effectBrush;
        private SpringScalarNaturalMotionAnimation _springAnimation;

        //Events Handler
        public event RoutedEventHandler OpenCameraEventHandler;
        public event RoutedEventHandler OpenFileEventHandler;

        public AvatarUploadImage()
        {
            this.InitializeComponent();
            ActiveCamButton.Scale = new Vector3(0, 0, 0);
            OpenFolderButton.Scale = new Vector3(0, 0, 0);
        }



        public BitmapImage AvatarImage
        {
            get { return (BitmapImage)GetValue(AvatarImageProperty); }
            set { SetValue(AvatarImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvatarImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvatarImageProperty =
            DependencyProperty.Register("AvatarImage", typeof(BitmapImage), typeof(AvatarUploadImage), new PropertyMetadata(null));



        private void Avatar_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ActiveCamButton.Scale = new Vector3(1.5f, 1.5f, 1.5f);
            OpenFolderButton.Scale = new Vector3(1.5f, 1.5f, 1.5f);

            if (_springAnimation != null && effectBrush != null)
            {
                _springAnimation.FinalValue = 100f;
                effectBrush.StartAnimation("Blur.BlurAmount", _springAnimation);
            }
        }

        private void Avatar_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ActiveCamButton.Scale = new Vector3(0, 0, 0);
            OpenFolderButton.Scale = new Vector3(0, 0, 0);

            if (_springAnimation != null && effectBrush != null)
            {
                _springAnimation.FinalValue = 0f;
                effectBrush.StartAnimation("Blur.BlurAmount", _springAnimation);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            effectVisual = compositor.CreateSpriteVisual();
            var destinationBrush = compositor.CreateBackdropBrush();
            //Create the Effect you want
            var graphicsEffect = new GaussianBlurEffect
            {
                Name = "Blur",
                BlurAmount = 0f,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("Background")
            };



            effectFactory = compositor.CreateEffectFactory(graphicsEffect, new[] { "Blur.BlurAmount" });
            effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("Background", destinationBrush);

            effectVisual.Brush = effectBrush;
            effectVisual.Size = new Vector2(150, 150);

            ElementCompositionPreview.SetElementChildVisual(Avatar, effectVisual);

            //Create Spring Animation for nature increase and decrease value
            _springAnimation = compositor.CreateSpringScalarAnimation();
            _springAnimation.Period = TimeSpan.FromSeconds(0.20);
            //_springAnimation.DampingRatio = 0.25f;
        }

        private void ActiveCamButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenCameraEventHandler != null)
            {
                this.OpenCameraEventHandler(this, e);
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFileEventHandler != null)
            {
                this.OpenFileEventHandler(this, e);
            }
        }
    }
}
