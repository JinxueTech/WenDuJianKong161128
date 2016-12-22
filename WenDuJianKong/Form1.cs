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
using System.Data.SqlClient;

namespace WenDuJianKong
{
    public partial class Form1 : Form
    {
        string connString = "Server=.;database=WenDuJianKong;user id=sa;password=81048204bylsbels ";

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
                Thread.Sleep(20000);                 //发送时间线程间隔时间设置20s
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
            string Time = DateTime.Now.ToString();
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
                    ReceiveStatus = "Package ";
                    ReceiveStatus = ReceiveStatus + Convert.ToString(ReceiveIndex + j + 1) + " is ";
                    if (msgRead[j].id==AdvCan.ERRORID)
                    {
                        ReceiveStatus += "a incorrect package!";
                        ShowState(ReceiveStatus);
                        return;
                    }
                    else if(msgRead[j].id >0)
                    {
                        for (int i = 0; i < msgRead[j].length; i++)
                        {
                            ReceiveStatus += msgRead[j].data[i].ToString();
                            ReceiveStatus += " ";
                        }
                        ShowState(ReceiveStatus);
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
                            SaveRecieveState123(Controller_Id, Id[0], Id[1], Id[2],Time);
                            nSaveRecieveState123(Controller_Id, Id[0], Id[1], Id[2],Time);
                            SaveRecieveTemp123(Controller_Id, Temperature[0], Temperature[1], Temperature[2],Time);
                            nSaveRecieveTemp123(Controller_Id, Temperature[0], Temperature[1], Temperature[2],Time);
                            nUpdateState123(Controller_Id, Id[0], Id[1], Id[2],Time);
                            nUpdateTemp123(Controller_Id, Temperature[0], Temperature[1], Temperature[2],Time);
                            

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
                            SaveRecieveState456(Controller_Id, Id[3], Id[4], Id[5],Time);
                            nSaveRecieveState456(Controller_Id, Id[3], Id[4], Id[5],Time);
                            SaveRecieveTemp456(Controller_Id, Temperature[3], Temperature[4], Temperature[5],Time);
                            nSaveRecieveTemp456(Controller_Id, Temperature[3], Temperature[4], Temperature[5],Time);
                            nUpdateState456(Controller_Id, Id[3], Id[4], Id[5],Time);
                            nUpdateTemp456(Controller_Id, Temperature[3], Temperature[4], Temperature[5],Time);

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

        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        #region 向数据库写入数据函数
        /// <summary>
        /// 存123户状态到主表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="id3"></param>
        private void SaveRecieveState123(int Id,int id1,int id2,int id3,string time)
        {
            string savestate123 = string.Format("insert into WKState(WangKongID,State1,State2,State3,Time) values('{0}','{1}','{2}','{3}','{4}')", Id, id1, id2, id3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command1 = new SqlCommand(savestate123, conn);
                command1.Connection.Open();
                command1.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 存123户的状态到该网控器表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="id3"></param>
        private void nSaveRecieveState123(int Id, int id1, int id2, int id3,string time)
        {
            string nId = Id.ToString();
            string biaoming = "WKState" + nId+ "123";
            string nsavestate123 = string.Format("insert into {0}(WangKongID,State1,State2,State3,Time) values('{1}','{2}','{3}','{4}','{5}')", biaoming ,Id, id1, id2, id3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command1 = new SqlCommand(nsavestate123, conn);
                command1.Connection.Open();
                command1.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 存123户温度到主表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="temp3"></param>
        private void SaveRecieveTemp123(int Id, double temp1, double temp2, double temp3,string time)
        {
            string savetemp123 = string.Format("insert into WKTemp(WangKongID,Temperature1,Temperature2,Temperature3,Time) values('{0}','{1}','{2}','{3}','{4}')", Id, temp1, temp2, temp3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command2 = new SqlCommand(savetemp123, conn);
                command2.Connection.Open();
                command2.ExecuteNonQuery();
            }

        }
        /// <summary>
        /// 存123户温度到该网控器表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="temp3"></param>
        private void nSaveRecieveTemp123(int Id, double temp1, double temp2, double temp3,string time)
        {
            string nId = Id.ToString();
            string biaoming = "WKTemp" + nId +"123";
            string nsavetemp123 = string.Format("insert into {0}(WangKongID,Temperature1,Temperature2,Temperature3,Time) values('{1}','{2}','{3}','{4}','{5}')", biaoming , Id, temp1, temp2, temp3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command1 = new SqlCommand(nsavetemp123, conn);
                command1.Connection.Open();
                command1.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 存456户状态到主表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="id3"></param>
        private void SaveRecieveState456(int Id, int id1, int id2, int id3,string time)
        {
            string savestate456 = string.Format("insert into WKState(WangKongID,State4,State5,State6,Time) values('{0}','{1}','{2}','{3}','{4}')", Id, id1, id2, id3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command3 = new SqlCommand(savestate456, conn);
                command3.Connection.Open();
                command3.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 存456户状态到网控器表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="id3"></param>
        private void nSaveRecieveState456(int Id, int id1, int id2, int id3,string time)
        {
            string nId = Id.ToString();
            string biaoming = "WKState" + nId + "456";
            string savestate456 = string.Format("insert into {0}(WangKongID,State4,State5,State6,Time) values('{1}','{2}','{3}','{4}','{5}')", biaoming ,Id, id1, id2, id3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command3 = new SqlCommand(savestate456, conn);
                command3.Connection.Open();
                command3.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 存456户温度到主表中
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="temp3"></param>
        private void SaveRecieveTemp456(int Id, double temp1, double temp2, double temp3,string time)
        {
            string savetemp456 = string.Format("insert into WKTemp(WangKongID,Temperature4,Temperature5,Temperature6,Time) values('{0}','{1}','{2}','{3}','{4}')", Id, temp1, temp2, temp3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command4 = new SqlCommand(savetemp456, conn);
                command4.Connection.Open();
                command4.ExecuteNonQuery();
            }

        }
        /// <summary>
        /// 存456户温度到该网控器表中。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="temp3"></param>
        private void nSaveRecieveTemp456(int Id, double temp1, double temp2, double temp3,string time)
        {
            string nId = Id.ToString();
            string biaoming = "WKTemp" + nId + "456";
            string savetemp456 = string.Format("insert into {0}(WangKongID,Temperature4,Temperature5,Temperature6,Time) values('{1}','{2}','{3}','{4}','{5}')",biaoming , Id, temp1, temp2, temp3,time);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command4 = new SqlCommand(savetemp456, conn);
                command4.Connection.Open();
                command4.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// 更新界面显示表对应数据库，网控器ID的123户温度，数据库信息。
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="temp3"></param>
        private void nUpdateTemp123(int Id, double temp1, double temp2, double temp3,string time)
        {
            string nId = Id.ToString();
            string updatetemp123 = string.Format("update TempState set Temperature1='{0}',Temperature2='{1}',Temperature3='{2}',Time='{3}' where WangKongID='{4}'", temp1, temp2, temp3, time,Id);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command7 = new SqlCommand(updatetemp123, conn);
                command7.Connection.Open();
                command7.ExecuteNonQuery();
            }

        }

        private void nUpdateTemp456(int Id, double temp1, double temp2, double temp3,string time)
        {
            string nId = Id.ToString();
            string updatetemp456 = string.Format("update TempState set Temperature4='{0}',Temperature5='{1}',Temperature6='{2}',Time='{3}' where WangKongID='{4}'", temp1, temp2, temp3, time,Id);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command8 = new SqlCommand(updatetemp456, conn);
                command8.Connection.Open();
                command8.ExecuteNonQuery();
            }

        }

        private void nUpdateState123(int Id, int id1, int id2, int id3,string time)
        {
            string nId = Id.ToString();
            string updatestate123 = string.Format("update TempState set State1='{0}',State2='{1}',State3='{2}',Time='{3}' where WangKongID='{4}'", id1, id2, id3, time,Id);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command9 = new SqlCommand(updatestate123, conn);
                command9.Connection.Open();
                command9.ExecuteNonQuery();
            }
        }

        private void nUpdateState456(int Id, int id1, int id2, int id3,string time)
        {
            string nId = Id.ToString();
            string updatestate456 = string.Format("update TempState set State4='{0}',State5='{1}',State6='{2}',Time='{3}' where WangKongID='{4}'", id1, id2, id3, time, Id);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand command10 = new SqlCommand(updatestate456, conn);
                command10.Connection.Open();
                command10.ExecuteNonQuery();
            }
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string text = e.Node.Text;
            switch (text)
            {
                case "五号楼":
                    Form2 form2 = new Form2();
                    form2.ShowDialog();
                    break;
            }
        }
        #endregion



        //private void SaveRecieveData456(int Id, int id1,int id2,int id3, double temp1,double temp2,double temp3)
        //{
        //    string savestate456 = string.Format("insert into WKState(WangKongID,State4,State5,State6) values('{0}','{1}','{2}','{3}')", Id, id1, id2, id3);
        //    string savetemp456 = string.Format("insert into WKTemp(WangKongID,Temperature4,Temperature5,Temperature6) values('{0}','{1}','{2}','{3}')", Id, temp1, temp2, temp3);
        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        SqlCommand command = new SqlCommand(savestate456, conn);
        //        command.Connection.Open();
        //        SqlCommand command3 = new SqlCommand(savetemp456, conn);
        //    }
        //    conn.Open();

        //SqlCommand command3 = new SqlCommand(savestate456, conn);
        //SqlCommand command4 = new SqlCommand(savetemp456, conn);


    }
}



