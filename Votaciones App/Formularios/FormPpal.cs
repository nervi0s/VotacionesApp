using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;
using SunVote;
using System.Net;
using System.Net.Sockets;
using Votaciones_App.Views;
using System.Text.RegularExpressions;
using Votaciones_App.Formularios;

namespace Votaciones_App
{
    public partial class FormPpal : Form
    {
        UserControlSettings settingsPanel;
        UserControlVoting votingPanel;

        private int connectionMode;
        private bool enableMultichoice;
        BaseConnection baseConn;
        BaseManage baseManage;
        KeypadManage keypadManage;

        Choices _choice;
        TrueFalse _truefalse;

        CFichero _ficherocsv = new CFichero();
        CFileXML xmlFile = new CFileXML();
        List<Mando> lista_mandos;
        int tipo_recuento = 0;
        int estado_votacion = 0;
        Thread cuentaAtras;
        Thread aleatorio;
        int[] recuenta_opciones;
        string[] array_letras = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        string[] array_numeros = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        string[] array_verdaderofalso = new String[] { "True", "False" };
        FormResultados ventana_resultados = null;
        bool unregistered;
        IPEndPoint endpoint;
        Socket socket;
        int estado_conexion_grafismo;
        Thread reconexion;


        public BaseConnection getBase()
        {
            return this.baseConn;
        }
        public FormPpal()
        {
            InitializeComponent();
        }

        private void connectToBaseAntena()
        {
            this.baseConn = new BaseConnection
            {
                DemoMode = false,
                IsWriteErrorLog = false,
                ProtocolType = 1
            };

            this.baseConn.BaseOnLine += new IBaseConnectionEvents_BaseOnLineEventHandler(baseConn_BaseOnLine);

        }

        private void createKeyPadManager()
        {
            this.keypadManage = new KeypadManage();
        }

        /*
         *  Carga incial al arrancar el programa
         */
        private void FormPpal_Load(object sender, EventArgs e)
        {
            // BaseConnection object
            connectToBaseAntena();
            createKeyPadManager();

            loadInitalPanel();

            //carga_ajustes();
            //ReloadLicense();

            // BaseConn
            //this.baseConn = new SunVote.BaseConnection();
            //this.baseConn.DemoMode = false;
            //this.baseConn.DemoKeyIDs = "1-131"; // Para pruebas
            //this.baseConn.IsWriteErrorLog = false;
            //this.baseConn.BaseOnLine += new SunVote.IBaseConnectionEvents_BaseOnLineEventHandler(baseConn_BaseOnLine);
            //this.baseConn.Open(1, Convert.ToString(CAjustes.base_antena)); //usb Connect                      
            //this._keypadManage = new KeypadManage();
            //ventana_resultados = new FormResultados(new Point(this.Left + this.Width, this.Top)); 
        }


        /// <param name="BaseID">Base station ID</param>
        /// <param name="BaseState">Connection status</param>
        void baseConn_BaseOnLine(int BaseID, int BaseState)
        {
            string sState = "";
            string sMsg = "";

            try
            {
                switch (BaseState)
                {
                    case 0:
                        sState = "Fallo de conexión con el host o conexion finalizada";
                        this.settingsPanel.setEnableButtonAceptar(false);
                        this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                        break;
                    case 1:
                        sState = "Conexión con la base exitosa";
                        //this.button_apagar_mandos.Enabled = true;

                        this.keypadManage.BaseConnection = this.baseConn;

                        this.baseManage = new BaseManage();
                        this.baseManage.BaseConnection = this.baseConn;
                        this.baseManage.SetBasicFeature(BaseID, 0, 0, 0, 0, this.enableMultichoice ? 0 : 1);

                        this.settingsPanel.setEnableButtonAceptar(true);
                        this.settingsPanel.setImageConnectionStatus(Properties.Resources.verde);
                        break;
                    case -1:
                        sState = "No puede soportarse este tipo de conexión";
                        //this.button_apagar_mandos.Enabled = false;
                        this.settingsPanel.setEnableButtonAceptar(false);
                        this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                        break;
                    case -2:
                        sState = "No se encuentra la base";
                        //this.button_apagar_mandos.Enabled = false;
                        this.settingsPanel.setEnableButtonAceptar(false);
                        this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                        break;
                    case -3:
                        sState = "Error del puerto";
                        //this.button_apagar_mandos.Enabled = false;
                        this.settingsPanel.setEnableButtonAceptar(false);
                        this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                        break;
                    case -4:
                        sState = "la conexión ha sido cerrada";
                        break;
                    case -5:
                        sState = "Base usada por la aplicación: " + this.baseConn.BaseUsedByApp;
                        break;
                }

                sMsg = "baseConn_BaseOnLine: " + "BaseID= " + BaseID + ", BaseState= " + BaseState + "  " + sState;
                Console.WriteLine(sMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /*
         * Se ejecuta al cerrar el programa
         */
        private void FormPpal_FormClosed(object sender, FormClosedEventArgs e)
        {
            finaliza_votacion();
            this.baseConn.Close();
            string message = "Cerrando Aplicación";
            string title = "Info";
            MessageBox.Show(message, title, MessageBoxButtons.OK);
        }



        /*
         *  Cambia al panel de ajustes
         */
        private void panel_go_ajustes_Click(object sender, EventArgs e)
        {
            finaliza_votacion();
            cambia_panel("ajustes");
        }


        /*
         *  Pone a cero la votacion
         */
        public void reset_votacion()
        {
            this.Invoke(new EventHandler(delegate
            {
                // Vacio la lista de mandos
                if (lista_mandos != null)
                {
                    this.lista_mandos.Clear();
                }

                // La relleno
                this.lista_mandos = new List<Mando>();
                 ids = createIDsList();

                for (int i = 0; i < CAjustes.num_mandos; i++)
                {
                    this.lista_mandos.Add(new Mando(ids[i]));
                }

                this.votingPanel.actualiza_recuento();

            }));
        }

        public List<int> ids;

        public Mando getMandoByID(int id)
        {
            foreach (Mando mando in lista_mandos)
            {
                if (mando.getID() == id)
                {
                    return mando;
                }
            }
            return null;
        }

        public List<int> createIDsList()
        {
            string[] ranges = CAjustes.rangos.Split(',');

            List<int> data = new List<int>();
            foreach (string entry in ranges)
            {
                string[] range = entry.Split('-');
                int numeroInferior = int.Parse(range[0]);
                int numeroSuperior = int.Parse(range[1]);

                for (int i = numeroInferior; i <= numeroSuperior; i++)
                {
                    data.Add(i);
                }
            }

       

            return data;
        }

        public List<Mando> getMandos()
        {
            return this.lista_mandos;
        }

        /*
         *  Cambia el modo de recuento utilizado (cuenta atras o votados/total)
         */
        private void label_recuento_Click(object sender, EventArgs e)
        {
            if (this.tipo_recuento == 0)
            {
                this.tipo_recuento = 1;

            }
            else
            {
                this.tipo_recuento = 0;
            }
            actualiza_recuento();
        }


        /*
         *  Inicializa gráfico
         */
        private void inicializa_graficos()
        {
            //this.chart1.Series.Clear();
            //this.chart1.Series.Add("Votos");
            //this.chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
            //this.chart1.Series["Votos"].IsValueShownAsLabel = true; // This will display Data Label on the bar.
            //this.chart1.Series["Votos"]["LabelStyle"] = "Bottom";  // This will change Label Position
            //this.chart1.Series["Votos"].LabelForeColor = Color.White;
            //this.chart1.Series["Votos"].Font = new Font("Courier New", 14, FontStyle.Bold);
        }

        /*
         *  Arranca la votacion
         */



        /*
         *  Inicia la cuenta atras y comprueba que llegue a 0
         */
        private void inicia_cuenta_atras()
        {
            //while (Convert.ToInt32(this.label_contador.Text) > 0)
            //{
            //    // Duero 1 s
            //    Thread.Sleep(1000);                    
            //    // Resto el contador
            //    int c = Convert.ToInt32(this.label_contador.Text);                   
            //    c = c-1;

            //    this.Invoke(new EventHandler(delegate
            //    {
            //        this.label_contador.Text = Convert.ToString(c);
            //    }));

            //    if (c == 0)
            //    {
            //        finaliza_votacion();
            //    }
            //}           
        }

        private bool idAllowed(int id)
        {
            string[] ranges = CAjustes.rangos.Split(',');

            List<List<int>> result = new List<List<int>>();
            List<int> data = new List<int>();
            foreach (string entry in ranges)
            {
                string[] range = entry.Split('-');
                int numeroInferior = int.Parse(range[0]);
                int numeroSuperior = int.Parse(range[1]);
                data.Add(numeroInferior);
                data.Add(numeroSuperior);
                result.Add(data);
            }

            foreach (List<int> item in result)
            {
                if (item.Contains(id))
                {
                    return true;
                }
            }

            return false;
        }

        // Recive un voto
        void keyStatus(string id_base, int id_mando, string valor, double tiempo_respuesta)
        {
            if (!idAllowed(id_mando))
            {
                Console.WriteLine("Id del mando no permitido");
                return;
            }



            // Solo ahce caso a los votos si no es el modo aleatorio
            if (CAjustes.comBaseChanel == 0)
            {
                // Paso de letras a números en el caso de que esté en modo números
                if (CAjustes.tipo_votacion == 0)
                {
                    switch (valor)
                    {
                        case "A":
                            valor = "1";
                            break;
                        case "B":
                            valor = "2";
                            break;
                        case "C":
                            valor = "3";
                            break;
                        case "D":
                            valor = "4";
                            break;
                        case "E":
                            valor = "5";
                            break;
                        case "F":
                            valor = "6";
                            break;
                        case "G":
                            valor = "7";
                            break;
                        case "H":
                            valor = "8";
                            break;
                        case "I":
                            valor = "9";
                            break;
                        case "J":
                            valor = "10";
                            break;
                    }
                }

                // Se permite cambiar la respuesta
                if (CAjustes.permitir_cambio_respuesta)
                {
                    this.lista_mandos[get_mando_id_index(id_mando)].respondido = true;
                    this.lista_mandos[get_mando_id_index(id_mando)].respuesta = valor;
                }
                else // No se puede cambiar la respuesta
                {
                    if (!this.lista_mandos[get_mando_id_index(id_mando)].respondido)
                    {
                        this.lista_mandos[get_mando_id_index(id_mando)].respondido = true;
                        this.lista_mandos[get_mando_id_index(id_mando)].respuesta = valor;
                    }
                }


                // Actualizo la informacion
                this.ventana_resultados.actualizar_grid(this.lista_mandos);
                actualiza_recuento();
                actualiza_grafico();
                guarda_resultados_csv();

            }
        }



        /*
         *  Obtiene el indice que ocupa el mando dado por su id
         */
        private int get_mando_id_index(int id)
        {
            //return id - CAjustes.mando_inferior;
            return 0;
        }



        private void modoAleatorio()
        {
            // Número aleatorio de mandos que no va a responder: fallo del 2%
            Random rnd_sin_responder = new Random();
            int mandos_sin_responder = rnd_sin_responder.Next(0, Convert.ToInt32(Math.Floor(CAjustes.num_mandos - CAjustes.num_mandos * 0.98)));

            // Genera mandos aleatoriamente
            Random rnd_id_mando = new Random();


            // Genera retardos de 30 ,s
            int retardo = Convert.ToInt32(25);
            while (this.estado_votacion == 1 && recuenta_votados() != CAjustes.num_mandos - mandos_sin_responder)
            {
                int id_mando = rnd_id_mando.Next(1, CAjustes.num_mandos + 1);
                if (!this.lista_mandos[id_mando - 1].respondido)
                {
                    this.lista_mandos[id_mando - 1].respondido = true;

                    int res = calcula_respuesta_aleatoria(Convert.ToString(CAjustes.comBaseChanel));
                    if (CAjustes.tipo_votacion == 0)
                    {
                        this.lista_mandos[id_mando - 1].respuesta = array_numeros[res];
                    }

                    if (CAjustes.tipo_votacion == 1)
                    {
                        this.lista_mandos[id_mando - 1].respuesta = array_letras[res];
                    }

                    if (CAjustes.tipo_votacion == 2)
                    {
                        this.lista_mandos[id_mando - 1].respuesta = array_verdaderofalso[res];
                    }


                }
                Thread.Sleep(retardo);
            }
        }

        private int calcula_respuesta_aleatoria(string clasif)
        {
            char resultado;
            Random r = new Random();
            int decisor = r.Next(0, 100);
            int _1, _2, _3 = 0;

            switch (clasif.Length)
            {
                case 2:
                    _1 = r.Next(55, 70);
                    _2 = 100 - _1;

                    if (Enumerable.Range(0, _2).Contains(decisor))
                    {
                        resultado = clasif[1];
                    }
                    else
                    {
                        resultado = clasif[0];
                    }

                    break;
                case 3:
                    _1 = r.Next(45, 55);
                    _2 = _1 - r.Next(15, 20);
                    _3 = 100 - _1 - _2;

                    if (Enumerable.Range(0, _3).Contains(decisor))
                    {
                        resultado = clasif[2];
                    }
                    else if (Enumerable.Range(_3, _2).Contains(decisor))
                    {
                        resultado = clasif[1];
                    }
                    else
                    {
                        resultado = clasif[0];
                    }

                    break;
                default:
                    return 0;
            }
            return (int)resultado - 49;
        }




        /*
         *  Actualiza el recuento de votos
         */
        private void actualiza_recuento()
        {
            try
            {
                if (this.tipo_recuento == 0)
                {
                    double porcentaje = Convert.ToDouble(Decimal.Divide(recuenta_votados(), CAjustes.num_mandos)) * 100;
                    //this.label_recuento.Text = Math.Round(porcentaje, 2) + "%";

                }
                else
                {
                    //this.label_recuento.Text = Convert.ToString(recuenta_votados()) + "/" + Convert.ToString(CAjustes.num_mandos);
                }
            }
            catch (Exception ex) { }

        }


        /*
         *  Actualiza el gráfico según llegan los votos
         */
        private void actualiza_grafico()
        {
            inicializa_graficos();


            int[] contador = new int[CAjustes.numero_opciones];
            if (CAjustes.tipo_votacion == 0)
            {
                // Para todos los valores de la votacion
                for (int i = 0; i < CAjustes.numero_opciones; i++)
                {

                    // recuenta
                    for (int j = 0; j < CAjustes.num_mandos; j++)
                    {
                        if (lista_mandos[j].respuesta == this.array_numeros[i])
                        {
                            contador[i]++;
                        }
                    }

                    //chart1.Series["Votos"].Points.AddXY(array_numeros[i], contador[i]);
                    //chart1.Refresh();
                }
            }


            if (CAjustes.tipo_votacion == 1)
            {
                // Para todos los valores de la votacion
                for (int i = 0; i < CAjustes.numero_opciones; i++)
                {
                    for (int j = 0; j < CAjustes.num_mandos; j++)
                    {
                        if (lista_mandos[j].respuesta == array_letras[i])
                        {
                            contador[i]++;
                        }
                    }
                    //chart1.Series["Votos"].Points.AddXY(array_letras[i], contador[i]);
                    //chart1.Refresh();
                }
            }


            comprueba_si_hay_ganador(contador);


            //if (CAjustes.tipo_votacion == 2)
            //{
            //    // Para todos los valores de la votacion
            //    int contador_v = 0;
            //    int contador_f = 0;
            //    for (int j = 0; j < CAjustes.num_mandos; j++)
            //    {
            //        if (lista_mandos[j].respuesta == "True")
            //        {
            //            contador_v++;
            //        }


            //        if (lista_mandos[j].respuesta == "False")
            //        {
            //            contador_f++;
            //        }
            //    }
            //    chart1.Series["Votos"].Points.AddXY("Verdadero", contador_v);
            //    chart1.Series["Votos"].Points.AddXY("Falso", contador_f);
            //    chart1.Refresh();
            //}   

        }

        /*
         *  Comprueba si hay ganador en ese instante
         */
        private void comprueba_si_hay_ganador(int[] contador)
        {
            int votos_restantes = CAjustes.num_mandos - recuenta_votados();
            int indice_ganador = -1;
            int votos_ganador = 0;



            // Busco el mas votado
            for (int i = 0; i < CAjustes.numero_opciones; i++)
            {
                if (votos_ganador <= contador[i])
                {
                    indice_ganador = i;
                    votos_ganador = contador[i];
                }
            }


            bool hay_ganador = true;

            // Miro si es ganador
            for (int i = 0; i < CAjustes.numero_opciones; i++)
            {
                if (i != indice_ganador)
                {
                    if (contador[i] + votos_restantes >= votos_ganador)
                        hay_ganador = false && hay_ganador;
                    else
                        hay_ganador = true && hay_ganador;

                }
            }

            //Si hay ganador coloreo
            if (hay_ganador)
            {
                for (int i = 0; i < CAjustes.numero_opciones; i++)
                {
                    //if(i == indice_ganador)
                    //this.chart1.Series["Votos"].Points[i].Color = Color.LightGreen;
                    //else
                    //this.chart1.Series["Votos"].Points[i].Color = Color.Tomato;
                }
            }
        }



        /*
         *  LLama a finaliza_votacion()
         */
        private void panel_stop_Click(object sender, EventArgs e)
        {
            if (this.estado_votacion == 1)
            {
                finaliza_votacion();
            }
        }


        /*
         *  Ababa la votacion
         */
        private void finaliza_votacion()
        {
            this.estado_votacion = 0;
            //this.panel_indicador_estado.BackgroundImage = Votaciones_App.Properties.Resources.rojo;
            switch (CAjustes.tipo_votacion)
            {
                case 0: // Numeros
                    if (this._choice != null)
                    {
                        Console.WriteLine("Numeros fin: " + this._choice.Stop());
                    }

                    break;


                case 1: // Letras
                    if (this._choice != null)
                    {
                        Console.WriteLine("Letras fin: " + this._choice.Stop());
                    }

                    break;



                case 2: // V/FR
                    if (this._truefalse != null)
                    {
                        Console.WriteLine("True/False fin: " + this._truefalse.Stop());
                    }


                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            if (CAjustes.conexion_grafismo)
            {
                envia_mensaje_progamaExterno("Stop");
            }

            guarda_resultados_csv();


            // Aborto la cuenta atrás
            if (this.cuentaAtras != null && this.cuentaAtras.IsAlive)
            {
                this.cuentaAtras.Abort();
            }

            // Aborto el modo aleatorio
            if (this.aleatorio != null && this.aleatorio.IsAlive)
            {
                this.aleatorio.Abort();
            }

            // Aborto el modo aleatorio
            if (this.reconexion != null && this.reconexion.IsAlive)
            {
                this.reconexion.Abort();
            }


        }


        /*
         *  Guarda el resultado en un csv
         */
        private void guarda_resultados_csv()
        {
            //int tiempo = CAjustes.tiempo_crono -  Convert.ToInt32(label_contador.Text);
            //string resultados = "Tiempo de votacion: "+ tiempo +  " s\n";
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                //resultados = resultados + (i+CAjustes.mando_inferior) + ";" +this.lista_mandos[i].respuesta + "\n";
            }
            //this._ficherocsv.EscribeFichero(CAjustes.ruta_resultados+"Resultados.csv",false, resultados);
        }


        /*
         *  Calcula cuantos mandos han votado hasta el momento
         */
        private int recuenta_votados()
        {
            int contador = 0;
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                if (this.lista_mandos[i].respondido)
                {
                    contador++;
                }
            }
            return contador;
        }

        #region Ajustes

        /*
         *  Lee el fichero y carga los ajustes
         */
        private void carga_ajustes()
        {
            if (!File.Exists(CAjustes.ruta_ajustes))
            {
                // Crear XML
                ArrayList lista = new ArrayList();
                lista.Add("MandosTotales");
                lista.Add("Rangos");
                lista.Add("TiempoCrono");
                lista.Add("BaseAntena");
                lista.Add("PermitirCambioRespuesta");
                lista.Add("PermitirMultichoice");
                lista.Add("TipoVotacion");
                lista.Add("NumeroOpciones");
                lista.Add("RutaResultados");
                lista.Add("ConexionGrafismo");
                lista.Add("Ip");
                lista.Add("IpAntena");
                lista.Add("MacAntena");
                lista.Add("MaskAntena");
                lista.Add("GatewayAntena");
                xmlFile.CreaFicheroVacio("Ajustes", lista, CAjustes.ruta_ajustes);

                // Escribe el XML con valores por defecto
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MandosTotales", "100");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Rangos", "1-100");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TiempoCrono", "120");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", CAjustes.base_antena == null ? "1" : CAjustes.base_antena.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta", "false");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirMultichoice", CAjustes.permitir_multichoice.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TipoVotacion", "0");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "NumeroOpciones", "3");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "RutaResultados", "./");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "ConexionGrafismo", "false");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Ip", "127.0.0.1");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "IpAntena", CAjustes.ip_antena ?? "192.168.0.199");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MacAntena", CAjustes.mac_antena ?? "74-30-13-02-05-36");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MaskAntena", CAjustes.mask_antena ?? "255.255.255.0");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "GatewayAntena", CAjustes.gateway_antena ?? "192.168.0.1");
            }
            if (validaAjustesFicheroXml())
            {
                // Cargar los ajustes del fichero en memoria
                CAjustes.num_mandos = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales"));
                CAjustes.rangos = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos");
                CAjustes.tiempo_crono = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono"));
                CAjustes.base_antena = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "BaseAntena"));
                CAjustes.permitir_cambio_respuesta = bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta"));
                CAjustes.tipo_votacion = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion"));
                CAjustes.numero_opciones = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones"));
                CAjustes.ruta_resultados = xmlFile.LeerXml(CAjustes.ruta_ajustes, "RutaResultados");
                CAjustes.conexion_grafismo = bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "ConexionGrafismo"));
                CAjustes.ip = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Ip");
                CAjustes.ip_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "IpAntena");
                CAjustes.mac_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MacAntena");
                CAjustes.mask_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MaskAntena");
                CAjustes.gateway_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "GatewayAntena");
            }
            else
            {
                // Cargar ajustes por defecto en memoria en caso de no pasar la validación
                CAjustes.num_mandos = 100;
                CAjustes.rangos = "1-100";
                CAjustes.tiempo_crono = 120;
                CAjustes.base_antena = 1;
                CAjustes.permitir_cambio_respuesta = false;
                CAjustes.permitir_multichoice = false;
                CAjustes.tipo_votacion = 0;
                CAjustes.numero_opciones = 3;
                CAjustes.ruta_resultados = "./";
                CAjustes.conexion_grafismo = false;
                CAjustes.ip = "127.0.0.1";
                CAjustes.ip_antena = "192.168.0.199";
                CAjustes.mac_antena = "74-30-13-02-05-36";
                CAjustes.mask_antena = "255.255.255.0";
                CAjustes.gateway_antena = "192.168.0.1";
            }
        }

        private bool validaAjustesFicheroXml()
        {
            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales")) != FormMandosConfig.counterFromString(xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos")))
            {
                MessageBox.Show("El número de mandos totales debe ser igual que el número de mandos en los rangos. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono")) < 1 || int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono")) > 1000)
            {
                MessageBox.Show("El tiempo del crono debe estar comprendido entre 1 y 1000. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion")) < 0 || int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion")) > 2)
            {
                MessageBox.Show("Tipo de votación incorrecta. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones")) < 1 || int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones")) > 10)
            {
                MessageBox.Show("EL número de opciones debe estar comprendido entre 1 y 10. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "Ip"), out _))
            {
                MessageBox.Show("EL número de IP proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (!Directory.Exists(xmlFile.LeerXml(CAjustes.ruta_ajustes, "RutaResultados")))
            {
                MessageBox.Show("El directorio seleccionado no existe. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "IpAntena"), out _))
            {
                MessageBox.Show("EL número de IP proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            Match match = Regex.Match(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MacAntena"), "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})|([0-9a-fA-F]{4}\\.[0-9a-fA-F]{4}\\.[0-9a-fA-F]{4})$");
            if (!match.Success)
            {
                MessageBox.Show("La dirección MAC proporcionada no es válida. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MaskAntena"), out _))
            {
                MessageBox.Show("EL número de IP de la máscara de subred proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "GatewayAntena"), out _))
            {
                MessageBox.Show("EL número de IP de la puerta de enlace proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            return true;
        }

        /*
         *   Handler boton aceptar ajustes
         */


        /*
         *  Valida que los ajustes sean correctos
         */
        private bool valida_ajustes()
        {
            //if (this.numericUpDown_ajustes_mandos_inferior.Value < 1 )
            //{
            //    MessageBox.Show("El número de mandos no debe ser menor de 1", "Error");
            //    return false;
            //}

            //if (this.numericUpDown_ajustes_mandos_inferior.Value >= this.numericUpDown_ajustes_mandos_superior.Value)
            //{
            //    MessageBox.Show("El rango de valores de los mandos no es válido", "Error");
            //    return false;
            //}

            //if (this.numericUpDown_ajustes_tiempo_crono.Value < 1 || this.numericUpDown_ajustes_tiempo_crono.Value > 1000)
            //{
            //    MessageBox.Show("El tiempo del crono debe estar comprendido entre 1 y 1000", "Error");
            //    return false;
            //}

            //if (!this.radioButton_ajustes_cambiar_resp_Si.Checked && !this.radioButton_ajustes_cambiar_resp_No.Checked)
            //{
            //    MessageBox.Show("Por favor, elija si se permite cambiar de respuesta o no", "Error");
            //    return false;
            //}

            //if (this.comboBox_ajustes_tipo_votacion.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Por favor, elija un tipo de votación", "Error");
            //    return false;
            //}


            //if (this.numericUpDown_ajustes_num_opciones.Value < 1 || this.numericUpDown_ajustes_num_opciones.Value > 10)
            //{
            //    MessageBox.Show("EL número de opciones debe estar comprendido entre 1 y 10", "Error");
            //    return false;
            //}


            //if (!Directory.Exists(this.textBox_ajustes_resultados_path.Text))
            //{
            //    MessageBox.Show("El directorio seleccionado no existe", "Error");
            //    return false;
            //}
            return true;
        }


        /*
         *  Apaga todos los mandos
         */
        private void button_apagar_mandos_Click(object sender, EventArgs e)
        {
            this.keypadManage.RemoteOff(0);
        }


        /*
         *  Cambia del panel de ajustes al principal y viceversa
         */
        public void cambia_panel(string nombre_panel)
        {
            if (nombre_panel == "ajustes")
            {
                this.WindowState = FormWindowState.Normal;
                //this.Controls.Remove(this.panel_principal);
                //this.Controls.Add(this.panel_ajustes);
                this.MinimumSize = new Size(800, 375);
                this.Size = new Size(800, 375);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
            }

            if (nombre_panel == "principal")
            {
                //this.Controls.Remove(this.panel_ajustes);
                //this.Controls.Add(this.panel_principal);
                this.Size = new Size(569, 377);
                this.MinimumSize = new Size(569, 377);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.MaximizeBox = true;
            }
        }


        /*
         *  Lanza el file folder para seleccionar la carpeta donde se guardara el csv de ajustes
         */
        private void button_ajustes_resultados_path_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                //this.textBox_ajustes_resultados_path.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }


        /*
         *  Para mostrar u ocultar el panel de número de opciones
         */
        private void comboBox_ajustes_tipo_votacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.comboBox_ajustes_tipo_votacion.SelectedIndex == 0 || this.comboBox_ajustes_tipo_votacion.SelectedIndex == 1)
            //{
            //    this.panel_ajustes_num_opciones.Visible = true;
            //}
            //else
            //{
            //    this.panel_ajustes_num_opciones.Visible = false;
            //}
        }




        #endregion

        /*
         *  Abre la lista con la informacion de las votaciones
         */
        private void panel_abre_lista_Click(object sender, EventArgs e)
        {
            if (this.ventana_resultados.IsDisposed)
            {
                this.ventana_resultados = new FormResultados(new Point(this.Left + this.Width, this.Top));
                this.ventana_resultados.inicializa_grid();
                this.ventana_resultados.actualizar_grid(this.lista_mandos);
            }


            this.ventana_resultados.Show();
            this.ventana_resultados.BringToFront();
        }






        /*
         *  Establece las conexiones con el programa esterno
         */
        public bool conecta_cliente()
        {

            try
            {
                this.endpoint = new IPEndPoint(IPAddress.Parse(CAjustes.ip), CAjustes.puerto);
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(endpoint);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0}", e.Message);
                return false;
            }
        }



        /*
         *  Intenta reconectar hasta que lo consigue. Lo reintenta cada 100ms
         */
        private void reintenta_reconectar_cliente()
        {
            while (this.estado_conexion_grafismo == 0)
            {
                if (conecta_cliente())
                {
                    this.estado_conexion_grafismo = 1;
                }
                else
                {
                    this.estado_conexion_grafismo = 0;
                }
                Console.WriteLine("Intentando conectarse con el programa externo");
                Thread.Sleep(1000);
            }

            // Cuando reconecta finalizo
            Console.WriteLine("Conexión realizada con exito");
            termina_reconexion_cliente();
        }


        /*
         *  Inicia el thread para la reconexion
         */
        public void inicia_reconexion_cliente()
        {
            this.reconexion = new Thread(reintenta_reconectar_cliente);
            if (this.reconexion != null)
            {
                if (!this.reconexion.IsAlive)
                {

                    this.reconexion.Start();
                }
            }
        }


        /*
         *  Termina el thread para la reconexion
         */
        public void termina_reconexion_cliente()
        {
            if (this.reconexion != null)
            {
                if (this.reconexion.IsAlive)
                {

                    this.reconexion.Abort();
                }
            }
        }


        public void desconecta_cliente()
        {
            try
            {
                if (this.socket != null)
                {
                    this.socket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al desconectar con la Tiva: " + e.Message);
            }
        }


        /*
         * Envia un mensaje a la aplicación externa
         */
        public void envia_mensaje_progamaExterno(string instruccion)
        {

            if (this.estado_conexion_grafismo == 1)
            {

                instruccion = "itemset(\"" + instruccion + "\", \"MAP_EXE\");";

                try
                {
                    byte[] strASCII = System.Text.Encoding.Default.GetBytes(instruccion);
                    this.socket.Send(strASCII);
                }
                catch (Exception e)
                {
                    Console.WriteLine("SocketException: {0}", e.Message);
                    this.estado_conexion_grafismo = 0;
                    this.reconexion = new Thread(reintenta_reconectar_cliente);
                    inicia_reconexion_cliente();
                }
            }
        }




        #region license

        private void ReloadLicense()
        {
            string idMachineOnFile = "", product = "", expirationDate = "", license = "", ifLicense = "", lastRun = "";
            string licenseFile = ".\\serial.lic";
            string[] words = new string[4];
            DateTime dt1 = DateTime.Now;
            DateTime dt2;

            if (!File.Exists(licenseFile))
                ifLicense = "4";
            else
            {
                // Get el id de la máquina desde el fichero
                try
                {
                    using (StreamReader sr = new StreamReader(licenseFile))
                    {
                        license = sr.ReadToEnd();
                        sr.Close();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("The file could not be read: ");
                    Console.WriteLine(exception.Message);
                }
                char[] delimiterChars = { ';' };
                words = license.Split(delimiterChars);
                idMachineOnFile = CSeguridad.DesEncriptar(words[0]);
                product = CSeguridad.DesEncriptar(words[1]);
                expirationDate = CSeguridad.DesEncriptar(words[2]);
                lastRun = CSeguridad.DesEncriptar(words[3]);

                //Console.WriteLine(idMachineOnFile);
                //Console.WriteLine(product);
                //Console.WriteLine(expirationDate);

                ifLicense = CheckLicencia(idMachineOnFile, product, expirationDate);

                // Comprobamos si han cambiado la fecha del reloj del sistema
                dt1 = DateTime.Now;
                dt2 = Convert.ToDateTime(lastRun);
                int result = DateTime.Compare(dt1, dt2);
                if (result <= 0)
                    ifLicense = "5";

                // Comprobamos si queda menos de una semana para que expire la licencia
                dt2 = Convert.ToDateTime(expirationDate);
                TimeSpan ts = dt2 - dt1;
                int differenceInDays = ts.Days;
                if ((ifLicense != "1") && (ifLicense != "2") && (ifLicense != "3") && (ifLicense != "4") && (ifLicense != "5") && (differenceInDays <= 7))
                    MessageBox.Show("La licencia expirará en " + Convert.ToString(differenceInDays) + " días.\nPor favor, póngase en contacto con su proveedor.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            switch (ifLicense)
            {
                case "1":
                    //ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 01");
                    //this.unregistered = true;
                    //this.panel_license.BackColor = Color.LightPink;
                    //this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 01";
                    //this.panel_license.Visible = true;
                    //this.label_license.Visible = true;
                    break;
                case "2":
                    //ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 02");
                    //this.unregistered = true;
                    //this.panel_license.BackColor = Color.LightPink;
                    //this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 02";
                    //this.panel_license.Visible = true;
                    //this.label_license.Visible = true;
                    break;
                case "3":
                    //ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 03");
                    //this.unregistered = true;
                    //this.panel_license.BackColor = Color.LightPink;
                    //this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 03";
                    //this.panel_license.Visible = true;
                    //this.label_license.Visible = true;
                    break;
                case "4":
                    //ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 04");
                    //this.unregistered = true;
                    //this.panel_license.BackColor = Color.LightPink;
                    //this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 04";
                    //this.panel_license.Visible = true;
                    //this.label_license.Visible = true;

                    break;
                case "5":
                    //ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 05");
                    //this.unregistered = true;
                    //this.panel_license.BackColor = Color.LightPink;
                    //this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 05";
                    //this.panel_license.Visible = true;
                    //this.label_license.Visible = true;
                    break;
                default:
                    //ToolTip1.SetToolTip(this.panel_info, "Licencia válida hasta el "+ ifLicense);
                    //this.unregistered = false;
                    //this.panel_license.BackColor = Color.Transparent;
                    //this.label_license.ForeColor = Color.Black;
                    //this.label_license.Text = "Licencia válida hasta el " + ifLicense;
                    //this.panel_license.Visible = false;
                    //this.label_license.Visible = false;  
                    File.WriteAllText(licenseFile, words[0] + ";" + words[1] + ";" + words[2] + ";" + CSeguridad.Encriptar(Convert.ToString(dt1)));
                    break;
            }
        }

        // Return = "0": No existe el fichero host.txt
        // Return = "1": No existe licencia válida
        // Return = "2": Licencia correcta
        public string CheckLicencia(string idMachine, string product, string dateExpiration)
        {
            string idMachineDecryp = "";
            int posSpace = -1;

            // Get el id de la máquina con el comando de msdos
            idMachineDecryp = GetMachineID();

            DateTime dt1 = DateTime.Now;
            DateTime dt2 = Convert.ToDateTime(dateExpiration);
            int result = DateTime.Compare(dt1, dt2);

            posSpace = dateExpiration.IndexOf(' ');
            if (idMachineDecryp.Trim() != idMachine.Trim())
                return "1";
            else
            {
                if (product != "VOTACIONES APP")
                    return "2";
                else
                {
                    if (result > 0)
                        return "3";
                    else
                        return dateExpiration.Substring(0, posSpace).Trim();
                }
            }
        }


        public string GetMachineID()
        {
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("CMD.EXE", "/C wmic csproduct get uuid");
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd();
            int pos = result.IndexOf("\n");
            string aux = result.Substring(pos + 1, result.Length - pos - 1);
            //System.IO.File.WriteAllText(@".\serial.txt", aux);

            proc.WaitForExit();
            proc.Close();

            return aux;
        }

        #endregion

        private void radioButton_ajustes_conexion_grafismo_Si_CheckedChanged(object sender, EventArgs e)
        {
            //if(this.radioButton_ajustes_conexion_grafismo_Si.Checked){
            //    this.textBox_ip.Visible = true;
            //    this.textBox_puerto.Visible = true;
            //    this.label_ajustes_ip.Visible = true;
            //    this.label_ajustes_puerto.Visible = true;
            //}
        }

        private void radioButton_ajustes_conexion_grafismo_No_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.radioButton_ajustes_conexion_grafismo_No.Checked)
            //{
            //    this.textBox_ip.Visible = false;
            //    this.textBox_puerto.Visible = false;
            //    this.label_ajustes_ip.Visible = false;
            //    this.label_ajustes_puerto.Visible = false;
            //}
        }

        // Loads the view that allows to select the connection type to the Base Antenna
        private void loadInitalPanel()
        {
            UserControlConnectionChoice choicePanel = new UserControlConnectionChoice(setConnectionMode);
            this.panel_root.Controls.Add(choicePanel);

        }

        private void setConnectionMode(int mode)
        {
            this.connectionMode = mode;
            loadMultichoiseSelecctionDialog();
            removeChoicePanel();
            carga_ajustes();
            if (this.connectionMode == 1)
            {
                this.baseConn.Open(this.connectionMode, Convert.ToString(CAjustes.base_antena));
            }
            else if (this.connectionMode == 2)
            {
                this.baseConn.BaseIP = CAjustes.ip_antena;
                this.baseConn.Open(this.connectionMode, Convert.ToString(CAjustes.base_antena));
            }
            addSettingsPanel("");
        }

        private void removeChoicePanel()
        {
            UserControlConnectionChoice choicePanel = ((UserControlConnectionChoice)this.panel_root.Controls.OfType<Control>().First());
            this.panel_root.Controls.Remove(choicePanel);
        }

        private void addSettingsPanel(string msg)
        {
            setFixedWindowFormState();
            if (this.votingPanel != null)
            {
                this.panel_root.Controls.Remove(this.votingPanel);
            }
            // Si el panel setting es nulo crear una nuevo, sino no
            if (this.settingsPanel == null)
            {
                this.settingsPanel = new UserControlSettings(loadVotingPanel);
            }
            this.panel_root.Controls.Add(this.settingsPanel);
        }

        private void setFixedWindowFormState()
        {
            this.WindowState = FormWindowState.Normal;
            this.MinimumSize = new Size(1148, 467);
            this.Size = new Size(1148, 467);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void setFreeWindowFormState()
        {
            this.WindowState = FormWindowState.Normal;
            this.MinimumSize = new Size(1148, 467);
            this.Size = new Size(1148, 467);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
        }

        private void loadVotingPanel(string msg)
        {
            saveDataToXmlFie();
            this.panel_root.Controls.Remove(this.settingsPanel);
            setFreeWindowFormState();
            // Si el panel de votación es nulo crear uno nuevo, sino no
            if (this.votingPanel == null)
            {
                this.votingPanel = new UserControlVoting(addSettingsPanel);
            }

            this.panel_root.Controls.Add(this.votingPanel);
            reset_votacion();

        }



        private void loadMultichoiseSelecctionDialog()
        {
            string message = "¿Desea activar la opción Multirespuesta?";
            string title = "Multichoice selector";
            DialogResult dialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.enableMultichoice = true;
            }
            else
            {
                this.enableMultichoice = false;
            }
            CAjustes.permitir_multichoice = this.enableMultichoice;
        }

        private void openConnection()
        {
            if (this.connectionMode == 1)
            {
                this.baseConn.Open(this.connectionMode, Convert.ToString(CAjustes.base_antena));
            }
            else if (this.connectionMode == 2)
            {
                this.baseConn.BaseIP = CAjustes.ip_antena;
                this.baseConn.Open(this.connectionMode, Convert.ToString(CAjustes.base_antena));
            }
        }

        private void saveDataToXmlFie()
        {
            // Escribe el XML
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MandosTotales", CAjustes.num_mandos.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Rangos", CAjustes.rangos);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TiempoCrono", CAjustes.tiempo_crono.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", CAjustes.base_antena.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta", CAjustes.permitir_cambio_respuesta.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirMultichoice", CAjustes.permitir_multichoice.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TipoVotacion", CAjustes.tipo_votacion.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "NumeroOpciones", CAjustes.numero_opciones.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "RutaResultados", CAjustes.ruta_resultados);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "ConexionGrafismo", CAjustes.conexion_grafismo.ToString());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Ip", CAjustes.ip);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "IpAntena", CAjustes.ip_antena);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MacAntena", CAjustes.mac_antena);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MaskAntena", CAjustes.mask_antena);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "GatewayAntena", CAjustes.gateway_antena);
        }
    }
}
