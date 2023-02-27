using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductsClient.UserControls
{
    public partial class Order : UserControl
    {
        public static readonly DependencyProperty _titleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Order));
        public static readonly DependencyProperty _descProperty = DependencyProperty.Register("Desc", typeof(string), typeof(Order));
        public static readonly DependencyProperty _iconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Order));

        public Order()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(_titleProperty);
            set => SetValue(_titleProperty, value);
        }

        public string Desc
        {
            get => (string)GetValue(_descProperty);
            set => SetValue(_descProperty, value);
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(_iconProperty);
            set => SetValue(_iconProperty, value);
        }
    }
}
