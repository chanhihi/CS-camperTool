using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using camperTool_TEST.Class;

namespace camperTool_TEST
{
    public partial class camperTest : Form
    {
        public SerialPort Serial = new SerialPort();    //시리얼 통신 변수
        delegate void SetTextCallBack(string opt);

        public byte[] SerialRX = new byte[2048];
        public byte[] SerialTX = new byte[2048];
        public int Serial_In_Pointer = 0;
        public int Serial_Out_Pointer = 0;
        public int Serial_Size = 0;
        public string[] cmdTemp = new string[10];
        public static int keyTemp;
        public enum Simulator_Name { PBA, FINAL };
        public Simulator_Name SelectApp = Simulator_Name.PBA;

        public string PortNumber;
        public string PRESENT_PORT = "";

        public tpms_Handler tpms_handler = null;
        public ResultForm resultMsg = null;

        public string PRODUCTNUMBER { get; set; }
        public string IMEI { get; set; }
        public string SERIALNUMBER { get; set; }
        public camperTest()
        {
            InitializeComponent();
            tpms_handler = new tpms_Handler(this);
            ComPortDisplay();
            Serial.DataReceived += new SerialDataReceivedEventHandler(serial_DataReceived);
        }

        private void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int i_recv_size = Serial.BytesToRead;
            byte[] b_tmp_buf = new byte[i_recv_size];
            string recv_str = "";

            Serial.Read(b_tmp_buf, 0, i_recv_size);
            recv_str += Encoding.Default.GetString(b_tmp_buf);

            //this.Invoke(new SetTextCallBack(display_data), new object[] { recv_str });
            this.BeginInvoke(new SetTextCallBack(display_data), new object[] { recv_str });

            recv_str = null;
        }
        private void display_data(string str)
        {
            //this.textBox_msg.AppendText(str);
            //this.textBox_msg.AppendText("\r\n");
            //this.textBox_msg.ScrollToCaret();

            tpms_handler.rx_data_proc(str);

        }

        private void textBox_cmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (Serial.IsOpen)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Thread tBcmd = new Thread(sendCMD);
                    tBcmd.Start();
                    if (!tBcmd.Join(TimeSpan.FromSeconds(10)))
                    {
                        tBcmd.Abort();
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    textBox_cmd.Text = cmdTemp[keyTemp];
                    if (keyTemp != 9) keyTemp++;
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (keyTemp != 0) keyTemp--;
                    textBox_cmd.Text = cmdTemp[keyTemp];
                }
            }
            else
            {
                MessageBox.Show("SERIAL PORT CONNECTION FAIL");
            }
            return;
        }

        private void sendCMD()
        {
            try
            {
                button_cmd_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Timeout" + ex);
            }
        }

        private void button_cmd_Click(object sender, EventArgs e)
        {
            string comport_str = "";
            if (Serial.IsOpen)
            {
                comport_str = textBox_cmd.Text;
                this.textBox_msg.AppendText(comport_str + "\r\n");
                textBox_msg.ScrollToCaret();
                Send(comport_str);
                if (cmdTemp[0] != null)
                {
                    for (int i = 9; i != 0; i--)
                    {
                        cmdTemp[i] = cmdTemp[i - 1];
                    }
                }
                cmdTemp[0] = textBox_cmd.Text;
                keyTemp = 0;
                textBox_cmd.Text = null;
            }
            else
            {
                MessageBox.Show("SERIAL PORT CONNECTION FAIL");
            }
            return;
        }


        private void button_clear_MouseClick(object sender, MouseEventArgs e)
        {
            textBox_msg.Text = null;
        }


        public static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        public void BootTimerStart()
        {
            tpms_PARAM.BootTimer.Interval = 5000;
            tpms_PARAM.BootTimer.Enabled = true;
        }

        public void BootTimerStop()
        {
            tpms_PARAM.BootTimer.Enabled = false;
        }

        public void CmdWaitTimerStart()
        {
            tpms_PARAM.CmdWaitTimer.Interval = 1000;
            tpms_PARAM.CmdWaitTimer.Enabled = true;
        }

        public void CmdWaitTimerStop()
        {
            tpms_PARAM.CmdWaitTimer.Enabled = false;
        }

        public void WaitTimer_Start(int sec)
        {
            int tout = sec * 1000;

            tpms_PARAM.WaitTimer.Interval = tout;
            tpms_PARAM.WaitTimer.Enabled = true;
        }

        public void WaitTimer_Stop()
        {
            tpms_PARAM.WaitTimer.Enabled = false;
        }
        public void ShortTimer_Start(int msec)
        {
            tpms_PARAM.ShortTimer.Interval = msec;
            tpms_PARAM.ShortTimer.Enabled = true;
        }

        public void ShortTimer_Stop()
        {
            tpms_PARAM.ShortTimer.Enabled = false;
        }

        public void Device_Test_Param_Save()
        {
            xlData xlData = new xlData();

            if (resultMsg != null)
            {
                string update_data = TPMS_LOG_HANDLER.build_result_msg((int)SelectApp);
                resultMsg.update_debug_msg(Color.Black, update_data, true);
            }

            string log_data = TPMS_LOG_HANDLER.build_log_msg();

            string modem_data = TPMS_LOG_HANDLER.build_modem_msg();

            xlData.putSheet(log_data);

            try
            {

                //TPMS_LOG_HANDLER.TPMS_LogWrite(log_data, modem_data);
            }
            catch
            {
                MessageBox.Show("Log Write Failed");
            }

            try
            {
                TPMS_LOG_HANDLER.TPMS_LogClose();
            }
            catch
            {
                //DEBUG_MSG.PRT_RED(rbShell, "Log Close Failed", true);
            }
        }
        public string[] GetPortName()
        {
            return SerialPort.GetPortNames();
        }
        private void ComPortDisplay()
        {
            if (!Serial.IsOpen)
            {
                cbComPort.Items.Clear();
                string[] ports = GetPortName();

                foreach (string port in ports)
                {
                    cbComPort.Items.Add(port);
                }

                btnUartConnect.Enabled = false;
                btnUartDisconnect.Enabled = false;

                if (ports.Length > 0)
                {
                    btnUartConnect.Enabled = true;

                    try
                    {
                        cbComPort.SelectedIndex = 0;

                        int idx = cbComPort.FindStringExact(Properties.Settings.Default.ComPort);
                        if (idx >= 0)
                            cbComPort.SelectedIndex = idx;
                    }
                    catch { }
                }
            }
        }

        public System.IO.Ports.Parity Parity;
        public System.IO.Ports.StopBits StopBit;

        private void btnUartConnect_Click(object sender, EventArgs e)
        {

            if (!this.Serial.IsOpen)
            {

                this.Serial.PortName = cbComPort.Text;
                this.Serial.BaudRate = 115200;
                this.Serial.DataBits = 8;
                this.Parity = System.IO.Ports.Parity.None;
                this.StopBit = System.IO.Ports.StopBits.None;
                this.Serial.Open();

                Status.Text = cbComPort.Text + " connect";
                btnUartDisconnect.Enabled = true;
                gbControl.Enabled = true;
                groupBox3.Enabled = true;

                btnComClear_Click(null, null);
                tpms_handler.Initialize_excel_parameter();
                tpms_handler.initialize_label_parameter();
                tpms_handler.initialize_button_parameter();
            }
            else
            {
                MessageBox.Show("Already Open it");
            }
        }

        private void btnUartDisconnect_Click(object sender, EventArgs e)
        {
            this.Serial.Close();
            Status.Text = "Disconnet";
            btnUartConnect.Enabled = true;
            gbControl.Enabled = false;
            groupBox3.Enabled = false;
            tpms_PARAM.bAutoTestOn = false;

            tpms_handler.Initialize_excel_parameter();
            tpms_handler.initialize_label_parameter();
            tpms_handler.initialize_button_parameter();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void cbComPort_Click(object sender, EventArgs e)
        {
            cbComPort.Items.Clear();
            foreach (string comport in SerialPort.GetPortNames())
            {
                this.cbComPort.Items.Add(comport);
            }
        }

        private void btnTestStart_Click(object sender, EventArgs e)
        {
            btnComClear_Click(null, null);

            if (Serial.IsOpen)
            {
                btnTestStart.Enabled = false;
                btnTestStop.Enabled = true;
                Status.Text = cbComPort.Text + " Test Start";

                this.Enabled = false;
                Delay(1000);
                this.Enabled = true;

                tpms_PARAM.bAutoTestOn = true;
                tpms_handler.LB_MOUSE_CLICK(this, null, testModeCheck);

            }
        }

        private void btnTestStop_Click(object sender, EventArgs e)
        {
            if (Serial.IsOpen)
            {
                Status.Text = cbComPort.Text + " Test Pause";

                btnTestStart.Enabled = true;
                btnTestStop.Enabled = false;
                this.Enabled = false;
                Delay(1000);
                this.Enabled = true;

                tpms_PARAM.bAutoTestOn = false;
            }
        }

        private void resultForm_Click(object sender, EventArgs e)
        {
            if (tpms_PARAM.resultFormOpen == false)
            {
                tpms_PARAM.resultFormOpen = true;
                resultMsg = new ResultForm(this);

                resultMsg.FormClosed += new FormClosedEventHandler(this.resultMsgForm_Close);
                resultMsg.Show();
            }
        }
        private void resultMsgForm_Close(object sender, EventArgs e)
        {
            tpms_PARAM.resultFormOpen = false;
            resultMsg.Dispose();
            resultMsg = null;
        }

        private void checkComplete_Click(object sender, EventArgs e)
        {

            xlData xlData = new xlData();

            string pt = productNumb.Text;
            Int32 ProductNumb = Convert.ToInt32(pt);
            tpms_LOG.ProducNumber = ProductNumb;
            tpms_LOG.IMEI = xlData.id[ProductNumb].IMEI;
            tpms_LOG.SerialNumber = xlData.id[ProductNumb].SERIALNUMBER;

            if (tpms_PARAM.resultFormOpen == false)
            {
                tpms_PARAM.resultFormOpen = true;
                resultMsg = new ResultForm(this);

                resultMsg.FormClosed += new FormClosedEventHandler(this.resultMsgForm_Close);
                resultMsg.Show();
            }
            BootTimerStop();
            WaitTimer_Stop();
            CmdWaitTimerStop();
            ShortTimer_Stop();

            this.Enabled = false;

            PRESENT_PORT = "";
            Device_Test_Param_Save();
            tpms_handler.Initialize_excel_parameter();
            tpms_handler.initialize_label_parameter();
            tpms_handler.initialize_button_parameter();

            if (Serial.IsOpen)
            {
                Send("!ftm reset");
            }
            else
            {
                //DEBUG_MSG.PRT_DARKORANGE(rbShell, "Not Connected", true);
            }

            this.Enabled = true;

            btnUartDisconnect.PerformClick();

            if (checkBox2.Checked)
            {
                btnUartConnect_Click(null, null);
            }

        }

        public bool Send(string txMsg)
        {
            if ((txMsg == "") || (txMsg.Length <= 0))
                return false;

            if (!Serial.IsOpen)
                return false;

            try
            {
                //serial.Write(txMsg + "\r\n");
                //Serial.Write(txMsg + "\r\n"); << 설정
                Serial.Write(txMsg + "\n");
                //this.textBox_msg.AppendText("serial Send :"+txMsg +" \r\n");

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Send(byte[] txMsg, int len)
        {
            if (len <= 0)
                return false;

            if (!Serial.IsOpen)
                return false;

            try
            {
                Serial.Write(txMsg, 0, len);

                //PRT("UART] TTx:" + len + "/" + Util.disp_pkt(txMsg, len));

                return true;
            }
            catch
            {
                return false;
            }
        }


        private void ledSuccess_Click(object sender, EventArgs e)
        {
            tpms_LOG.r_led = true;
            led_Resp.Text = "Success!!!!";

            //if (tpms_PARAM.bAutoTestOn)
            //    tpms_handler.LB_MOUSE_CLICK(this, null, modemCheck);

            ledSuccess.BackColor = Color.LightSeaGreen;
            ledFail.BackColor = Color.Transparent;

            textBox_msg.AppendText("LED SUCCESS CHECKING\r\n");
            textBox_msg.ScrollToCaret();
        }

        private void ledFail_Click(object sender, EventArgs e)
        {
            tpms_LOG.r_led = false;
            led_Resp.Text = "Failure!!!";

            //if (tpms_PARAM.bAutoTestOn)
            //    tpms_handler.LB_MOUSE_CLICK(this, null, modemCheck);

            ledSuccess.BackColor = Color.Transparent;
            ledFail.BackColor = Color.IndianRed;

            textBox_msg.AppendText("LED FAILURE CHECKING\r\n");
            textBox_msg.ScrollToCaret();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox_msg.Text = null;
            tpms_handler.msgCounting = "";
        }

        private void btnTestInit_Click(object sender, EventArgs e)
        {
            BootTimerStop();
            WaitTimer_Stop();
            CmdWaitTimerStop();
            ShortTimer_Stop();

            tpms_handler.Initialize_excel_parameter();
            tpms_handler.initialize_label_parameter();
            tpms_handler.initialize_button_parameter();
        }
        public static List<xlData> Datas = new List<xlData>();
        private void button1_Click(object sender, EventArgs e)
        {
            xlData.includeExcel(Datas);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            xlData.exportExcel(Datas);
            Task.WaitAll();
        }

        public void btnCompare_Click(object sender, EventArgs e)
        {
            if (xlData.fCheck)
            {
                string pt;
                string it;
                string st;

                pt = productNumb.Text;

                int i;

                Int32 ProductNumb = Convert.ToInt32(pt);
                //Int32 SerialNumb = Convert.ToInt32(st);
                //Int32 ImeiNumb = Convert.ToInt32(it);
                if (ProductNumb != 0)
                {
                    i = ProductNumb;
                    IMEI_txt_Box.Text = xlData.id[i].IMEI;
                    SN_txt_Box.Text = xlData.id[i].SERIALNUMBER.ToString();
                }

                it = IMEI_txt_Box.Text;
                st = SN_txt_Box.Text;

                //textBox_msg.AppendText("pn value : " + pt + "\r\n");
                //textBox_msg.AppendText("im value : " + it + "\r\n");
                //textBox_msg.AppendText("sn value : " + st + "\r\n");
                //textBox_msg.ScrollToCaret();

                if (checkBox1.Checked)
                {
                    productNumb.Value = i = (int)productNumb.Value + 1;
                    IMEI_txt_Box.Text = xlData.id[i].IMEI;
                    SN_txt_Box.Text = xlData.id[i].SERIALNUMBER.ToString(); ;
                }
            }
            else
            {
                MessageBox.Show("엑셀을 먼저 가져와야 합니다.");
            }
        }

        private void btnIMEI_Click(object sender, EventArgs e)
        {
            var result = xlData.id.FindIndex(1, x => x.IMEI == IMEI_txt_Box.Text);

            // var cmpTemp = ;

            if (result != 0)
            {
                try
                {
                    productNumb.Value = Convert.ToInt32(xlData.id[result].PRODUCTNUMBER);
                    SN_txt_Box.Text = xlData.id[result].SERIALNUMBER.ToString();
                    MessageBox.Show("찾았습니다. 생산번호 " + result + "번 입니다." + "\r\n" + "시리얼 넘버 " + SN_txt_Box.Text + "번 입니다.");
                }
                catch
                {
                    MessageBox.Show("잘못된 번호입니다.");
                }
            }
            else
            {
                MessageBox.Show("ERROR : Access Denied");
            }
        }

        private void btnComClear_Click(object sender, EventArgs e)
        {
            tpms_handler.msgCounting = null;
        }
    }
}

