using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public class tpms_PARAM
{
    public static string PROJECT_NAME = "tpms-000";
    public static string TPMS_AP_VERSION = "2120"; //0x2120
    public static string REP_REVISION = "490";

    public static string DEVICE_VERSION = "1.7.1";
    public static string MODEM_VERSION = "17.00.543-B007";
    public static int MODEM_RSSI = -128;
    public static int MODEM_RSRP = -128;

    public const string DEF_PROJECT_NAME = "tpms-401";
    public const string DEF_AP_VERSION = "2120";
    public const string DEF_REP_REVISION = "490";
    public const string DEF_DEVICE_VER = "1.7.1";
    public const string DEF_MODEM_VER = "17.00.543-B007";
    public const int DEF_CHK_RSSI = -80;
    public const int DEF_CHK_RSRP = -95;

    public const uint RETRY_COUNT = 30;
    public static uint retry_cnt = 0;

    // UI Handler
    public static Color lbBackColor = Color.Transparent;
    public static Color btnBackColor = Color.Transparent;
    public static Color btnNormalColor = Color.Transparent;


    public const int FTM_WAIT_COUNT = 8;
    public static int boot_wait_cnt = 0;
    public static bool b_boot_timer = false;

    public static uint wifi_rx_cnt = 0;
    public static uint wifi_ap_cnt = 0;

    public static bool bAutoTestOn = false;
    public static bool bIDWrite = false;
    public static bool resultFormOpen = false;


    public static Timer BootTimer = new Timer();
    public static Timer WaitTimer = new Timer();
    public static Timer CmdWaitTimer = new Timer();
    public static Timer ShortTimer = new Timer();

    public static bool bAgpsData = false;
    public static DateTime RecvTime = DateTime.Now;
    public static byte[] AGPS_Data;
    public static int content_length = 0;
    public static byte[] content_body = new byte[10000];

    public static bool agps_http_parse()
    {
        if (bAgpsData != true)
            return false;

        if (AGPS_Data.Length == 0)
            return false;

        string str_len = "";

        try
        {
            string output = Encoding.ASCII.GetString(AGPS_Data, 0, AGPS_Data.Length);

            if (output.Contains("200 OK") != true)
                return false;

            int idx = output.IndexOf("Content-Length:");
            idx += 16;

            while (true)
            {
                if (output.Substring(idx, 1) == "\r")
                {
                    break;
                }
                else
                {
                    str_len += output.Substring(idx, 1);
                    idx++;
                }
            }

            idx = output.IndexOf("\r\n\r\n");

            string msg_token = output.Substring(idx, 4);
            if (msg_token != "\r\n\r\n")
            {
                msg_token = output.Substring(0, idx);
                return false;
            }
            else
            {
                idx += 4;
                msg_token = output.Substring(0, idx);
            }

            content_length = Convert.ToInt32(str_len);

            Array.Copy(AGPS_Data, idx, content_body, 0, content_length);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public enum SIM_STATUS_E_TYPE
    {
        SIM_INIT = 0,
        SIM_READY,

        SIM_NOT_INSERTED = 10,
        SIM_PIN_REQUIRED,
        SIM_PUK_REQUIRED,
        SIM_FAILURE,
        SIM_BUSY,
        SIM_WRONG,
        SIM_INCORRECT_PASSWD,
        SIM_PIN2_REQUIRED,
        SIM_PUK2_REQUIRED,

        SIM_BIP_REQUIRED = 20,
        SIM_STATUS_MAX
    };

    public struct SIM_STATUS
    {
        SIM_STATUS_E_TYPE sim_sts;
        string str_sts;

        public SIM_STATUS(SIM_STATUS_E_TYPE type, string msg)
        {
            this.sim_sts = type;
            this.str_sts = msg;
        }

        public SIM_STATUS_E_TYPE SIM_STS
        {
            get
            {
                return sim_sts;
            }
            set
            {
                sim_sts = value;
            }
        }

        public string STR_STS
        {
            get
            {
                return str_sts;
            }
            set
            {
                str_sts = value;
            }
        }
    }

    public static SIM_STATUS[] usim_status = new SIM_STATUS[]
    {
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_INIT, "Init Complete" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_READY, "Ready" ),

        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_NOT_INSERTED, "NOT Inserted" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_PIN_REQUIRED, "PIN Required" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_PUK_REQUIRED, "PUK Required" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_FAILURE, "Failure" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_BUSY, "Busy" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_WRONG, "Wrong" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_INCORRECT_PASSWD, "Incorrect PASSWD" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_PIN2_REQUIRED, "PIN2 Required" ),
        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_PUK2_REQUIRED, "PUK2 Required" ),

        new SIM_STATUS ( SIM_STATUS_E_TYPE.SIM_BIP_REQUIRED, "BIP Required" )
    };
}


public class tpms_CMD
{
    public enum tpms_TYPE
    {
        tpms_40X,
        tpms_50X,

        MAX_TYPE
    };

    public static tpms_TYPE device_type = tpms_TYPE.MAX_TYPE;

    public enum WaitType
    {
        TEST_MODE_WAIT = 2,
        SFLASH_WAIT = 2,
        RF_EX_WAIT = 2,
        RF_IN_WAIT = 2,
        GPS_1_WAIT = 2,
        GPS_2_WAIT = 2,
        MODEM_TEST_WAIT = 2,
        MODEM_ICCID_WAIT = 2,
        MODEM_RSSI_WAIT = 7,
        MODEM_MDN_WAIT = 2,
        MODEM_REG_WAIT = 5,
        COMPLETE_WAIT = 2,
        SERIAL_WAIT = 2,
        INIT_CFG_WAIT = 2,
        LED_WAIT = 10,
        PWR_OFF_WAIT = 30
    };

    public enum CmdType
    {
        INFO = 0,
        TEST_MODE,              //1
        btnCheck2,              //2
        btnCheck3,              //3
        btnCheck4,              //4
        btnCheck5,              //5
        btnCheck6,              //6
        btnCheck7,              //7
        btnCheck8,              //8
        btnCheck9,              //9
        btnCheck10,              //10
        btnCheck11,              //11
        btnCheck12,              //12
        btnCheck13,              //13
        btnCheck14,              //14
        btnCheck15,              //15
        btnCheck16,              //16
        btnCheck17,              //17
        btnCheck18,              //18
        btnCheck19,              //19
        btnCheck20,              //20
        btnCheck21,              //21
        btnCheck22,              //22
        btnCheck23,              //23
        btnCheck24,              //24
        btnCheck25,              //25

        COMPLETE,               //26
        SERIAL,                 
        INIT_CFG,               
        LED,                
        PWR_OFF,            
                            
        CMD_MAX             
    };

    public static string[] CmdMsg = new string[]
    {
        "!ftm info?",           //0
        "!ftm 001",             //1
        "!ftm 002",             //2        //"!ftm sflash2",
        "!ftm 003",             //
        "!ftm 004",             //
        "!ftm 005",             //
        "!ftm 006",             //6
        "!ftm 007",             //7
        "!ftm 008",             //8
        "!ftm 009",             //9
        "!ftm 010",             //10
        "!ftm 011",             //11
        "!ftm 012",             //12
        "!ftm 013",             //13
        "!ftm 014",             //14
        "!ftm 015",             //15
        "!ftm 016",             //16
        "!ftm 017",             //17
        "!ftm 018",             //18
        "!ftm 019",             //19
        "!ftm 020",             //20
        "!ftm 021",             //21
        "!ftm 022",             //22
        "!ftm 023",             //23
        "!ftm 024",             //24
        "!ftm 025",             //25
        "!complete",            //26
        "!cfg snWrite",         //
        "!cfg init config",     //
        "!ftm led",             //
        "!pwr off?",            //
        ""
    };

    public static CmdType CTYPE = CmdType.CMD_MAX;
}


public class tpms_LOG
{
    // Result Parameter
    public static bool r_test_mode = false;

    public static int ProducNumber = 0;
    public static string IMEI = null;
    public static string SerialNumber = null;

    public static int r_sflash = 0;
    public static uint r_sflash_rx = 0;

    public static bool r_modem_ver = false;
    public static int r_modem_iccid = 0;
    public static int r_modem_rssi = 0;
    public static bool r_modem_mdn = false;
    public static bool r_modem_reg = false;

    public static bool r_rf_in = false;
    public static bool r_rf_ex = false;

    public static bool r_led = false;

    public static bool r_gps = false;
    public static byte r_gps_snr = 0;

    public static bool r_check_all = false;
    public static bool r_cfginit = false;

    public static bool r_motor = false;
    public static bool r_battery = false;
    public static bool r_current = false;
    public static bool r_fuel_gauge = true;
    public static bool r_s_long = false;
    public static bool r_s_short = false;
    public static bool r_s_case = false;

    public static string r_id = "000000";

    public static string[] modem_param = new string[9];
    public static string modem_imei = string.Empty;
    public static string usim_ccid = string.Empty;

    public enum Sensor
    {
        L_STRAP_OCCUR = 0,
        L_STRAP_CLEAR,
        S_STRAP_OCCUR,
        S_STRAP_CLEAR,
        CASE_CLOSE,
        CASE_OPEN,

        SENSOR_MAX
    };

    public static bool[] sensor_press = new bool[(int)Sensor.SENSOR_MAX];

    public static void init_log_param()
    {
        r_test_mode = false;
        r_sflash = 0;
        r_sflash_rx = 0;

        r_modem_ver = false;
        r_modem_iccid = 0;
        r_modem_rssi = 0;
        r_modem_mdn = false;

        r_modem_reg = false;
        r_rf_in = false;
        r_rf_ex = false;
        r_check_all = false;
        r_led = false;
        r_motor = false;
        r_battery = false;
        r_fuel_gauge = true;

        r_gps = false;
        r_gps_snr = 0;

        r_s_long = false;
        r_s_short = false;
        r_s_case = false;

        r_id = "000000";

        Array.Clear(tpms_LOG.sensor_press, 0, (int)tpms_LOG.Sensor.SENSOR_MAX);

        Array.Clear(modem_param, 0, 9);

        modem_imei = string.Empty;
        usim_ccid = string.Empty;
    }
}