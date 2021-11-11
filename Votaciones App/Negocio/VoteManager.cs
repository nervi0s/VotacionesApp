using SunVote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votaciones_App.Formularios;
using Votaciones_App.Views;

namespace Votaciones_App.Negocio
{
    class VoteManager
    {
        public delegate void CommunicatorDelegate(int stateMessage);
        public CommunicatorDelegate communicatorCallBack;

        public UserControlVoting votingPanel;

        private BaseConnection baseConn;
        private BaseManage baseManage;
        private KeypadManage keypadManage;
        private Choices _choice;
        private TrueFalse _truefalse;

        private int connectionMode;
        private int baseID;

        private bool voteActive;

        private List<Mando> listaMandos;

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
                        this.baseManage.SetBasicFeature(baseID, 0, 0, 0, 0, CAjustes.permitir_multichoice ? 0 : 1);
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
        public void setVotingPanel(UserControlVoting votingPanel)
        {
            this.votingPanel = votingPanel;
        }
        public bool isVoting()
        {
            return this.voteActive;
        }

        public void setVoteActive(bool value)
        {
            this.voteActive = value;
        }

        public void iniciaVotacion()
        {
            this.voteActive = true;

            // Ventana axiliar de los votos
            this.votingPanel.getVentanaResultados().inicializa_grid();

            //Configuración del panel de votaciones
            this.votingPanel.setImageVoteStatus(Properties.Resources.verde);
            this.votingPanel.inicializarGrafica();

        }

        public void resetVotacion()
        {
            // Vacío la lista de mandos
            if (listaMandos != null)
            {
                this.listaMandos.Clear();
            }

            // Relleno la lista de mandos
            this.listaMandos = new List<Mando>();
            List<int> ids = FormMandosConfig.createIDsList();
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                this.listaMandos.Add(new Mando(ids[i]));
            }

            //Configuración del panel de votaciones
            this.votingPanel.setCronoTime(CAjustes.tiempo_crono.ToString());
            this.votingPanel.setImageVoteStatus(Properties.Resources.rojo);

            actulizarRecuento();
        }

        public void actulizarRecuento()
        {
            if (this.votingPanel.getTipoRecuento() == 0)
            {
                double porcentaje = Convert.ToDouble(Decimal.Divide(recuentaVotados(), CAjustes.num_mandos)) * 100;
                this.votingPanel.setRecuento(Math.Round(porcentaje, 2) + "%");
            }
            else
            {
                this.votingPanel.setRecuento(recuentaVotados() + "/" + CAjustes.num_mandos);
            }
        }

        public int recuentaVotados()
        {
            int contador = 0;

            foreach (Mando mando in this.listaMandos)
            {
                if (mando.respondido)
                    contador++;
            }

            return contador;
        }
    }
}
