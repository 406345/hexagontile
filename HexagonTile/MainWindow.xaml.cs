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
            fileWatcher.Created += FileWatcher_Created;
            fileWatcher.Deleted += FileWatcher_Deleted      ;
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                LoadShotcuts();
            });
        }

        private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                LoadShotcuts();
            });
        }

        private void CreateNotifyIcon()
        {
            
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
                    tile.VirtualPosition.X = posConfig.X;
                    tile.VirtualPosition.Y = posConfig.Y;
                }
                else
                {
                    tile.VirtualPosition.X = idx++;
                    tile.VirtualPosition.Y = 0;
                }
                tile.Margin = p;
                world.Children.Add(tile);
                tile.AdjustToGrid();
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
                    world.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 255, 255));
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

                selectedTile.VirtualPosition.X = (int)((m.Left+ SysConfig.Instance.Tile.Width / 2) / SysConfig.Instance.Tile.Width);
                selectedTile.VirtualPosition.Y = (int)((m.Top + SysConfig.Instance.Tile.Height / 2) / SysConfig.Instance.Tile.Height);
                selectedTile.Margin = m;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StyleHelper.HideAppInTaskbar(this);
        }
    }
}
