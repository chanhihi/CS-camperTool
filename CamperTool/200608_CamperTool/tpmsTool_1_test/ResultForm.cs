using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace camperTool_TEST
{
    public partial class ResultForm : Form
    {
        //Log msg class
        public class LOG_MSG_INFO
        {
            public Color color;
            public string msg;
            public bool bAddCrLf;
        };

        camperTest MForm;

        // Message delegate
        public delegate void OnRxDebugEvent(string msg);
        public OnRxDebugEvent rxDebugMsg = null;

        ArrayList debugMsgList = new ArrayList();
        bool m_bPauseSts = false;

        public ResultForm()
        {
            InitializeComponent();
        }

        public ResultForm(camperTest MF)
        {
            InitializeComponent();
            MForm = MF;
        }

        public void update_debug_msg(Color color, string msg, bool bAddCrLf)
        {
            if (m_bPauseSts == false)
            {
                if (debugTxtBox.InvokeRequired)
                {
                    debugTxtBox.Invoke(new EventHandler(delegate
                    {
                        debugTxtBox.SelectedText = string.Empty;
                        debugTxtBox.SelectionColor = color;
                        debugTxtBox.AppendText(msg);

                        if (bAddCrLf)
                            debugTxtBox.AppendText("\r\n");

                        debugTxtBox.ScrollToCaret();
                    }));
                }
                else
                {
                    debugTxtBox.SelectedText = string.Empty;
                    debugTxtBox.SelectionColor = color;
                    debugTxtBox.AppendText(msg);

                    if (bAddCrLf)
                        debugTxtBox.AppendText("\r\n");

                    debugTxtBox.ScrollToCaret();
                }
            }
            else
            {
                AddPauseMsg(color, msg, bAddCrLf);
            }
        }

        public void UpdateCrLf()
        {
            if (m_bPauseSts == false)
            {
                if (debugTxtBox.InvokeRequired)
                {
                    debugTxtBox.Invoke(new EventHandler(delegate
                    {
                        debugTxtBox.SelectedText = string.Empty;
                        debugTxtBox.AppendText("\r\n");

                        debugTxtBox.ScrollToCaret();
                    }));
                }
                else
                {
                    debugTxtBox.SelectedText = string.Empty;
                    debugTxtBox.AppendText("\r\n");

                    debugTxtBox.ScrollToCaret();
                }
            }
        }

        private void AddPauseMsg(Color color, string msg, bool bAddCrLf)
        {
            LOG_MSG_INFO logMsgInfo = new LOG_MSG_INFO();

            logMsgInfo.color = color;
            logMsgInfo.msg = msg;
            logMsgInfo.bAddCrLf = bAddCrLf;

            debugMsgList.Add(logMsgInfo);
        }

        private void DisplayPauseMsg()
        {
            if (debugMsgList.Count == 0)
                return;

            foreach (object obj in debugMsgList)
            {
                LOG_MSG_INFO logMsgInfo = new LOG_MSG_INFO();

                logMsgInfo = (LOG_MSG_INFO)obj;

                update_debug_msg(logMsgInfo.color, logMsgInfo.msg, logMsgInfo.bAddCrLf);
            }

            debugMsgList.Clear();
        }


    }
}
