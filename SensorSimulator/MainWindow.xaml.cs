using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitPort();
            InitSensor();
        }

        private void PortOpenOrCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                if (ClosePort())
                {
                    portOpenOrCloseButton.Content = "打开";
                    // 关闭时串口配置可用
                    avaliablePortsComboBox.IsEnabled = true;
                    baudRateComboBox.IsEnabled = true;
                    parityComboBox.IsEnabled = true;
                    dataBitsComboBox.IsEnabled = true;
                    stopBitsComboBox.IsEnabled = true;
                    // 关闭时查找可用
                    portFindButton.IsEnabled = true;
                    // 关闭时控制区不可用
                    sensorControlGroupBox.IsEnabled = false;
                }
            }
            else
            {
                if (OpenPort())
                {
                    portOpenOrCloseButton.Content = "关闭";
                    // 打开时串口配置不可用
                    avaliablePortsComboBox.IsEnabled = false;
                    baudRateComboBox.IsEnabled = false;
                    parityComboBox.IsEnabled = false;
                    dataBitsComboBox.IsEnabled = false;
                    stopBitsComboBox.IsEnabled = false;
                    // 打开时查找不可用
                    portFindButton.IsEnabled = false;
                    // 打开时控制区可用
                    sensorControlGroupBox.IsEnabled = true;
                }
            }
        }

        private void PortFindButton_Click(object sender, RoutedEventArgs e)
        {
            FindPorts();
        }

        private void StartOrStopSensorButton_Click(object sender, RoutedEventArgs e)
        {
            if (isListening)
            {
                if (StopSensor())
                {
                    startOrStopSensorButton.Content = "启动传感器";
                    refreshFreqTextBox.IsEnabled = true;
                    serialConfigGroupBox.IsEnabled = true;
                    isListening = false;
                }
            }
            else
            {
                if (StartSensor())
                {
                    startOrStopSensorButton.Content = "关闭传感器";
                    refreshFreqTextBox.IsEnabled = false;
                    serialConfigGroupBox.IsEnabled = false;
                    isListening = true;
                }
            }
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
