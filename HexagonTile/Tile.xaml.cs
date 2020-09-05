using HexagonTile.Config;
using HexagonTile.Core;
using HexagonTile.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HexagonTile
{
    /// <summary>
    /// Tile.xaml 的交互逻辑
    /// </summary>
    public partial class Tile : UserControl
    {
        public AppMeta AppMeta { get; private set; } = new AppMeta();
        public Window Owner { get; set; }
        private double HalfWidth = 1;
        public Tile()
        {
            InitializeComponent();
        }
        public Tile(AppMeta meta)
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Margin = new Thickness(0, 0, 0, 0);
            this.AppMeta = meta;
            this.Width = SysConfig.Instance.Tile.Width;
            this.Height = SysConfig.Instance.Tile.Height;
            this.HalfWidth = this.Width / 2;
            this.iconGrid.Margin = new Thickness(
                 this.Width * 0.15,
                 this.Width * 0.15,
                 this.Width * 0.15,
                 this.Width * 0.15
            );

            this.LoadMeta();
        }

        public void LoadMeta()
        {
            this.iconImage.Source = W32Warpper.GetIconBitmap(this.AppMeta.Path);
            this.tbName.Text = this.AppMeta.Name;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            switch (SystemContext.Instance.TileMode)
            {
                case Enums.TileMode.Normal:
                    this.iconGrid.Margin = new Thickness(
                        this.Width * 0.15,
                        this.Width * 0.15,
                        this.Width * 0.15,
                        this.Width * 0.15
                    );
                    break;
                case Enums.TileMode.Editing:
                    break;
                default:
                    break;
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            switch (SystemContext.Instance.TileMode)
            {
                case Enums.TileMode.Normal:
                    this.UpdateTileSize(e);
                    break;
                case Enums.TileMode.Editing:

                    break;
                default:
                    break;
            }

        }

        public void AdjustToGrid()
        {
            var m = this.Margin;
            var xf = (int)((m.Left + this.HalfWidth) / SysConfig.Instance.Tile.Width);
            var yf = (int)((m.Top + this.HalfWidth) / SysConfig.Instance.Tile.Height);
            var offset = ((int)yf % 2) == 0 ? 0.0 : (SysConfig.Instance.Tile.Width / 2.0);
            m.Left = xf * SysConfig.Instance.Tile.Width + offset;
            m.Top = yf * SysConfig.Instance.Tile.Height; // +

            this.Margin = m;
            SavePosToConfig();
        }

        private void UpdateTileSize(MouseEventArgs e)
        {
            var pos = e.GetPosition(this);
            var center = new Point(SysConfig.Instance.Tile.Width / 2, SysConfig.Instance.Tile.Height / 2);
            var distance = MathHelper.Distance(pos, center);
            var percent = (distance / this.HalfWidth) + 0.2;

            if (percent > 1)
            {
                percent = 1;
            }

            percent = 1 - percent;

            this.iconGrid.Margin = new Thickness(
                 this.Width * 0.15 - (this.Width * 0.15 * percent),
                 this.Width * 0.15 - (this.Width * 0.15 * percent),
                 this.Width * 0.15 - (this.Width * 0.15 * percent),
                 this.Width * 0.15 - (this.Width * 0.15 * percent)
            );

        }

        private void SavePosToConfig()
        {
            var pos = SysConfig.Instance.Position.Where(x => x.Path == this.AppMeta.Path).FirstOrDefault();

            if (pos == null)
            {
                pos = new TilePositionConfig();
                SysConfig.Instance.Position.Add(pos);
            }

            pos.Path = this.AppMeta.Path;
            pos.X = (int)this.Margin.Left;
            pos.Y = (int)this.Margin.Top;

            SysConfig.Save();
        }
    }
}
