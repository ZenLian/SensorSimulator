using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;

namespace SensorSimulator
{
    public partial class MainWindow
    {
        // 传感器初始化
        private void InitSensor() => InitTimer();
        // 传感器数据
        private byte[] sensor0Data = { 0xFF, 0x5A, 0x00, 0x00, 0x00, 0x33 };
        private byte[] sensor1Data = { 0xFF, 0x5A, 0x00, 0x01, 0x00, 0x33 };
        private byte[] sensor2Data = { 0xFF, 0x5A, 0x00, 0x02, 0x00, 0x33 };

        #region 模拟传感器操作
        // TODO: SendSensorData()发送相应传感器数据
        private bool SendSensorData(byte sensor)
        {
            switch (sensor)
            {
                case 0x00:
                    serialPort.Write(sensor0Data, 0, sensor0Data.Length);
                    break;
                case 0x01:
                    serialPort.Write(sensor1Data, 0, sensor0Data.Length);
                    break;
                case 0x02:
                    serialPort.Write(sensor2Data, 0, sensor0Data.Length);
                    break;
                default:
                    break;
            }
            return true;
        }

        // StartSensor()启动传感器
        private bool StartSensor()
        {
            try
            {
                // 启动数据更新定时器
                StartTimer(1000/(int.Parse(refreshFreqTextBox.Text)));
                // 注册接收函数
                serialPort.DataReceived += DataReceivedHandler;
                serialPort.ReceivedBytesThreshold = 1; // 每收到1字节触发一次
                Info("成功启动传感器！");
                return true;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
        }

        // StopSensor()停止传感器
        private bool StopSensor()
        {
            try
            {
                // 停止数据更新定时器
                StopTimer();
                // 注销接收函数
                serialPort.DataReceived -= DataReceivedHandler;
                Info("传感器已停止");
                return true;
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
                return false;
            }
        }

        // 发送线程
        //private Thread sendThread;
        //private void SendRoutine()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            //string nowString;
        //            DateTime now = DateTime.Now;
        //            //nowString = string.Format("{0}:{1}:{2}.{3}", now.)
        //            Console.WriteLine(string.Format("[{0}] send data: {1}", now.ToLongTimeString(), dataFrame.ToString()));
        //            //SerialSend(dataFrame);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Send Thread Aborted.");
        //    }
        //}


        // 数据帧状态
        private enum DataReceivedState
        {
            Reset,
            Head,
            Data,
            Tail
        };
        private DataReceivedState state = DataReceivedState.Reset;
        // 数据接收处理函数
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // 读出缓冲区中所有数据
            var bytesToRead = serialPort.BytesToRead;
            var receiveBuffer = new byte[bytesToRead];
            serialPort.Read(receiveBuffer, 0, bytesToRead);

            // 逐字节处理接收
            foreach (var x in receiveBuffer)
            {
                CheckDataState(x);
            }

        }
        // 逐个字节处理
        private void CheckDataState(byte data)
        {
            switch (state)
            {
                case DataReceivedState.Reset:
                    if (data == 0xFF)
                    {
                        state = DataReceivedState.Head;
                    }
                    break;
                case DataReceivedState.Head:
                    if (data == 0x5A)
                    {
                        state = DataReceivedState.Data;
                    }
                    else
                    {
                        state = DataReceivedState.Reset;
                    }
                    break;
                case DataReceivedState.Data:
                    // 收到命令，发送相应传感器数据
                    if (data == 0x00 || data == 0x01 || data == 0x02)
                    {
                        SendSensorData(data);
                    }
                    else
                    {
                        state = DataReceivedState.Reset;
                    }
                    break;
                default:
                    state = DataReceivedState.Reset;
                    break;
            }
        }
        #endregion

        #region 用于数据更新的定时器
        DispatcherTimer updateDataTimer = new DispatcherTimer();

        private void InitTimer()
        {
            updateDataTimer.IsEnabled = false;
            updateDataTimer.Tick += UpdateDataTimer_Tick;
        }

        // 启动定时器(ms)
        private void StartTimer(int interval)
        {
            updateDataTimer.IsEnabled = true;
            updateDataTimer.Interval = TimeSpan.FromMilliseconds(interval);
            updateDataTimer.Start();
        }

        // 停止定时器
        private void StopTimer()
        {
            updateDataTimer.IsEnabled = false;
            updateDataTimer.Stop();
        }

        // 定时器时间到，更新数据（递增）
        private void UpdateDataTimer_Tick(object sender, EventArgs e)
        {
            // 更新数据
            sensor0Data[4]++;
            sensor1Data[4]++;
            sensor2Data[4]++;
            // 同时更新UI
            sensor0TextBox.Text = string.Format("{0:X2} {1:X2}", sensor0Data[3], sensor0Data[4]);
            sensor1TextBox.Text = string.Format("{0:X2} {1:X2}", sensor1Data[3], sensor1Data[4]);
            sensor2TextBox.Text = string.Format("{0:X2} {1:X2}", sensor2Data[3], sensor2Data[4]);
        }


        #endregion
    }
}
