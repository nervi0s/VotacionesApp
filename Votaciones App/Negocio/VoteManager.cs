using SunVote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votaciones_App.Negocio
{
    class VoteManager
    {
        public delegate void CommunicatorDelegate(int stateMessage);
        public CommunicatorDelegate communicatorCallBack;

        private BaseConnection baseConn;
        private BaseManage baseManage;
        private KeypadManage keypadManage;
        private Choices _choice;
        private TrueFalse _truefalse;

        private int connectionMode;
        private int baseID;

        public VoteManager(int baseID, int connectionMode)
        {
            this.baseID = baseID;
            this.connectionMode = connectionMode;

            this.baseConn = new BaseConnection()
            {
                DemoMode = false,
                IsWriteErrorLog = false,
                ProtocolType = 1,
                BaseIP = CAjustes.ip_antena
            };

            this.baseConn.BaseOnLine += new IBaseConnectionEvents_BaseOnLineEventHandler(baseConn_BaseOnline);
        }

        private void baseConn_BaseOnline(int baseID, int baseState)
        {
            string sState = string.Empty;
            string sMsg = string.Empty;

            try
            {
                switch (baseState)
                {
                    case 0:
                        sState = "Fallo de conexión con el host o conexion finalizada";
                        break;
                    case 1:
                        sState = "Conexión con la base exitosa";
                        this.keypadManage = new KeypadManage();
                        this.keypadManage.BaseConnection = this.baseConn;
                        this.baseManage = new BaseManage();
                        this.baseManage.BaseConnection = this.baseConn;
                        this.baseManage.SetBasicFeature(1, 0, 0, 0, 0, 1);
                        break;
                    case -1:
                        sState = "No puede soportarse este tipo de conexión";
                        break;
                    case -2:
                        sState = "No se encuentra la base";
                        break;
                    case -3:
                        sState = "Error del puerto";
                        break;
                    case -4:
                        sState = "la conexión ha sido cerrada";
                        break;
                }

                this.communicatorCallBack(baseState); // Comunicación al Form Principal

                sMsg = "baseConn_BaseOnLine: " + "BaseID= " + baseID + ", BaseState= " + baseState + "  " + sState;
                Console.WriteLine(sMsg);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        public void connectToAntena()
        {
            this.baseConn.Open(this.connectionMode, this.baseID.ToString());
        }

        public BaseConnection getBaseConnection()
        {
            return this.baseConn;
        }
    }
}
