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
using System;
using System.Threading;

namespace SensorSimulator
{
    public partial class MainWindow : Window
    {
        #region 数据
        // 串口对象
        private SerialPort serialPort = new SerialPort();
        // 启动标识
        private bool isListening = false;

        #endregion

        #region 串口底层操作
        // 初始化端口
        private void InitPort() => FindPorts();

        // 打开串口
        private bool OpenPort()
        {
            ConfigPort();
            
            try
            {
                serialPort.Open();
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();
                Info(string.Format("成功打开端口{0}，波特率{1}", serialPort.PortName, serialPort.BaudRate));
                return true;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
        }


        // 关闭串口
        private bool ClosePort()
        {
            try
            {
                serialPort.Close();
                Info(string.Format("成功关闭端口{0}", serialPort.PortName));
                return true;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
        }

        // 查找可用端口
        private bool FindPorts()
        {
            var portNames = SerialPort.GetPortNames();
            if (portNames.Length != 0)
            {
                avaliablePortsComboBox.ItemsSource = portNames;
                avaliablePortsComboBox.SelectedIndex = 0;
                avaliablePortsComboBox.IsEnabled = true;
                portOpenOrCloseButton.IsEnabled = true;
                Info(string.Format("查找到{0}个可用端口", portNames.Length));
                return true;
            }
            else
            {
                avaliablePortsComboBox.IsEnabled = false;
                portOpenOrCloseButton.IsEnabled = false;
                Alert("未找到可用端口");
                return false;
            }
        }

        // 发送字符串
        private bool SerialSend(string textData)
        {
            if (serialPort.IsOpen != true)
            {
                Alert("串口未打开!");
                return false;
            }

            try
            {
                serialPort.Write(textData);
                return true;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
        }

        // TODO: 发送16进制
        private bool SerialSend(byte[] hexData)
        {
            if (serialPort.IsOpen != true)
            {
                Alert("串口未打开!");
                return false;
            }

            try
            {
                serialPort.Write(hexData, 0, hexData.Length);
                return true;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
        }

        #endregion

        #region 读取串口配置
        private void ConfigPort()
        {
            serialPort.PortName = GetSelectedPortName();
            serialPort.BaudRate = GetSelectedBaudRate();
            serialPort.Parity = GetSelectedParity();
            serialPort.DataBits = GetSelectedDataBits();
            serialPort.StopBits = GetSelectedStopBits();
        }

        private string GetSelectedPortName()
        {
            return avaliablePortsComboBox.Text;
        }

        private int GetSelectedBaudRate()
        {
            int baudrate;
            if (int.TryParse(baudRateComboBox.Text, out baudrate))
                return baudrate;
            else
                return 9600;
        }

        private Parity GetSelectedParity()
        {
            switch (parityComboBox.Text)
            {
                case "Odd":
                    return Parity.Odd;
                case "Even":
                    return Parity.Even;
                case "Mark":
                    return Parity.Mark;
                case "Space":
                    return Parity.Space;
                default: // "None"
                    return Parity.None;
            }
        }

        private int GetSelectedDataBits()
        {
            return int.Parse(dataBitsComboBox.Text);
        }

        private StopBits GetSelectedStopBits()
        {
            switch (stopBitsComboBox.Text)
            {
                default:
                case "1":
                    return StopBits.One;
                case "0":
                    return StopBits.None;
                case "1.5":
                    return StopBits.OnePointFive;
                case "2":
                    return StopBits.Two;
            }
        }
        #endregion

    }
}
