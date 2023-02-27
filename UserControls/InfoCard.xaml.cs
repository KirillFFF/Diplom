using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductsClient.UserControls
{

    public partial class InfoCard : UserControl
    {
        public static readonly DependencyProperty _titleProperty = DependencyProperty.Register("Title", typeof(string), typeof(InfoCard));
        public static readonly DependencyProperty _numberProperty = DependencyProperty.Register("Number", typeof(string), typeof(InfoCard));
        public static readonly DependencyProperty _iconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(InfoCard));
        public static readonly DependencyProperty _background1Property = DependencyProperty.Register("Background1", typeof(Color), typeof(InfoCard));
        public static readonly DependencyProperty _background2Property = DependencyProperty.Register("Background2", typeof(Color), typeof(InfoCard));
        public static readonly DependencyProperty _elipseBackground1Property = DependencyProperty.Register("ElipseBackground1", typeof(Color), typeof(InfoCard));
        public static readonly DependencyProperty _elipseBackground2Property = DependencyProperty.Register("ElipseBackground2", typeof(Color), typeof(InfoCard));
        
        public InfoCard()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(_titleProperty);
            set => SetValue(_titleProperty, value);
        }

        public string Number
        {
            get => (string)GetValue(_numberProperty);
            set => SetValue(_numberProperty, value);
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(_iconProperty);
            set => SetValue(_iconProperty, value);
        }

        public Color Background1
        {
            get => (Color)GetValue(_background1Property);
            set => SetValue(_background1Property, value);
        }

        public Color Background2
        {
            get => (Color)GetValue(_background2Property);
            set => SetValue(_background2Property, value);
        }

        public Color ElipseBackground1
        {
            get => (Color)GetValue(_elipseBackground1Property);
            set => SetValue(_elipseBackground1Property, value);
        }

        public Color ElipseBackground2
        {
            get => (Color)GetValue(_elipseBackground2Property);
            set => SetValue(_elipseBackground2Property, value);
        }

    }
}
