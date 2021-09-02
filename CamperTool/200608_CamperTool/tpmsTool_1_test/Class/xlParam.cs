using System.Collections;
using System.Collections.Generic;

public class EXCEL_PARAM
{
    public static List<EXCEL_PARAM> succDatas = new List<EXCEL_PARAM> {
                new EXCEL_PARAM
                {
                    r_product_number= null,
                    r_imei= null,
                    r_serial_number = null,
                    r_test_mode = null,
                    r_sflash = null,
                    r_sflash_rx = null,
                    r_modem_ver = null,
                    r_modem_iccid = null,
                    r_modem_rssi = null,
                    r_modem_mdn = null,
                    r_modem_reg = null,
                    r_led = null,
                    r_rf_in = null,
                    r_rf_ex = null,
                    r_gps = null,
                    r_gps_snr = null,
                    r_check_all = null,
                    r_cfginit = null,
                },
                new EXCEL_PARAM{},
            };
    public static List<EXCEL_PARAM> failDatas = new List<EXCEL_PARAM> {
                new EXCEL_PARAM
                {
                    r_product_number = null,
                    r_imei = null,
                    r_serial_number = null,
                    r_test_mode = null,
                    r_sflash = null,
                    r_sflash_rx = null,
                    r_modem_ver = null,
                    r_modem_iccid = null,
                    r_modem_rssi = null,
                    r_modem_mdn = null,
                    r_modem_reg = null,
                    r_led = null,
                    r_rf_in = null,
                    r_rf_ex = null,
                    r_gps = null,
                    r_gps_snr = null,
                    r_check_all = null,
                    r_cfginit = null,
                },
                new EXCEL_PARAM{},
            };
    public string r_product_number { get; set; }
    public string r_imei { get; set; }
    public string r_serial_number { get; set; }
    public string r_test_mode { get; set; }
    public string r_sflash { get; set; }
    public string r_sflash_rx { get; set; }
    public string r_modem_ver { get; set; }
    public string r_modem_iccid { get; set; }
    public string r_modem_rssi { get; set; }
    public string r_modem_mdn { get; set; }
    public string r_modem_reg { get; set; }
    public string r_led { get; set; }
    public string r_rf_in { get; set; }
    public string r_rf_ex { get; set; }
    public string r_gps { get; set; }
    public string r_gps_snr { get; set; }
    public string r_check_all { get; set; }
    public string r_cfginit { get; set; }


    public const int MAX_LOW_NUM = 17;

    public static string format;
    public static string succ_format;
    public static string fail_format;

    public static string fileName;
    public static int excelRow1 = 1;
    public static int excelRow2 = 1;
    public static bool bExcelOpen = false;

    public static int total1 = 1;
    public static int total2 = 1;

    public static ArrayList SuccData;
    public static ArrayList FailData;

    public static PARAM_TYPE[] type = new PARAM_TYPE[MAX_LOW_NUM]
    {
        PARAM_TYPE.ProducNumber,
        PARAM_TYPE.IMEI,
        PARAM_TYPE.SerialNumber,
        PARAM_TYPE.test_mode,
        PARAM_TYPE.sflash,
        PARAM_TYPE.sflash_rx,
        PARAM_TYPE.modem_ver,
        PARAM_TYPE.modem_iccid,
        PARAM_TYPE.modem_rssi,
        PARAM_TYPE.modem_mdn,
        PARAM_TYPE.modem_reg,
        PARAM_TYPE.led,
        PARAM_TYPE.rf_in,
        PARAM_TYPE.rf_ex,
        PARAM_TYPE.gps,
        PARAM_TYPE.gps_snr,
        PARAM_TYPE.check_all,
    };

    public static string[] EXCEL_PARARM = new string[MAX_LOW_NUM];
    public static bool[] EXCEL_SUCC = new bool[MAX_LOW_NUM];

    public enum PARAM_TYPE
    {
        ProducNumber = 0,
        IMEI = 1,
        SerialNumber = 2,
        test_mode = 3,
        sflash = 4,
        sflash_rx = 5,
        modem_ver = 6,
        modem_iccid = 7,
        modem_rssi = 8,
        modem_mdn = 9,
        modem_reg = 10,
        led = 11,
        rf_in = 13,
        rf_ex = 14,
        gps = 15,
        gps_snr = 16,
        check_all = 17,
    };
    public enum EXCEL_TYPE
    {
        EXCEL_PBA,
        EXCEL_FINAL
    };

    public enum EXCEL_DATA_TYPE
    {
        EXCEL_SUCC,
        EXCEL_FAIL
    };
}