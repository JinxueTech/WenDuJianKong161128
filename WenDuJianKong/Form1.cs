using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace WenDuJianKong
{
    public partial class Form1 : Form
    {
        AdvCANIO Device = new AdvCANIO();     
        bool m_bRun = false;
        bool syncflag = false;
        uint nMsgCount = 10;
        public bool self;
        public bool rtr;
        int TotalContrl;
        //AdvCan device = new AdvCan();
        //uint pulNumberofWritten;
        delegate void SetTextCallback(string text);
        
        Thread SendTimeThread;
        Thread SeRcThread1;
        Thread SeRcThread2;

        ThreadStart SendTimeThreadSt;
        ThreadStart ThreadFunc1St;
        ThreadStart ThreadFunc2St;

        public Form1()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            //m_bRun = false;
            Device.acCanClose();
            try
            {
                if ((SendTimeThread.ThreadState&(ThreadState.Stopped|ThreadState.Unstarted))==0)
                {
                    SendTimeThread.Abort();
                    SendTimeThread.Join();
                }
                if ((SeRcThread1.ThreadState & (ThreadState.Stopped | ThreadState.Unstarted)) == 0)
                {
                    SeRcThread1.Abort();
                    SeRcThread1.Join();
                }
                if ((SeRcThread2.ThreadState & (ThreadState.Stopped | ThreadState.Unstarted)) == 0)
                {
                    SeRcThread2.Abort();
                    SeRcThread2.Join();
                }

            }
            catch (Exception ex)
            {

                ShowStatus.Items.Add("系统错误提示"+ex.Message);
            }
            if (disposing)
            {
                if (components!=null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// 开启板卡can1，开启多线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStop_Click(object sender, EventArgs e)
        {
            string CanPortName;
            UInt16 BaudRateValue;
            int nRet = 0;
            uint WriteTimeOutValue;
            uint ReadTimeOutValue;


            if (StartStop.Text=="Start")
            {
                ShowStatus.Items.Clear();
                StartStop.Text = "Stop";
                CanPortName = "Can1";

                nRet = Device.acCanOpen(CanPortName, syncflag, 500, 500);
                if (nRet<0)
                {
                    ShowStatus.Items.Add("Failed to open the CAN port, please check the CAN port name!");
                    StartStop.Text = "Start";
                    return;
                }
                

                nRet = Device.acEnterResetMode();
                if (nRet<0)
                {
                    ShowStatus.Items.Add("Failed to stop opertion!");
                    StartStop.Text = "Start";
                    return;
                }

                if (radioButton1.Checked)
                {
                    BaudRateValue = 50;
                    nRet = Device.acSetBaud(BaudRateValue);
                    if (nRet < 0)
                    {
                        ShowStatus.Items.Add("Failed to set baud!");
                        Device.acCanClose();
                        StartStop.Text = "Start";
                        return;
                    }
                }
                else if (radioButton2.Checked)
                {
                    BaudRateValue = 500;
                    nRet = Device.acSetBaud(BaudRateValue);
                    if (nRet < 0)
                    {
                        ShowStatus.Items.Add("Failed to set baud!");
                        Device.acCanClose();
                        StartStop.Text = "Start";
                        return;
                    }
                }
                else
                {
                    ShowStatus.Items.Add("通讯速率警告，已设置50k");
                    BaudRateValue = 50;
                    nRet = Device.acSetBaud(BaudRateValue);
                    if (nRet < 0)
                    {
                        ShowStatus.Items.Add("Failed to set baud!");
                        Device.acCanClose();
                        StartStop.Text = "Start";
                        return;
                    }
                }

                try
                {
                    WriteTimeOutValue = 5000;//5000ms
                }
                catch (Exception)
                {
                    ShowStatus.Items.Add("Invalid TimeOut value!");
                    Device.acCanClose();
                    StartStop.Text = "Start";
                    return;
                }

                ReadTimeOutValue = WriteTimeOutValue;
                nRet = Device.acSetTimeOut(ReadTimeOutValue, WriteTimeOutValue);
                if (nRet<0)
                {
                    ShowStatus.Items.Add("Failed to set Timeout!");
                    Device.acCanClose();
                    StartStop.Text = "Start";
                    return;
                }

                nRet = Device.acSetSelfReception(self);
                if (nRet <0)
                {
                    ShowStatus.Items.Add("Failed to set self reception!");
                    Device.acCanClose();
                    StartStop.Text = "Start";
                    return;
                }

                nRet = Device.acEnterWorkMode();
                if (nRet <0)
                {
                    ShowStatus.Items.Add("err enter work mode");
                    ShowStatus.Items.Add("Failed to restart operation!");
                    Device.acCanClose();
                    StartStop.Text = "Start";
                    return;
                }

                ShowStatus.Items.Add("acCanopen Finished.");

                SendTimeThreadSt = new ThreadStart(SendTimeThreadMethod);
                SendTimeThread = new Thread(SendTimeThreadSt);
                SendTimeThread.Priority = ThreadPriority.Normal;
                SendTimeThread.Start();

                ThreadFunc1St = new ThreadStart(ThreadMethod1);
                SeRcThread1 = new Thread(ThreadFunc1St);
                SeRcThread1.Priority = ThreadPriority.Normal;

                ThreadFunc2St = new ThreadStart(ThreadMethod2);
                SeRcThread2 = new Thread(ThreadFunc2St);
                SeRcThread2.Priority = ThreadPriority.Normal;

                SeRcThread1.Start();
                SeRcThread2.Start();

                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                TotalControlBox.Enabled = false;


            }
            else
            {

                Device.acCanClose();
                SendTimeThread.Abort();
                SeRcThread1.Abort();
                SeRcThread2.Abort();
                Thread.Sleep(100);
                StartStop.Text = "Start";
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                TotalControlBox.Enabled = true;
                SaveListToText(ShowStatus);             //把showstatus中的条目保存到桌面指定的text中。
                Application.Exit();
            }
            if (ShowStatus.Items.Count > 3000)
            {
                ShowStatus.Items.Clear();

            }
            //ShowStatus.Items.Clear();
        }

        /// <summary>
        /// 发送时间线程执行此函数。包括设置线程间隔时间。
        /// </summary>
        private void SendTimeThreadMethod()
        {
            while (true)
            {
                SendTimeEveryMinute();
                Thread.Sleep(6000);                 //发送时间线程间隔时间设置
            }
        }

        /// <summary>
        /// 下面为发送时间函数，向整个网络控制器循环发送时间
        /// </summary>
        private void SendTimeEveryMinute()
        {
            byte Year, Mon, Day, Week, Hour, Minute;
            int nRet;
            AdvCan.canmsg_t[] timemsg = new AdvCan.canmsg_t[1];
            timemsg[0].id = 0; timemsg[0].cob = 0; timemsg[0].length = 8; timemsg[0].flags = 0x00; timemsg[0].data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            uint pulNumberofWritten = 0;
            //m_bRun = true;
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            Year = (byte)(dt.Year - 2000);
            Mon = (byte)(dt.Month);
            Day = (byte)(dt.Day);
            Week = (byte)(dt.DayOfWeek);
            Hour = (byte)(dt.Hour);
            Minute = (byte)(dt.Minute);
            ShowState("现在时间是：" + dt);
            //ShowStatus.Items.Add("现在时间是：" + dt);
            TotalContrl = 1 + Convert.ToInt32(TotalControlBox.Text);
            for (int i = 1; i < TotalContrl; i++)
            {

                timemsg[0].id = (byte)i;
                timemsg[0].cob = 0;
                timemsg[0].length = 8;
                timemsg[0].flags = 0x00;
                timemsg[0].data[0] = 0;
                timemsg[0].data[1] = 3;
                timemsg[0].data[2] = Year;
                timemsg[0].data[3] = Mon;
                timemsg[0].data[4] = Day;
                timemsg[0].data[5] = Week;
                timemsg[0].data[6] = Hour;
                timemsg[0].data[7] = Minute;

                //cansend.CanSend(timemsg);
                nRet = Device.acCanWrite(timemsg, 1, ref pulNumberofWritten);
                if (nRet == AdvCANIO.TIME_OUT)
                {
                    ShowState("Sending timeout!");
                }
                else if (nRet == AdvCANIO.OPERATION_ERROR)
                {
                    ShowState("Sending error!");
                }


            }
            ShowState("Sending the time is ok!");



        }

        private void ThreadMethod1()
        {
            while (true)
            {
                ReceiveMessage();
                Thread.Sleep(0);
            }
        }

        private void ReceiveMessage()
        {
            string ReceiveStatus;
            int nRet;
            uint nReadCount = nMsgCount;
            uint pulNumberofRead = 0;
            uint ReceiveIndex = 0;
            //char InnerTemp[6][3];
            //byte[][] InnerTemp = new byte[6][3];
            byte[] Sta_Id = new byte[6];
            byte[] Temp = new byte[12];
            byte Type, Format;
            int w = 0, Controller_Id, Flag = 0;
            int[] Id = new int[6];                          //存入6户的状态，需要输出。0防冻，1无人，2有人。
            UInt16[] TempNum = new UInt16[6];
            double[] Temperature = new double[6];           //存入6户的温度，需要输出。

            //AdvCan.canmsg_t[] msgRead = new AdvCan.canmsg_t[nMsgCount];
            //for (int i = 0; i < nMsgCount; i++)
            //{
            //    msgRead[i].data = new byte[8];
            //}

            //ReceiveIndex = 0;
            AdvCan.canmsg_t[] msgRead = new AdvCan.canmsg_t[1];
            msgRead[0].data = new byte[8];

            //nRet = Device.acCanRead(msgRead, nReadCount, ref pulNumberofRead);
            nRet = Device.acCanRead(msgRead, 1, ref pulNumberofRead);
            if (nRet==AdvCANIO.TIME_OUT)
            {
                ShowState("Package receiving timeout!");
            }
            else if (nRet==AdvCANIO.OPERATION_ERROR)
            {
                ShowState("Package error!");
            }
            else
            {
                for (int j = 0; j < pulNumberofRead; j++)
                {
                    ReceiveStatus = "Package";
                    ReceiveStatus = ReceiveStatus + Convert.ToString(ReceiveIndex + j + 1) + "is";
                    if (msgRead[j].id==AdvCan.ERRORID)
                    {
                        ReceiveStatus += "a incorrect package!";
                        ShowState(ReceiveStatus);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < msgRead[j].length; i++)
                        {
                            ReceiveStatus += msgRead[j].data[i].ToString();
                            ReceiveStatus += " ";
                            ShowState(ReceiveStatus);
                        }
                        Type = msgRead[0].data[0];
                        Format = msgRead[0].data[1];
                        Controller_Id = (int)msgRead[0].id;         //网络控制器id，需要输出。
                        if (Type==0)    //设置1,2,3户状态，室内温度。
                        {
                            Sta_Id[0] = (byte)(((byte)(msgRead[0].data[1] >> 2)) & 0x03);   //0防冻 1无人 2有人
                            Sta_Id[1] = (byte)(((byte)(msgRead[0].data[1] >> 4)) & 0x03);
                            Sta_Id[2] = (byte)(((byte)(msgRead[0].data[1] >> 6)) & 0x03);
                            Id[0] = Sta_Id[0];                                              //1户  的状态0防冻 1无人 2有人
                            Id[1] = Sta_Id[1];                                              //2户
                            Id[2] = Sta_Id[2];                                              //3户
                            byte[] temp1 = new byte[2] { msgRead[0].data[2], msgRead[0].data[3] };
                            byte[] temp2 = new byte[2] { msgRead[0].data[4], msgRead[0].data[5] };
                            byte[] temp3 = new byte[2] { msgRead[0].data[6], msgRead[0].data[7] };
                            TempNum[0] = BitConverter.ToUInt16(temp1, 0);                   //两个字节转换来的16位无符号整数。
                            TempNum[1] = BitConverter.ToUInt16(temp2, 0);
                            TempNum[2] = BitConverter.ToUInt16(temp3, 0);
                            Temperature[0] = (double)TempNum[0] / 10; ;
                            Temperature[1] = (double)TempNum[1] / 10; ;
                            Temperature[2] = (double)TempNum[2] / 10; ;
                        }
                        else if (Type==1)   //设置4,5,6户状态，室内温度。
                        {
                            Sta_Id[3] = (byte)(((byte)(msgRead[0].data[1] >> 2)) & 0x03);   //0防冻 1无人 2有人
                            Sta_Id[4] = (byte)(((byte)(msgRead[0].data[1] >> 4)) & 0x03);
                            Sta_Id[5] = (byte)(((byte)(msgRead[0].data[1] >> 6)) & 0x03);
                            Id[3] = Sta_Id[3];                                              //4户  的状态0防冻 1无人 2有人
                            Id[4] = Sta_Id[4];                                              //5户
                            Id[5] = Sta_Id[5];                                              //6户
                            byte[] temp4 = new byte[2] { msgRead[0].data[2], msgRead[0].data[3] };
                            byte[] temp5 = new byte[2] { msgRead[0].data[4], msgRead[0].data[5] };
                            byte[] temp6 = new byte[2] { msgRead[0].data[6], msgRead[0].data[7] };
                            TempNum[3] = BitConverter.ToUInt16(temp4, 0);                   //两个字节转换来的16位无符号整数。
                            TempNum[4] = BitConverter.ToUInt16(temp5, 0);
                            TempNum[5] = BitConverter.ToUInt16(temp6, 0);
                            Temperature[3] = (double)TempNum[3] / 10; ;
                            Temperature[4] = (double)TempNum[4] / 10; ;
                            Temperature[5] = (double)TempNum[5] / 10; ;

                        }
                    }
                }
            }


        }

        private void ThreadMethod2()
        {

        }



        /// <summary>
        /// 为了界面显示代码安全，而添加的在showstatus上显示的显示函数
        /// </summary>
        /// <param name="text"></param>
        private void ShowState (string text)
        {
            if (this.ShowStatus.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ShowState);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.ShowStatus.Items.Add(text);
            }
        }

        /// <summary>
        /// 在桌面创建日志文件，记录showstatus中的各项。
        /// </summary>
        /// <param name="listbox"></param>
        private void SaveListToText(ListBox listbox)
        {
            string path = @"C:\Users\Administrator\Desktop\ZhuangTaiJiLu.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream file=File.Create(path))
            {
                StreamWriter sw = new StreamWriter(file, Encoding.UTF8);
                int icount = listbox.Items.Count - 1;
                for (int i = 0; i < icount; i++)
                {
                    sw.WriteLine(listbox.Items[i].ToString());
                }
                sw.Flush();
                sw.Close();
                file.Close();
            }
            
            

            
            //sfd.Filter = "(*.txt)|*.txt";
        }

    }
}

