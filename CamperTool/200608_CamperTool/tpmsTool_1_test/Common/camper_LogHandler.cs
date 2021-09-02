using System;

public class TPMS_LOG_HANDLER
{
    public enum Simulator_Name { PBA, FINAL };

    public static void TPMS_LogOpen(int Select)
    {
        if (Log.LogIsOpen() != true)
        {
            Log.LogOpen(Select);
        }

        if ((Simulator_Name)Select == Simulator_Name.FINAL)
        {
            if (ModemLog.LogIsOpen() != true)
            {
                ModemLog.LogOpen();
            }
        }
    }

    public static void TPMS_LogWrite(string data_log, string modem_log)
    {
        if (data_log != string.Empty && data_log != "")
        {
            if (Log.LogIsOpen())
                Log.LogWrite(data_log);
        }

        if (modem_log != string.Empty && modem_log != "")
        {
            if (ModemLog.LogIsOpen())
                ModemLog.LogWrite(modem_log);
        }
    }

    public static void TPMS_LogClose()
    {
        Log.LogClose();
        ModemLog.LogClose();
    }

    public static string build_result_msg(int Select)
    {
        string update_data = string.Empty;

        update_data = "=======" + DateTime.Now.ToString("yy:MM:dd-hh:mm:ss") + "=======\r\n";
        update_data += string.Format("Product No.   : {0}\r\n", tpms_LOG.ProducNumber);
        update_data += string.Format("IMEI          : {0}\r\n", tpms_LOG.IMEI);
        update_data += string.Format("Serial Number : {0}\r\n", tpms_LOG.SerialNumber);
        update_data += string.Format("testmode Ver  : {0}\r\n", tpms_LOG.r_test_mode == true ? "OK" : "NOK");
        update_data += string.Format("sFlash tx     : {0}\r\n", tpms_LOG.r_sflash);
        update_data += string.Format("sFlash rx     : {0}\r\n", tpms_LOG.r_sflash_rx);
        update_data += string.Format("Modem Version : {0}\r\n", tpms_LOG.r_modem_ver == true ? "OK" : "NOK");
        update_data += string.Format("Modem iccid   : {0}\r\n", tpms_LOG.r_modem_iccid);
        update_data += string.Format("Modem RSSI    : {0}\r\n", tpms_LOG.r_modem_rssi);
        update_data += string.Format("Modem MDN     : {0}\r\n", tpms_LOG.r_modem_mdn == true ? "OK" : "NOK");
        update_data += string.Format("Modem REG     : {0}\r\n", tpms_LOG.r_modem_reg == true ? "OK" : "NOK");
        update_data += string.Format("LED           : {0}\r\n", tpms_LOG.r_led == true ? "OK" : "NOK");
        update_data += string.Format("RF_in         : {0}\r\n", tpms_LOG.r_rf_in == true ? "OK" : "NOK");
        update_data += string.Format("RF_ex         : {0}\r\n", tpms_LOG.r_rf_ex == true ? "OK" : "NOK");
        update_data += string.Format("GPS           : {0}\r\n", tpms_LOG.r_gps == true ? "OK" : "NOK");
        update_data += string.Format("GPS-SNR       : {0}\r\n", tpms_LOG.r_gps_snr);
        update_data += string.Format("Check All     : {0}\r\n", tpms_LOG.r_check_all == true ? "OK" : "NOK");
        update_data += string.Format("Config Init   : {0}\r\n", tpms_LOG.r_cfginit == true ? "OK" : "NOK");
        update_data += "===============================\r\n\r\n";

        return update_data;
    }

    public static string build_log_msg()
    {
        string log_data = string.Empty;

        log_data = string.Format("$EXCEL,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
            tpms_LOG.ProducNumber, tpms_LOG.IMEI, tpms_LOG.SerialNumber, tpms_LOG.r_test_mode, tpms_LOG.r_sflash, tpms_LOG.r_sflash_rx,
            tpms_LOG.r_modem_ver, tpms_LOG.r_modem_iccid, tpms_LOG.r_modem_rssi, tpms_LOG.r_modem_mdn, tpms_LOG.r_modem_reg,
            tpms_LOG.r_led, tpms_LOG.r_rf_in, tpms_LOG.r_rf_ex, tpms_LOG.r_gps, tpms_LOG.r_gps_snr, tpms_LOG.r_check_all, tpms_LOG.r_cfginit);

        return log_data;
    }

    public static string build_modem_msg()
    {
        string modem_data = string.Empty;

        modem_data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
            tpms_LOG.modem_param[0], tpms_LOG.r_id, tpms_LOG.modem_param[1], tpms_LOG.modem_param[2], tpms_LOG.modem_param[3],
            tpms_LOG.modem_param[4], tpms_LOG.modem_param[5], tpms_LOG.modem_param[6], tpms_LOG.modem_param[7], tpms_LOG.modem_param[8],
            tpms_LOG.modem_imei, tpms_LOG.usim_ccid);

        return modem_data;
    }
}