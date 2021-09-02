using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace camperTool_TEST.Class
{
    public class xlData
    {
        camperTest test = new camperTest();

        public static List<camperTest> id = new List<camperTest> {
                new camperTest
                {
                    PRODUCTNUMBER = null,
                    IMEI = null,
                    SERIALNUMBER = null
                },
                new camperTest{},
            };

        public static OpenFileDialog OpenDlg = new OpenFileDialog();

        public static bool fCheck = false;
        public static int fCount;

        public static void includeExcel(IEnumerable<xlData> datas)
        {
            OpenDlg.InitialDirectory = "./";
            OpenDlg.Filter = "xlsx Files|*.xlsx|All Files|*.*";
            OpenDlg.FilterIndex = 1;
            OpenDlg.RestoreDirectory = true;

            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                if (OpenDlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fContent = string.Empty;
                        var fPath = string.Empty;
                        fPath = OpenDlg.FileName;
                        var fStream = OpenDlg.OpenFile();


                        excelApp = new Excel.Application();
                        wb = excelApp.Workbooks.Open(fPath, null, true);
                        ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                        excelApp.Visible = false;
                        excelApp.DisplayAlerts = false;

                        //                        MessageBox.Show(fContent, "File Content at path: " + fPath, MessageBoxButtons.OK);

                        using (StreamReader reader = new StreamReader(fStream))
                        {
                            fContent = reader.ReadToEnd();
                            fCheck = false;
                            fCount = ws.UsedRange.Rows.Count;
                        }


                        int fTempC = fCount;

                        while (fTempC > 1)
                        {
                            fTempC--;
                            if (id.Count < 201)
                                id.Add(new camperTest());
                            else
                                break;

                        }

                        if (!fCheck)
                        {
                            ws.Cells[2, "A"] = "일체형 장치 Final(include)";
                            ws.Cells[5, "B"] = "기기별 구분";
                            ws.Cells[5, "A"] = "NO";
                            ws.Cells[7, "B"] = "ID No.";
                            ws.Cells[7, "D"] = "ETC";
                            ws.Cells[10, "B"] = "IMEI";
                            ws.Cells[10, "C"] = "SerialNumber";
                        }

                        try
                        {
                            int row = 11;
                            for (int i = 1; i < id.Count; i++)
                            {
                                row++;
                                if (row == 212)
                                    break;

                                ws.Cells[row, "D"] = "Read";

                                id[i].PRODUCTNUMBER = ws.Cells[row, "A"].Value.ToString();
                                id[i].IMEI = ws.Cells[row, "B"].Value.ToString();
                                id[i].SERIALNUMBER = ws.Cells[row, "C"].Value.ToString();


                                //ws.Cells[row, "E"] = id[i].PRODUCTNUMBER;
                                //ws.Cells[row, "F"] = id[i].IMEI ;
                                //ws.Cells[row, "G"] = id[i].SERIALNUMBER ;


                            }
                        }
                        catch
                        {
                            MessageBox.Show("ERRORR!!");
                        }
                        finally
                        {
                            MessageBox.Show("ReadSuccess ");
                        }

                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + x.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            //worksheet.Columns[1].AutoFit();
            //worksheet.Columns[2].AutoFit();
            finally
            {
                fCheck = true;
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }
        public static void exportExcel(IEnumerable<xlData> datas)
        {
            if (fCheck)
            {
                OpenDlg.InitialDirectory = "./";
                OpenDlg.Filter = "xlsx Files|*.xlsx|All Files|*.*";
                OpenDlg.FilterIndex = 2;
                OpenDlg.RestoreDirectory = true;

                Excel.Application excelApp = null;
                Excel.Workbook wb = null;
                Excel.Worksheet ws1 = null;
                Excel.Worksheet ws2 = null;
                try
                {
                    if (OpenDlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var fContent = string.Empty;
                            var fPath = string.Empty;
                            fPath = OpenDlg.FileName;
                            //MessageBox.Show(fContent, "File Content at path: " + fPath, MessageBoxButtons.OK);

                            var fStream = OpenDlg.OpenFile();
                            excelApp = new Excel.Application();
                            wb = excelApp.Workbooks.Open(fPath, 0, false);

                            excelApp.Visible = true;
                            excelApp.DisplayAlerts = false;
                            excelApp.Interactive = true;

                            ws1 = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                            ws1.Name = "성공";

                            ws2 = wb.Worksheets.get_Item(2) as Excel.Worksheet;
                            ws2.Name = "실패";

                            while (fCount > 1)
                            {
                                fCount--;
                                id.Add(new camperTest());
                            }

                            if (fCheck)
                            {
                                ws1.Cells[2, "A"] = "일체형 장치 Final(Export)";
                                ws1.Cells[2, "B"] = "출하 검사 항목 구분";
                                ws1.Cells[5, "A"] = "NO";
                                ws1.Cells[7, "B"] = "ID No.";
                                ws1.Cells[7, "D"] = "MODE";
                                ws1.Cells[7, "E"] = "sFlash";
                                ws1.Cells[7, "G"] = "RF";
                                ws1.Cells[7, "I"] = "GPS";
                                ws1.Cells[7, "K"] = "LED";
                                ws1.Cells[7, "L"] = "MODEM";
                                ws1.Cells[7, "P"] = "INIT";
                                ws1.Cells[7, "Q"] = "CheckAll";
                                ws1.Cells[10, "B"] = "IMEI";
                                ws1.Cells[10, "C"] = "S/N";
                                ws1.Cells[10, "E"] = "RX";
                                ws1.Cells[10, "F"] = "TX";
                                ws1.Cells[10, "G"] = "485Port";
                                ws1.Cells[10, "H"] = "RF IN";
                                ws1.Cells[10, "I"] = "Lock";
                                ws1.Cells[10, "J"] = "SNR";
                                ws1.Cells[10, "K"] = "SUCCESS";
                                ws1.Cells[10, "L"] = "ICCID";
                                ws1.Cells[10, "M"] = "RSSI";
                                ws1.Cells[10, "N"] = "MDN";
                                ws1.Cells[10, "O"] = "REG";
                                ////////////////////////////////////////////////////////
                                ws2.Cells[2, "A"] = "일체형 장치 Final(Failure) ";
                                ws2.Cells[2, "B"] = "불량 검사 항목 구분";
                                ws2.Cells[5, "A"] = "NO";
                                ws2.Cells[7, "B"] = "ID No.";
                                ws2.Cells[7, "D"] = "MODE";
                                ws2.Cells[7, "E"] = "sFlash";
                                ws2.Cells[7, "G"] = "RF";
                                ws2.Cells[7, "I"] = "GPS";
                                ws2.Cells[7, "K"] = "LED";
                                ws2.Cells[7, "L"] = "MODEM";
                                ws2.Cells[7, "P"] = "INIT";
                                ws2.Cells[7, "Q"] = "CheckAll";
                                ws2.Cells[10, "B"] = "IMEI";
                                ws2.Cells[10, "C"] = "S/N";
                                ws2.Cells[10, "E"] = "RX";
                                ws2.Cells[10, "F"] = "TX";
                                ws2.Cells[10, "G"] = "485Port";
                                ws2.Cells[10, "H"] = "RF IN";
                                ws2.Cells[10, "I"] = "Lock";
                                ws2.Cells[10, "J"] = "SNR";
                                ws2.Cells[10, "K"] = "FAIL";
                                ws2.Cells[10, "L"] = "ICCID";
                                ws2.Cells[10, "M"] = "RSSI";
                                ws2.Cells[10, "N"] = "MDN";
                                ws2.Cells[10, "O"] = "REG";
                            }
                            int row = 10;
                            try
                            {
                                for (int i = 1; i < EXCEL_PARAM.succDatas.Count; i++)
                                {
                                    row++;
                                    if (row == 212)
                                        break;
                                    ws1.Cells[row, "A"] = EXCEL_PARAM.succDatas[i].r_product_number;
                                    ws1.Cells[row, "B"] = EXCEL_PARAM.succDatas[i].r_imei;
                                    ws1.Cells[row, "C"] = EXCEL_PARAM.succDatas[i].r_serial_number;
                                    ws1.Cells[row, "D"] = EXCEL_PARAM.succDatas[i].r_test_mode;
                                    ws1.Cells[row, "E"] = EXCEL_PARAM.succDatas[i].r_sflash_rx;
                                    ws1.Cells[row, "F"] = EXCEL_PARAM.succDatas[i].r_sflash;

                                    ws1.Cells[row, "G"] = EXCEL_PARAM.succDatas[i].r_rf_ex;
                                    ws1.Cells[row, "H"] = EXCEL_PARAM.succDatas[i].r_rf_in;

                                    ws1.Cells[row, "I"] = EXCEL_PARAM.succDatas[i].r_gps;
                                    ws1.Cells[row, "J"] = EXCEL_PARAM.succDatas[i].r_gps_snr;

                                    ws1.Cells[row, "K"] = EXCEL_PARAM.succDatas[i].r_led;

                                    //ws1.Cells[row, "D"] = EXCEL_PARAM.succDatas[i].r_modem_ver;
                                    ws1.Cells[row, "L"] = EXCEL_PARAM.succDatas[i].r_modem_iccid;
                                    ws1.Cells[row, "M"] = EXCEL_PARAM.succDatas[i].r_modem_rssi;
                                    ws1.Cells[row, "N"] = EXCEL_PARAM.succDatas[i].r_modem_mdn;
                                    ws1.Cells[row, "O"] = EXCEL_PARAM.succDatas[i].r_modem_reg;

                                    ws1.Cells[row, "P"] = EXCEL_PARAM.succDatas[i].r_cfginit;
                                    ws1.Cells[row, "Q"] = EXCEL_PARAM.succDatas[i].r_check_all;
                                }

                                row = 10;

                                for (int i = 1; i < EXCEL_PARAM.failDatas.Count; i++)
                                {
                                    row++;
                                    if (row == 50)
                                        break;

                                    ws2.Cells[row, "A"] = EXCEL_PARAM.failDatas[i].r_product_number;
                                    ws2.Cells[row, "B"] = EXCEL_PARAM.failDatas[i].r_imei;
                                    ws2.Cells[row, "C"] = EXCEL_PARAM.failDatas[i].r_serial_number;
                                    ws2.Cells[row, "D"] = EXCEL_PARAM.failDatas[i].r_test_mode;
                                    ws2.Cells[row, "E"] = EXCEL_PARAM.failDatas[i].r_sflash_rx;
                                    ws2.Cells[row, "F"] = EXCEL_PARAM.failDatas[i].r_sflash;
                                    ws2.Cells[row, "G"] = EXCEL_PARAM.failDatas[i].r_rf_ex;
                                    ws2.Cells[row, "H"] = EXCEL_PARAM.failDatas[i].r_rf_in;
                                    ws2.Cells[row, "I"] = EXCEL_PARAM.failDatas[i].r_gps;
                                    ws2.Cells[row, "J"] = EXCEL_PARAM.failDatas[i].r_gps_snr;
                                    ws2.Cells[row, "K"] = EXCEL_PARAM.failDatas[i].r_led;
                                    //ws2.Cells[row, "D"] = EXCEL_PARAM.failDatas[i].r_modem_ver;
                                    ws2.Cells[row, "L"] = EXCEL_PARAM.failDatas[i].r_modem_iccid;
                                    ws2.Cells[row, "M"] = EXCEL_PARAM.failDatas[i].r_modem_rssi;
                                    ws2.Cells[row, "N"] = EXCEL_PARAM.failDatas[i].r_modem_mdn;
                                    ws2.Cells[row, "O"] = EXCEL_PARAM.failDatas[i].r_modem_reg;
                                    ws2.Cells[row, "P"] = EXCEL_PARAM.failDatas[i].r_cfginit;
                                    ws2.Cells[row, "Q"] = EXCEL_PARAM.failDatas[i].r_check_all;
                                }
                            }
                            catch
                            {
                                MessageBox.Show("xlParam ERRORR!!");
                            }
                            finally
                            {
                                MessageBox.Show("Write Success ");
                            }
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show("Error: Could not read file from disk. Original error: " + x.ToString());
                        }
                        finally
                        {
                            ReleaseExcelObject(ws1);
                            ReleaseExcelObject(ws2);
                            ReleaseExcelObject(wb);
                            ReleaseExcelObject(excelApp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
                finally
                {
                    //fCheck = false;
                }
            }

            else
            {
                MessageBox.Show("Error: Could not read Include Excel. please check input");
            }
        }
        public static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
        public static string CheckData = "check,Data,init";
        public void putSheet(string log_data)
        {
            //var result = id.FindIndex(1, x => x.IMEI == x.IMEI_txt_Box.Text);
            //var result = id.FindIndex(1, x => x.PRODUCTNUMBER == x.productNumb.Text);
            //int o = (int)test.productNumb.Value;
            try
            {
                string[] temp = CheckData.Split(',');
                string[] result = log_data.Split(',');

                if (log_data.Contains("False"))
                {

                    if (temp[1] == result[1] && temp[17] == result[17])
                    {
                        MessageBox.Show("같은 번호가 검색 됐습니다. \r\n이전 결과에 덮어 씌웁니다.");
                        EXCEL_PARAM.failDatas.RemoveAt(EXCEL_PARAM.failDatas.Count() - 1);
                    }
                    if (temp[1] == result[1] && temp[17] != result[17])
                    {
                        MessageBox.Show("같은 번호가 검색 됐습니다. \r\n이전 결과에 덮어 씌웁니다.");
                        EXCEL_PARAM.succDatas.RemoveAt(EXCEL_PARAM.succDatas.Count() - 1);
                    }
                    EXCEL_PARAM.failDatas.Add(
                        new EXCEL_PARAM
                        {
                            r_product_number = result[1],
                            r_imei = result[2],
                            r_serial_number = result[3],
                            r_test_mode = result[4],
                            r_sflash = result[5],
                            r_sflash_rx = result[6],
                            r_modem_ver = result[7],
                            r_modem_iccid = result[8],
                            r_modem_rssi = result[9],
                            r_modem_mdn = result[10],
                            r_modem_reg = result[11],
                            r_led = result[12],
                            r_rf_in = result[13],
                            r_rf_ex = result[14],
                            r_gps = result[15],
                            r_gps_snr = result[16],
                            r_check_all = result[17],
                            r_cfginit = result[18],
                        });
                }
                else
                {
                    if (temp[1] == result[1] && temp[17] == result[17])
                    {
                        MessageBox.Show("같은 번호가 검색 됐습니다. \r\n이전 결과에 덮어 씌웁니다.");
                        EXCEL_PARAM.succDatas.RemoveAt(EXCEL_PARAM.succDatas.Count() - 1);
                    }
                    if (temp[1] == result[1] && temp[17] != result[17])
                    {
                        MessageBox.Show("같은 번호가 검색 됐습니다. \r\n이전 결과에 덮어 씌웁니다.");
                        EXCEL_PARAM.failDatas.RemoveAt(EXCEL_PARAM.failDatas.Count() - 1);
                    }
                    EXCEL_PARAM.succDatas.Add(new EXCEL_PARAM
                    {
                        r_product_number = result[1],
                        r_imei = result[2],
                        r_serial_number = result[3],
                        r_test_mode = result[4],
                        r_sflash = result[5],
                        r_sflash_rx = result[6],
                        r_modem_ver = result[7],
                        r_modem_iccid = result[8],
                        r_modem_rssi = result[9],
                        r_modem_mdn = result[10],
                        r_modem_reg = result[11],
                        r_led = result[12],
                        r_rf_in = result[13],
                        r_rf_ex = result[14],
                        r_gps = result[15],
                        r_gps_snr = result[16],
                        r_check_all = result[17],
                        r_cfginit = result[18],
                    });
                }
            }
            catch
            {
                MessageBox.Show("xl Param list Error");
            }
            finally
            {
                CheckData = log_data;
            }
        }
    }
}

