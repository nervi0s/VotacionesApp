using SunVote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Votaciones_App.Views
{
    public partial class UserControlVoting : UserControl
    {
        public bool voting { get; set; }
        private int tipo_recuento = 0;
        FormResultados ventana_resultados;
        int[] recuenta_opciones;

        public UserControlVoting()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.label_contador.Text = CAjustes.tiempo_crono.ToString();
        }

        private void panel_go_ajustes_Click(object sender, EventArgs e)
        {
            //this.changeToSettingPanel("");
        }

        private void UserControlVoting_Paint(object sender, PaintEventArgs e)
        {
            this.label_contador.Text = CAjustes.tiempo_crono.ToString();
            if (voting)
            {
                this.panel_indicador_estado.BackgroundImage = Properties.Resources.verde;
            }
            else
            {
                this.panel_indicador_estado.BackgroundImage = Properties.Resources.rojo;
            }
        }

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

        public void actualiza_recuento()
        {
            try
            {
                if (this.tipo_recuento == 0)
                {
                    double porcentaje = Convert.ToDouble(Decimal.Divide(recuenta_votados(), CAjustes.num_mandos)) * 100;
                    this.label_recuento.Text = Math.Round(porcentaje, 2) + "%";

                }
                else
                {
                    this.label_recuento.Text = Convert.ToString(recuenta_votados()) + "/" + Convert.ToString(CAjustes.num_mandos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private int recuenta_votados()
        {
            int contador = 0;
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {

                if (((FormPpal)(this.Parent.Parent)).getMandos()[i].respondido)
                {
                    contador++;
                }
            }
            return contador;
        }

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
                    ((FormPpal)(this.Parent.Parent)).getMandoByID(id_mando).respondido = true;
                    ((FormPpal)(this.Parent.Parent)).getMandoByID(id_mando).respuesta = valor;

                }
                else // No se puede cambiar la respuesta
                {
                    if (!((FormPpal)(this.Parent.Parent)).getMandoByID(id_mando).respondido)
                    {
                        ((FormPpal)(this.Parent.Parent)).getMandoByID(id_mando).respondido = true;
                        ((FormPpal)(this.Parent.Parent)).getMandoByID(id_mando).respuesta = valor;
                    }
                }


                // Actualizo la informacion
                this.ventana_resultados.actualizar_grid(((FormPpal)(this.Parent.Parent)).getMandos());
                actualiza_recuento();
                actualiza_grafico();
                guarda_resultados_csv();

            }
        }
        private void guarda_resultados_csv()
        {
            int tiempo = CAjustes.tiempo_crono - Convert.ToInt32(label_contador.Text);
            string resultados = "Tiempo de votacion: " + tiempo + " s\n";
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                resultados = resultados + (i +  ";" + ((FormPpal)(this.Parent.Parent)).getMandos()[i].respuesta + "\n");
            }
            this._ficherocsv.EscribeFichero(CAjustes.ruta_resultados + "Resultados.csv", false, resultados);
        }
        CFichero _ficherocsv = new CFichero();
        string[] array_letras = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        string[] array_numeros = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
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
                        if (((FormPpal)(this.Parent.Parent)).getMandos()[j].respuesta == this.array_numeros[i])
                        {
                            contador[i]++;
                        }
                    }

                    chart1.Series["Votos"].Points.AddXY(array_numeros[i], contador[i]);
                    chart1.Refresh();
                }
            }


            if (CAjustes.tipo_votacion == 1)
            {
                // Para todos los valores de la votacion
                for (int i = 0; i < CAjustes.numero_opciones; i++)
                {
                    for (int j = 0; j < CAjustes.num_mandos; j++)
                    {
                        if (((FormPpal)(this.Parent.Parent)).getMandos()[j].respuesta == array_letras[i])
                        {
                            contador[i]++;
                        }
                    }
                    chart1.Series["Votos"].Points.AddXY(array_letras[i], contador[i]);
                    chart1.Refresh();
                }
            }


            comprueba_si_hay_ganador(contador);




        }

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
                    if (i == indice_ganador)
                        this.chart1.Series["Votos"].Points[i].Color = Color.LightGreen;
                    else
                        this.chart1.Series["Votos"].Points[i].Color = Color.Tomato;
                }
            }
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
                if (id <= item[1] && id >= item[0])
                {
                    return true;
                }
         
            }

            return false;
        }

        Choices _choice;
        TrueFalse _truefalse;
        private void panel_play_Click(object sender, EventArgs e)
        {

            if (!this.voting)
            {
                this.voting = true;

                ((FormPpal)(this.Parent.Parent)).reset_votacion();
                this.ventana_resultados.inicializa_grid();
                this.panel_indicador_estado.BackgroundImage = Votaciones_App.Properties.Resources.verde;
                if (CAjustes.tipo_votacion == 2)
                {
                    this.recuenta_opciones = new int[2];
                }
                else
                {
                    this.recuenta_opciones = new int[CAjustes.numero_opciones];
                }

                inicializa_graficos();

                if (CAjustes.conexion_grafismo)
                {
                    envia_mensaje_progamaExterno("Play");
                }



                // Arranca el programa
                switch (CAjustes.tipo_votacion)
                {
                    case 0: // Numeros
                        if (this._choice == null)
                        {
                            this._choice = new Choices();
                            _choice.KeyStatus += new IChoicesEvents_KeyStatusEventHandler(keyStatus);
                        }


                        this._choice.BaseConnection = ((FormPpal)(this.Parent.Parent)).getBase();

                        this._choice.OptionsMode = 1;

                        if (CAjustes.permitir_cambio_respuesta)
                        {
                            this._choice.ModifyMode = 1;
                        }
                        else
                        {
                            this._choice.ModifyMode = 0;
                        }
                        this._choice.SecrecyMode = 0;
                        this._choice.Options = CAjustes.numero_opciones;
                        this._choice.OptionalN = 1;
                        this._choice.StartMode = 1;

                        if (this._choice.Start() == "0")
                        {
                            Console.WriteLine("Votación números iniciada correctamente");
                        }
                        break;


                    case 1: // Letras
                        if (this._choice == null)
                        {
                            this._choice = new Choices();
                            _choice.KeyStatus += new IChoicesEvents_KeyStatusEventHandler(keyStatus);
                        }

                        this._choice.BaseConnection = ((FormPpal)(this.Parent.Parent)).getBase();

                        this._choice.OptionsMode = 0;

                        if (CAjustes.permitir_cambio_respuesta)
                        {
                            this._choice.ModifyMode = 1;
                        }
                        else
                        {
                            this._choice.ModifyMode = 0;
                        }
                        this._choice.SecrecyMode = 0;
                        this._choice.Options = CAjustes.numero_opciones;
                        this._choice.OptionalN = 1;
                        this._choice.StartMode = 1;


                        if (this._choice.Start() == "0")
                        {
                            Console.WriteLine("Votación letras iniciada correctamente");
                        }
                        break;



                    case 2: // V/F
                        if (this._truefalse == null)
                        {
                            this._truefalse = new TrueFalse();
                            this._truefalse.KeyStatus += new ITrueFalseEvents_KeyStatusEventHandler(keyStatus);
                        }
                        this._truefalse.BaseConnection = ((FormPpal)(this.Parent.Parent)).getBase();
                        this._truefalse.Mode = 1;
                        if (CAjustes.permitir_cambio_respuesta)
                        {
                            this._truefalse.ModifyMode = 1;
                        }
                        else
                        {
                            this._truefalse.ModifyMode = 0;
                        }
                        this._truefalse.SecrecyMode = 0;

                        if (this._truefalse.Start() == "0")
                        {
                            Console.WriteLine("Votación Ferdadero/falso iniciada correctamente");
                        }
                        break;

                    default:
                        Console.WriteLine("Default case");
                        break;
                }

                if (CAjustes.comBaseChanel != 0)
                {
                    //this.aleatorio = new Thread(modoAleatorio);
                    //this.aleatorio.Start();
                }

                // Arranco la cuenta atrás
                this.cuentaAtras = new Thread(inicia_cuenta_atras);
                cuentaAtras.Start();
            }
        }

        private void inicia_cuenta_atras()
        {
            while (Convert.ToInt32(this.label_contador.Text) > 0)
            {
                // Duero 1 s
                Thread.Sleep(1000);
                // Resto el contador
                int c = Convert.ToInt32(this.label_contador.Text);
                c = c - 1;

                this.Invoke(new EventHandler(delegate
                {
                    this.label_contador.Text = Convert.ToString(c);
                }));

                if (c == 0)
                {
                    finaliza_votacion();
                }
            }
        }

        private void finaliza_votacion()
        {
            this.voting = false;
            this.panel_indicador_estado.BackgroundImage = Votaciones_App.Properties.Resources.rojo;
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
        Thread cuentaAtras;
        Thread aleatorio;
        private void inicializa_graficos()
        {
            this.chart1.Series.Clear();
            this.chart1.Series.Add("Votos");
            this.chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
            this.chart1.Series["Votos"].IsValueShownAsLabel = true; // This will display Data Label on the bar.
            this.chart1.Series["Votos"]["LabelStyle"] = "Bottom";  // This will change Label Position
            this.chart1.Series["Votos"].LabelForeColor = Color.White;
            this.chart1.Series["Votos"].Font = new Font("Courier New", 14, FontStyle.Bold);
        }
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

        private void UserControlVoting_Load(object sender, EventArgs e)
        {
            this.ventana_resultados = new FormResultados(new Point(this.Left + this.Width, this.Top));
            this.estado_conexion_grafismo = 0;
            if (CAjustes.conexion_grafismo)
            {
                // Cliente
                if (conecta_cliente())
                {

                    this.estado_conexion_grafismo = 1;
                }
                else
                {
                    this.estado_conexion_grafismo = 1;
                    inicia_reconexion_cliente();
                }
            }
        }

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
        int estado_conexion_grafismo;
        IPEndPoint endpoint;
        Socket socket;

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
        Thread reconexion;

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
    }
}
