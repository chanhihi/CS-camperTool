using System;
using System.IO;
using System.Windows.Forms;

public class Log
{
    private static bool bOpen = false;
    private static StreamWriter sw;

    private enum Simulator_Name { PBA, FINAL };

    public static void LogOpen(int select)
    {
        string sDirPath = string.Empty;

        if (select == (int)Simulator_Name.PBA)
            sDirPath = Application.StartupPath + "\\PBA_40X";
        else
            sDirPath = Application.StartupPath + "\\FINAL_40X";

        DirectoryInfo di = new DirectoryInfo(sDirPath);

        if (di.Exists == false)
        {
            di.Create();
        }

        bOpen = true;

        if (select == (int)Simulator_Name.PBA)
            sw = new StreamWriter("PBA_40X\\pba40X_log_" + DateTime.Now.ToString("yyyyMM") + ".txt", true);
        else
            sw = new StreamWriter("FINAL_40X\\final40X_log_" + DateTime.Now.ToString("yyyyMM") + ".txt", true);
    }

    public static void LogOpen(string fname)
    {
        string sDirPath = Application.StartupPath + "\\Logs";
        DirectoryInfo di = new DirectoryInfo(sDirPath);

        if (di.Exists == false)
        {
            di.Create();
        }

        bOpen = true;
        sw = new StreamWriter("FINAL_40X\\" + fname + ".txt", true);
    }

    public static bool LogIsOpen()
    {
        return bOpen;
    }

    public static void LogWrite(string log)
    {
        if (bOpen != true)
        {
            LogOpen((int)Simulator_Name.FINAL);
        }

        sw.WriteLine(log);
    }

    public static void LogClose()
    {
        if (bOpen == true)
        {
            sw.Close();
        }

        bOpen = false;
    }
}


public class ModemLog
{
    private static bool bOpen = false;
    private static StreamWriter sw;

    public static void LogOpen()
    {
        string sDirPath = Application.StartupPath + "\\MODEM_40X";
        DirectoryInfo di = new DirectoryInfo(sDirPath);

        if (di.Exists == false)
        {
            di.Create();
        }

        bOpen = true;
        sw = new StreamWriter("MODEM_40X\\modem40X_" + DateTime.Now.ToString("yyyyMM") + ".txt", true);
    }

    public static void LogOpen(string fname)
    {
        string sDirPath = Application.StartupPath + "\\MODEM_40X";
        DirectoryInfo di = new DirectoryInfo(sDirPath);

        if (di.Exists == false)
        {
            di.Create();
        }

        bOpen = true;
        sw = new StreamWriter("MODEM_40X\\" + fname + ".txt", true);
    }

    public static bool LogIsOpen()
    {
        return bOpen;
    }

    public static void LogWrite(string log)
    {
        if (bOpen != true)
        {
            LogOpen();
        }

        sw.WriteLine(log);
    }

    public static void LogClose()
    {
        if (bOpen == true)
        {
            sw.Close();
        }

        bOpen = false;
    }
}