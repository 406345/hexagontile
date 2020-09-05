using HexagonTile.Config;
using HexagonTile.Helper;
using System;
using System.Collections.Generic;
using System.Windows;
using HexagonTile.Core;
using System.IO;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;
using WinApi.User32;
using System.Drawing;
using System.Windows.Media;

namespace HexagonTile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Tile> tiles = new List<Tile>();
        private FileSystemWatcher fileWatcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = false;
             
            this.Activated += MainWindow_Activated;
            this.StateChanged += MainWindow_StateChanged;

            this.LoadShotcuts();

            fileWatcher.EnableRaisingEvents = true;
            fileWatcher.Changed += fileWatcher_Changed;
        }

        private void CreateNotifyIcon()
        {
            
        }

        private void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    break;
                case WatcherChangeTypes.Deleted:
                    break;
                case WatcherChangeTypes.Changed:
                    this.Dispatcher.Invoke(() =>
                    {
                        LoadShotcuts();
                    });
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                case WatcherChangeTypes.All:
                    break;
                default:
                    break;
            }
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
            this.world.Children.Clear();
            this.tiles.Clear();

            var files = SystemHelper.GetAllDesktopShortcuts();

            int idx = 0;

            foreach (var item in files)
            {
                Tile tile = new Tile(AppMeta.create(item));
                tile.Owner = this;
                tiles.Add(tile);


                var posConfig = SysConfig.Instance.Position.Where(x => x.Path == tile.AppMeta.Path).FirstOrDefault();
                var p = tile.Margin;

                if (posConfig != null)
                {
                    p.Left = posConfig.X;
                    p.Top = posConfig.Y;
                }
                else
                {
                    p.Left = SysConfig.Instance.Tile.Width * idx++;
                    p.Top = 10;
                }
                tile.Margin = p;
                world.Children.Add(tile);
            }
        }

        Tile selectedTile = null;

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (selectedTile != null)
            {
                selectedTile.AdjustToGrid();
                selectedTile = null;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (SystemContext.Instance.TileMode == Enums.TileMode.Editing && e.Source is Tile)
            {
                selectedTile = (Tile)e.Source;
            }
            else if(Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt))
            {
                if(SystemContext.Instance.TileMode == Enums.TileMode.Editing)
                {
                    SystemContext.Instance.TileMode = Enums.TileMode.Normal;
                    world.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 255, 255, 255));
                }
                else if(SystemContext.Instance.TileMode == Enums.TileMode.Normal)
                {
                    SystemContext.Instance.TileMode = Enums.TileMode.Editing;
                    world.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 255, 255, 255));
                }
            }

        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (SystemContext.Instance.TileMode == Enums.TileMode.Editing &&
                selectedTile != null)
            {
                var m = selectedTile.Margin;
                var pos = e.GetPosition(this);
                m.Left = pos.X - SysConfig.Instance.Tile.Width / 2;
                m.Top = pos.Y - SysConfig.Instance.Tile.Height / 2;
                selectedTile.Margin = m;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StyleHelper.HideAppInTaskbar(this);
        }
    }
}
