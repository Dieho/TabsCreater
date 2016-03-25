
using System.Windows;
using FftGuitarTuner;

namespace TabsCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var a = new MainForm();
            a.Show();
        }
    }
}
