using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace HexagonTile.Core
{
    class ProcessExecutor
    {
        public static void Execute(AppMeta app)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(app.Shortcut.ExePath);
                startInfo.Arguments = app.Shortcut.Arguments;
                startInfo.UseShellExecute = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
