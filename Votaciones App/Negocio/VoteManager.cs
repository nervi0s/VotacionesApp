using SunVote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Votaciones_App.Formularios;
using Votaciones_App.Views;

namespace Votaciones_App.Negocio
{
    // Clase encargada de manejar las instancias y los métodos de los objetos necesarios para realizar una votación
    class VoteManager
    {
        public delegate void CommunicatorDelegate(int stateMessage);
        public CommunicatorDelegate communicatorCallBack;

        public UserControlVoting votingPanel;

        private BaseConnection baseConn;
        private BaseManage baseManage;
        private KeypadManage keypadManage;

        private Choices choice;
        private TrueFalse trueFalse;

        private int connectionMode;
        private int baseID;

        private bool voteActive;

        private List<Mando> listaMandos;

        private CFichero ficheroCSV = new CFichero();

        private Thread cuentaAtrasThread;

        // ##############   Constructor  ############## \\
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

        // ##############   Public functions   ############## \\
        public void connectToAntena()
        {
            this.baseConn.Open(this.connectionMode, this.baseID.ToString());
        }

        public void setVotingPanel(UserControlVoting votingPanel)
        {
            this.votingPanel = votingPanel;
        }

        public bool isVoting()
        {
            return this.voteActive;
        }

        public void iniciaVotacion()
        {
            this.voteActive = true;

            // Ventana auxiliar de los votos
            this.votingPanel.getVentanaResultados().inicializa_grid();

            //Configuración del panel de votaciones
            this.votingPanel.setImageVoteStatus(Properties.Resources.verde);
            this.votingPanel.inicializarGrafica();

            // Programa externo manejado por el panel de votaciones
            if (CAjustes.conexion_grafismo)
            {
                this.votingPanel.envia_mensaje_progamaExterno("Play");
            }

            // Configura e incia los objetos encargados de manejar la votación
            setConfigAndStartVotes();

            // Iniciaciación de la cuenta atrás
            iniciarCuentaAtras();
        }

        public void finalizarVotacion()
        {
            this.voteActive = false;

            //Configuración del panel de votaciones
            this.votingPanel.setImageVoteStatus(Properties.Resources.rojo);

            // Detiene los objetos encargados de manejar la votación
            getConfigAndStopVotes();

            // Programa externo manejado por el panel de votaciones
            if (CAjustes.conexion_grafismo)
            {
                this.votingPanel.envia_mensaje_progamaExterno("Stop");
            }

            // Guardar los resultados en fichero
            guardarResultadosCSV();

            // Terminar la cuenta atrás
            finalizarCuentaAtras();

            // Terminar reconexion
            this.votingPanel.termina_reconexion_cliente();
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

            actualizarRecuento();
        }

        public void actualizarRecuento()
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

        public void apagarMandos()
        {
            this.keypadManage.RemoteOff(0);
        }

        // ##############   Private functions   ############## \\

        // Evento que maneja las llamadas del objeto que se representa a la Antena
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

        private void setConfigAndStartVotes()
        {
            switch (CAjustes.tipo_votacion)
            {
                case 0: // Votación de números
                    if (this.choice == null)
                    {
                        this.choice = new Choices();
                        this.choice.KeyStatus += new IChoicesEvents_KeyStatusEventHandler(onKeyRemotePressedDetected);
                    }

                    this.choice.BaseConnection = this.baseConn;
                    this.choice.OptionsMode = 1;
                    this.choice.ModifyMode = CAjustes.permitir_cambio_respuesta ? 1 : 0;
                    this.choice.SecrecyMode = 0;
                    this.choice.Options = CAjustes.numero_opciones;
                    this.choice.OptionalN = 1;
                    this.choice.StartMode = 1; // Limpia los mandos al empezar la votación

                    if (this.choice.Start() == "0") // Start() empieza la votación
                        Console.WriteLine("Votación números iniciada correctamente");

                    break;
                case 1: // Votación de letras
                    if (this.choice == null)
                    {
                        this.choice = new Choices();
                        this.choice.KeyStatus += new IChoicesEvents_KeyStatusEventHandler(onKeyRemotePressedDetected);
                    }

                    this.choice.BaseConnection = this.baseConn;
                    this.choice.OptionsMode = 0;
                    this.choice.ModifyMode = CAjustes.permitir_cambio_respuesta ? 1 : 0;
                    this.choice.SecrecyMode = 0;
                    this.choice.Options = CAjustes.numero_opciones;
                    this.choice.OptionalN = 1;
                    this.choice.StartMode = 1; // Limpia los mandos al empezar la votación

                    if (this.choice.Start() == "0") // Start() empieza la votación
                        Console.WriteLine("Votación letras iniciada correctamente");

                    break;
                case 2: // Votación de Verdadero o Falso
                    if (this.trueFalse == null)
                    {
                        this.trueFalse = new TrueFalse();
                        this.trueFalse.KeyStatus += new ITrueFalseEvents_KeyStatusEventHandler(onKeyRemotePressedDetected);
                    }

                    this.trueFalse.BaseConnection = this.baseConn;
                    this.trueFalse.Mode = 1;
                    this.trueFalse.ModifyMode = CAjustes.permitir_cambio_respuesta ? 1 : 0;
                    this.trueFalse.SecrecyMode = 0;
                    this.trueFalse.PromptMode = 1;
                    this.trueFalse.CorrectAnswer = "1";
                    this.trueFalse.StartMode = 1; // Limpia los mandos al empezar la votación

                    if (this.trueFalse.Start() == "0") // Start() empieza la votación
                        Console.WriteLine("Votación verdadero/falso iniciada correctamente");

                    break;
            }
        }

        private void getConfigAndStopVotes()
        {
            switch (CAjustes.tipo_votacion)
            {
                case 0: // Votación de números
                    if (this.choice != null)
                    {
                        Console.WriteLine("Votación números fin: " + this.choice.Stop());
                    }
                    break;
                case 1: // Votación de letras
                    if (this.choice != null)
                    {
                        Console.WriteLine("Votación letras fin: " + this.choice.Stop());
                    }
                    break;
                case 2: // Votación de Verdadero o Falso
                    if (this.trueFalse != null)
                    {
                        Console.WriteLine("Votación verdadero/falso fin: " + this.trueFalse.Stop());
                    }
                    break;
            }
        }

        // Método llamado cuando se detecta una pulsación de un mando de votación
        private void onKeyRemotePressedDetected(string id_base, int id_mando, string valor, double tiempo_respuesta)
        {
            if (!idAllowed(id_mando))
            {
                Console.WriteLine("ID del mando no permitido: " + id_mando);
                return;
            }

            // Paso de letra a número en caso de haber seleccionado el tipo de votación numérica
            if (CAjustes.tipo_votacion == 0)
                valor = parseLetter(valor);

            // Check si se permite o no cambio de respuesta
            if (CAjustes.permitir_cambio_respuesta)
            {
                if (CAjustes.permitir_multichoice)
                {
                    if (getMandoById(id_mando).cantidadRespuestas < Mando.NUMERO_OPCIONES_MAXIMAS)
                    {
                        if (!getMandoById(id_mando).respuesta.Split(';').Contains(valor))
                        {
                            getMandoById(id_mando).respondido = true;
                            getMandoById(id_mando).respuesta += ";" + valor;
                            getMandoById(id_mando).cantidadRespuestas++;
                        }
                        else
                        {
                            Console.WriteLine("Voto repetido realizado por el mando: " + id_mando);
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("El mando ya ha agotado su cantidad respuestas permitidas: " + id_mando);
                        return;
                    }
                }
                else
                {
                    getMandoById(id_mando).respondido = true;
                    getMandoById(id_mando).respuesta = ";" + valor;
                }
            }
            else
            {
                if (!getMandoById(id_mando).respondido)
                {
                    getMandoById(id_mando).respondido = true;
                    getMandoById(id_mando).respuesta = ";" + valor;
                }
                else
                {
                    Console.WriteLine("El mando ya hizo su voto: " + id_mando);
                    return;
                }
            }

            // Actualizo la informacion \\

            // Ventana auxiliar de los votos
            this.votingPanel.getVentanaResultados().actualizar_grid(this.listaMandos);
            // Panel de votaciones/
            actualizarRecuento();
            actualizarGrafico();
            // Crear fichero CSV
            guardarResultadosCSV();
        }


        // ##############   Auxiliary private functions   ############## \\
        private void iniciarCuentaAtras()
        {
            this.cuentaAtrasThread = new Thread(cuentaRegresiva);
            this.cuentaAtrasThread.Start();
        }

        private void finalizarCuentaAtras()
        {
            if (this.cuentaAtrasThread != null && this.cuentaAtrasThread.IsAlive)
            {
                this.cuentaAtrasThread.Abort();
            }
        }

        private void cuentaRegresiva()
        {
            while (int.Parse(this.votingPanel.getCronoTime()) > 0)
            {
                int cuenta = int.Parse(this.votingPanel.getCronoTime());
                Thread.Sleep(1000);
                cuenta--;
                this.votingPanel.Invoke(new EventHandler(delegate
                {
                    this.votingPanel.setCronoTime(cuenta.ToString());

                }));

                if (cuenta == 0)
                {
                    finalizarVotacion();
                }
            }
        }

        private void actualizarGrafico()
        {
            this.votingPanel.actualizarGrafica(this.listaMandos, recuentaVotados());
        }

        private void guardarResultadosCSV()
        {
            int tiempo = CAjustes.tiempo_crono - Convert.ToInt32(this.votingPanel.getCronoTime());
            string resultados = "Tiempo de votacion: " + tiempo + " s\n";

            foreach (Mando mando in this.listaMandos)
            {
                resultados = resultados + mando.getID() + (string.IsNullOrEmpty(mando.respuesta) ? ";" : mando.respuesta) + "\n";
            }
            Console.WriteLine(resultados);
            this.ficheroCSV.EscribeFichero(CAjustes.ruta_resultados + "Resultados.csv", false, resultados);
        }

        private bool idAllowed(int id)
        {
            return FormMandosConfig.createIDsList().Contains(id);
        }

        private string parseLetter(string letter)
        {
            switch (letter)
            {
                case "A":
                    return "1";
                case "B":
                    return "2";
                case "C":
                    return "3";
                case "D":
                    return "4";
                case "E":
                    return "5";
                case "F":
                    return "6";
                case "G":
                    return "7";
                case "H":
                    return "8";
                case "I":
                    return "9";
                case "J":
                    return "10";
                default:
                    return "_";
            }
        }

        private int recuentaVotados()
        {
            int contador = 0;

            foreach (Mando mando in this.listaMandos)
            {
                if (mando.respondido)
                    contador++;
            }

            return contador;
        }

        private Mando getMandoById(int id)
        {
            foreach (Mando mando in this.listaMandos)
            {
                if (mando.getID() == id)
                {
                    return mando;
                }
            }
            return null;
        }
    }
}
