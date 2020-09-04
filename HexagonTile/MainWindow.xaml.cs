using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinApi.Kernel32;
using WinApi.User32;

namespace HexagonTile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Tile> tiles = new List<Tile>();

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = false;

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ShowInTaskbar = false;

            this.Activated += MainWindow_Activated;
            this.StateChanged += MainWindow_StateChanged;

            this.LoadShotcuts();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            StyleHelper.SetButtom((Window)sender);
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            StyleHelper.SetButtom((Window)sender);
        }

        private void LoadShotcuts()
        {
            var files = SystemHelper.GetAllDesktopShortcuts();

            foreach (var item in files)
            {
                Tile tile = new Tile(AppMeta.create(item));
                world.Children.Add(tile);
                tiles.Add(tile);
            }
        }
    }
}
