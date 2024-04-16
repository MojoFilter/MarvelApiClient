using System.Windows;

namespace MarvelApiClientSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}