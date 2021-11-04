using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SunVoteSDK_Demo
{
    public partial class frmMain : Form
    {
        SunVote.BaseConnection baseConn;
        SunVote.SignIn signIn;
        SunVote.Number number;

        public frmMain()
        {
            InitializeComponent();

            //Create SDK Object
            baseConn = new SunVote.BaseConnection();
            baseConn.DemoMode = false;
            baseConn.IsWriteErrorLog = false;


            number = new SunVote.Number();
            number.BaseConnection = baseConn;
            number.Mode = 0;
            number.ModifyMode = 0;
            number.SecrecyMode = 0;
            number.Min = 0;
            number.Max = 9;
            number.StartMode = 1;

            signIn = new SunVote.SignIn();
            signIn.BaseConnection = baseConn;

            //Bound events
            baseConn.BaseOnLine += new SunVote.IBaseConnectionEvents_BaseOnLineEventHandler(baseConn_BaseOnLine);
            signIn.KeyStatus += new SunVote.ISignInEvents_KeyStatusEventHandler(signIn_KeyStatus);
            number.KeyStatus += new SunVote.INumberEvents_KeyStatusEventHandler(number_keyStatus);

        }



        void number_keyStatus(string id_base, int id_mando, string valor, double tiempo_respuesta)
        {
            string lState = "BaseTag= " + id_base + ",KeyID= " + id_mando + " ,ValueType= " + valor + " ,KeyValue= " + tiempo_respuesta;
            ShowMsg("signIn_KeyStatus:" + lState);
        }


        /// <summary>
        /// Signin event
        /// </summary>
        /// <param name="KeyID"></param>
        /// <param name="SignInType"></param>
        /// <param name="KeyValue"></param>
        void signIn_KeyStatus(string BaseTag,int KeyID, int ValueType, string KeyValue)
        {
            string lState = "";
            lState = "BaseTag=" + BaseTag +",KeyID=" + KeyID + 
                       ",ValueType=" + ValueType + 
                       ",KeyValue=" + KeyValue;
            ShowMsg("signIn_KeyStatus:" + lState);
        }

        /// <summary>
        /// Connected event
        /// </summary>
        /// <param name="BaseID">Base station ID</param>
        /// <param name="BaseState">Connection status</param>
        void baseConn_BaseOnLine(int BaseID, int BaseState)
        {
            string sState="";
            string sMsg = "";

            try
            {
                switch (BaseState)
                {
                    case 0:
                        sState="Connect Base Failure or BaseConnection Close!";
                        break;
                    case 1:
                        sState="Connect Base Success!";
                        break;
                    case -1:
                        sState="Can Not Support The ConnectType!";
                        break;
                    case -2:
                        sState = "Can not find Base!";
                        break;
                    case -3:
                        sState = "Port Error!";
                        break;
                    case -4:
                        sState = "The Baseconnection has been closed!";
                        break;
                }

                sMsg ="baseConn_BaseOnLine: " + "BaseID= " + BaseID + ", BaseState= " + BaseState + "  " + sState;
                ShowMsg(sMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnOpenConn_Click(object sender, EventArgs e)
        {
            //Connect base station.
            baseConn.Open(1, "1"); //usb Connect
        }

        private void btnCloseConn_Click(object sender, EventArgs e)
        {
            //Disconnect base station
            baseConn.Close();
        }

        private void btnStartSignIn_Click(object sender, EventArgs e)
        {
            signIn.Mode = 0;
            signIn.StartMode = 1;
            //Start Signin
            signIn.Start();
            string lState = signIn.Start();
            ShowMsg("signIn.Start:" + lState);
        }

        private void btnStopSignIn_Click(object sender, EventArgs e)
        {
            //Stop Signin
            string lState = signIn.Stop();
            ShowMsg ("signIn.Stop:" + lState);
            
        }

        private void ShowMsg(String Msg)
        {
            lstMsg.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss fff") + "  " + Msg);
        }

        private void button_vota_Click(object sender, EventArgs e)
        {
            Console.WriteLine(number.Start());
        }

        private void button_stopvota_Click(object sender, EventArgs e)
        {

        }
    }
}
