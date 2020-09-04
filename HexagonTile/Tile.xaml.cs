using System;
using System.Collections.Generic;
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
            this.Width = Config.Instance.Tile.Width;
            this.Height = Config.Instance.Tile.Height;
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
            this.iconGrid.Margin = new Thickness(
                this.Width * 0.15,
                this.Width * 0.15,
                this.Width * 0.15,
                this.Width * 0.15
           );
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var pos = e.GetPosition(this);
            var center = new Point(Config.Instance.Tile.Width / 2, Config.Instance.Tile.Height / 2);
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
    }
}
