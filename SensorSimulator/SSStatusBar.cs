using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SensorSimulator
{
    public partial class MainWindow : Window
    {
        #region 状态栏
        /// <summary>
        /// 状态信息提示
        /// </summary>
        /// <param name="message"></param>
        private void Info(string message)
        {
            statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
            statusInfoTextBlock.Text = message;
        }

        /// <summary>
        /// 警告信息提示
        /// </summary>
        /// <param name="message"></param>
        private void Alert(string message)
        {
            statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x21, 0x2A));
            statusInfoTextBlock.Text = message;
        }
        #endregion
    }
}
