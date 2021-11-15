using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Votaciones_App.Views
{
    // Clase encargada de mostrar los controles para gestionar la votación y visualizar los datos
    public partial class UserControlVoting : UserControl
    {
        public delegate void CommunicatorDelegate(string msg);
        public CommunicatorDelegate communicatorCallBack;

        private int tipoRecuento = 0;
        FormResultados ventanaResultados;

        private int estado_conexion_grafismo;
        private IPEndPoint endpoint;
        private Socket socket;
        private Thread reconexion;

        // ##############   Constructor  ############## \\
        public UserControlVoting()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void UserControlVoting_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill; // Dock style

            this.label_contador.Text = CAjustes.tiempo_crono.ToString();

            this.ventanaResultados = new FormResultados(new Point(((FormPrincipal)(this.Parent.Parent)).Left + ((FormPrincipal)(this.Parent.Parent)).Width, ((FormPrincipal)(this.Parent.Parent)).Top));

            this.estado_conexion_grafismo = 0;
            if (CAjustes.conexion_grafismo)
            {
                // Intento de conexión con el programa externo
                if (conecta_cliente())
                {
                    this.estado_conexion_grafismo = 1;
                }
                else
                {
                    this.estado_conexion_grafismo = 0;
                    inicia_reconexion_cliente();
                }
            }
        }

        private void panel_go_ajustes_Click(object sender, EventArgs e)
        {
            communicatorCallBack("UserControlVoting-cambiaPanel");
        }

        private void button_apagar_mandos_Click(object sender, EventArgs e)
        {
            communicatorCallBack("UserControlVoting-apagaMandos");
        }

        private void panel_play_Click(object sender, EventArgs e)
        {
            communicatorCallBack("UserControlVoting-iniciaVotacion");
        }

        private void panel_stop_Click(object sender, EventArgs e)
        {
            communicatorCallBack("UserControlVoting-detieneVotacion");
        }

        private void label_recuento_Click(object sender, EventArgs e)
        {
            if (this.tipoRecuento == 0)

                this.tipoRecuento = 1;
            else
                this.tipoRecuento = 0;

            communicatorCallBack("UserControlVoting-recuentoClick");
        }

        private void panel_abre_lista_Click(object sender, EventArgs e)
        {
            this.ventanaResultados.Show();
            this.ventanaResultados.BringToFront();
        }
        // ##############   Public events   ############## \\
        public int getTipoRecuento()
        {
            return this.tipoRecuento;
        }

        public void setRecuento(string value)
        {
            this.label_recuento.Text = value;
        }

        public string getCronoTime()
        {
            return this.label_contador.Text;
        }

        public void setCronoTime(string time)
        {
            this.label_contador.Text = time;
        }

        public void setImageVoteStatus(Image image)
        {
            this.panel_indicador_estado.BackgroundImage = image;
        }

        public FormResultados getVentanaResultados()
        {
            return this.ventanaResultados;
        }

        public void inicializarGrafica()
        {
            this.chart.Series.Clear();
            this.chart.Series.Add("Votos");
            this.chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
            this.chart.ChartAreas["ChartArea1"].AxisX.Interval = 1; // Show the axis X names in 1 interval
            this.chart.Series["Votos"].IsValueShownAsLabel = true; // This will display Data Label on the bar.
            this.chart.Series["Votos"]["LabelStyle"] = "Top";  // This will change Label Position
            this.chart.Series["Votos"].LabelForeColor = Color.Purple;
            this.chart.Series["Votos"].Font = new Font("Courier New", 14, FontStyle.Bold);
        }

        private string[] array_letras = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        private string[] array_numeros = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        public static string[] array_nombres;

        public void actualizarGrafica(List<Mando> listaMandos, int votados)
        {
            inicializarGrafica();

            int[] contador = new int[CAjustes.numero_opciones];
            if (CAjustes.tipo_votacion == 0)
            {
                // Para todos los valores de la votacion
                for (int i = 0; i < CAjustes.numero_opciones; i++)
                {
                    // Recuenta
                    for (int j = 0; j < listaMandos.Count; j++)
                    {
                        if (listaMandos[j].respuesta.Split(';').Contains(this.array_numeros[i]))
                        {
                            contador[i]++;
                        }
                    }

                    //chart.Series["Votos"].Points.AddXY(array_numeros[i], contador[i]);
                    chart.Series["Votos"].Points.AddXY(array_nombres == null ? array_numeros[i] : array_nombres[i], contador[i]);
                    chart.Refresh();
                }
            }

            if (CAjustes.tipo_votacion == 1)
            {
                // Para todos los valores de la votacion
                for (int i = 0; i < CAjustes.numero_opciones; i++)
                {
                    // Recuenta
                    for (int j = 0; j < CAjustes.num_mandos; j++)
                    {
                        if (listaMandos[j].respuesta.Split(';').Contains(this.array_letras[i]))
                        {
                            contador[i]++;
                        }
                    }
                    chart.Series["Votos"].Points.AddXY(array_letras[i], contador[i]);
                    chart.Refresh();
                }
            }

            comprueba_si_hay_ganador(contador, votados);
        }

        public void envia_mensaje_progamaExterno(string instruccion)
        {
            if (this.estado_conexion_grafismo == 1)
            {
                instruccion = "itemset(\"" + instruccion + "\", \"MAP_EXE\");";

                try
                {
                    byte[] strASCII = Encoding.Default.GetBytes(instruccion);
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

        // ##############   Auxiliary private functions   ############## \\
        private void comprueba_si_hay_ganador(int[] contador, int votados)
        {
            int votos_restantes = CAjustes.num_mandos - votados;
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
                    if (i == indice_ganador)
                        this.chart.Series["Votos"].Points[i].Color = Color.LightGreen;
                    else
                        this.chart.Series["Votos"].Points[i].Color = Color.Tomato;
                }
            }
        }

        private bool conecta_cliente()
        {
            try
            {
                this.endpoint = new IPEndPoint(IPAddress.Parse(CAjustes.ip), CAjustes.puerto);
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(endpoint);
                Console.WriteLine("Conexión realizada con exito");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0}", e.Message);
                return false;
            }
        }

        private void inicia_reconexion_cliente()
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

            // Cuando hay una conexión exitosa se finaliza el hilo creado para ella
            Console.WriteLine("Reintento de conexión realizado con exito");
            termina_reconexion_cliente();
        }
    }
}
