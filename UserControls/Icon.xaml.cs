using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductsClient.UserControls
{
    public partial class Icon : UserControl
    {
        public static readonly DependencyProperty _iconProperty = DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(Icon));

        public Icon()
        {
            InitializeComponent();
        }

        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(_iconProperty);
            set => SetValue(_iconProperty, value);
        }
    }
}
