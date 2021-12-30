using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Votaciones_App.Negocio;

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

        public static string[] array_nombres;

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
            this.ventanaResultados.setLocation(new Point(((FormPrincipal)(this.Parent.Parent)).Left + ((FormPrincipal)(this.Parent.Parent)).Width, ((FormPrincipal)(this.Parent.Parent)).Top));
            this.ventanaResultados.Show();
            this.ventanaResultados.BringToFront();
        }

        // ##############   Public methods   ############## \\
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

        public void setEnableButtonApagarMandos(bool value)
        {
            this.button_apagar_mandos.Enabled = value;
        }

        public FormResultados getVentanaResultados()
        {
            return this.ventanaResultados;
        }

        public void inicializarGrafica(List<Option> options)
        {
            this.chart.Series.Clear();
            this.chart.ChartAreas.Clear();

            this.chart.Series.Add("Votos");
            this.chart.ChartAreas.Add("ChartArea");
            this.chart.Series["Votos"].XValueMember = "id"; // Bind with Option object name "id"
            this.chart.Series["Votos"].YValueMembers = "totalVotes"; // Bind with Option object name "totalVotes"
            this.chart.Series["Votos"].IsValueShownAsLabel = true; // This will display Data Label on the bar
            this.chart.Series["Votos"]["LabelStyle"] = "Top"; // This will change Label Position
            this.chart.Series["Votos"].Font = new Font("Courier New", 14, FontStyle.Bold);
            this.chart.Series["Votos"].LabelForeColor = Color.Purple;
            this.chart.ChartAreas["ChartArea"].AxisX.LabelStyle.Font = new Font("Courier New", 14, FontStyle.Bold);
            this.chart.ChartAreas["ChartArea"].BackColor = Color.FromArgb(249, 237, 255);
            this.chart.ChartAreas["ChartArea"].AxisX.Interval = 1; // Show the axis X names in 1 interval
            this.chart.ChartAreas["ChartArea"].AxisX.MajorGrid.LineWidth = 0;
            this.chart.ChartAreas["ChartArea"].AxisY.MajorGrid.LineWidth = 1;
            this.chart.ChartAreas["ChartArea"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this.chart.ChartAreas["ChartArea"].AxisY.MajorGrid.LineColor = Color.FromArgb(50, Color.Black);
            this.chart.DataSource = options; // Bind with the data source

            Option.chart = this.chart; // Bind with the static Chart object of the Option class
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
