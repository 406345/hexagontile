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
        public Tile(AppMeta meta)
        {
            InitializeComponent();
            this.AppMeta = meta;
            this.LoadMeta();

        }

        public void LoadMeta()
        {
            this.iconImage.Source = W32Warpper.GetIconBitmap(this.AppMeta.Path);
            this.tbName.Text = this.AppMeta.Name;
        }
    }
}
