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

namespace Votaciones_App
{
    public partial class FormPpal : Form
    {

        BaseConnection _baseConn;
        BaseManage _baseManage;
        KeypadManage _keypadManage;
        Choices _choice;
        TrueFalse _truefalse;

        CFichero _ficherocsv = new CFichero();
        CFileXML _filexml = new CFileXML();
        List<Mando> lista_mandos;
        int tipo_recuento = 0;
        int estado_votacion = 0;
        Thread cuentaAtras;
        Thread aleatorio;
        int[] recuenta_opciones;
        string[] array_letras = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        string[] array_numeros = new String[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        string[] array_verdaderofalso = new String[] { "True","False" };
        FormResultados ventana_resultados = null;
        bool unregistered;
        IPEndPoint endpoint;
        Socket socket; 
        int estado_conexion_grafismo;
        Thread reconexion;


        public FormPpal()
        {
            InitializeComponent();
            cambia_panel("ajustes");
            this.radioButton_ajustes_cambiar_resp_Si.Checked = true;
            this.comboBox_ajustes_tipo_votacion.SelectedIndex = 0;
            this.panel_ajustes_num_opciones.Visible = true;
            this.numericUpDown_ajustes_mandos_inferior.Value = 1;
            this.numericUpDown_ajustes_mandos_superior.Value = 100;
            this.numericUpDown_ajustes_tiempo_crono.Value = 60;
            this.button_ajustes_aceptar.Enabled = false;
            this.button_apagar_mandos.Enabled = false;
            this.radioButton_ajustes_conexion_grafismo_Si.Checked = true;
            this.estado_conexion_grafismo = 0;
        }


        /*
         *  Carga incial al arrancar el programa
         */
        private void FormPpal_Load(object sender, EventArgs e)
        {
            carga_ajustes();
            ReloadLicense();

            // BaseConn
            this._baseConn = new SunVote.BaseConnection();
            this._baseConn.DemoMode = false;
            this._baseConn.DemoKeyIDs = "1-131"; // Para pruebas
            this._baseConn.IsWriteErrorLog = false;
            this._baseConn.BaseOnLine += new SunVote.IBaseConnectionEvents_BaseOnLineEventHandler(baseConn_BaseOnLine);
            this._baseConn.Open(1, Convert.ToString(CAjustes.base_antena)); //usb Connect                      
            this._keypadManage = new KeypadManage();
            ventana_resultados = new FormResultados(new Point(this.Left + this.Width, this.Top)); 
        }
        
        /// <summary>
        /// Connected event
        /// </summary>
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
                        break;
                    case 1:
                        sState = "Conexión con la base exitosa";
                        this.button_ajustes_aceptar.Enabled = true;
                        this.button_apagar_mandos.Enabled = true;
                        this.panel_indicador_conex_base.BackgroundImage = Votaciones_App.Properties.Resources.verde;
                        this._keypadManage.BaseConnection = this._baseConn;
                        this._baseManage = new BaseManage();
                        this._baseManage.BaseConnection = this._baseConn;
                        this._baseManage.SetBasicFeature(1, 0, 0, 0, 0, 1);
                        break;
                    case -1:
                        sState = "No puede soportarse este tipo de conexión";
                        this.button_ajustes_aceptar.Enabled = false;
                        this.button_apagar_mandos.Enabled = false;
                        this.panel_indicador_conex_base.BackgroundImage = Votaciones_App.Properties.Resources.rojo;
                        break;
                    case -2:
                        sState = "No se encuentra la base";
                        this.button_ajustes_aceptar.Enabled = false;
                        this.button_apagar_mandos.Enabled = false;
                        this.panel_indicador_conex_base.BackgroundImage = Votaciones_App.Properties.Resources.rojo;
                        break;
                    case -3:
                        sState = "Error del puerto";
                        this.button_ajustes_aceptar.Enabled = false;
                        this.button_apagar_mandos.Enabled = false;
                        this.panel_indicador_conex_base.BackgroundImage = Votaciones_App.Properties.Resources.rojo;
                        break;
                    case -4:
                        sState = "la conexión ha sido cerrada";
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
        private void reset_votacion()
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
                for (int i = 0; i < CAjustes.num_mandos; i++)
                {
                    this.lista_mandos.Add(new Mando());
                }                
                this.label_contador.Text = Convert.ToString(CAjustes.tiempo_crono);

                actualiza_recuento();                
                this.panel_indicador_estado.BackgroundImage = Votaciones_App.Properties.Resources.rojo;
            }));     
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
            this.chart1.Series.Clear();
            this.chart1.Series.Add("Votos");
            this.chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
            this.chart1.Series["Votos"].IsValueShownAsLabel = true; // This will display Data Label on the bar.
            this.chart1.Series["Votos"]["LabelStyle"] = "Bottom";  // This will change Label Position
            this.chart1.Series["Votos"].LabelForeColor = Color.White;
            this.chart1.Series["Votos"].Font = new Font("Courier New", 14, FontStyle.Bold);
        }

        /*
         *  Arranca la votacion
         */
        private void panel_play_Click(object sender, EventArgs e)
        {
            
            if (this.estado_votacion == 0)
            {
                reset_votacion();
                this.ventana_resultados.inicializa_grid();
                this.estado_votacion = 1;
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
                        

                        this._choice.BaseConnection = this._baseConn;

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

                        this._choice.BaseConnection = this._baseConn;

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
                        this._truefalse.BaseConnection = this._baseConn;
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
                    this.aleatorio = new Thread(modoAleatorio);                    
                    this.aleatorio.Start();
                }

                // Arranco la cuenta atrás
                this.cuentaAtras = new Thread(inicia_cuenta_atras);
                cuentaAtras.Start();
            }            
        }


        /*
         *  Inicia la cuenta atras y comprueba que llegue a 0
         */
        private void inicia_cuenta_atras()
        {            
                while (Convert.ToInt32(this.label_contador.Text) > 0)
                {
                    // Duero 1 s
                    Thread.Sleep(1000);                    
                    // Resto el contador
                    int c = Convert.ToInt32(this.label_contador.Text);                   
                    c = c-1;

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


        // Recive un voto
        void keyStatus(string id_base, int id_mando, string valor, double tiempo_respuesta)
        {
            if (id_mando > CAjustes.mando_superior || id_mando < CAjustes.mando_inferior)
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
            return id - CAjustes.mando_inferior;
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
                    if(CAjustes.tipo_votacion == 0){
                        this.lista_mandos[id_mando - 1].respuesta = array_numeros[res];
                    }

                    if(CAjustes.tipo_votacion == 1){
                        this.lista_mandos[id_mando - 1].respuesta = array_letras[res];
                    }

                    if(CAjustes.tipo_votacion == 2){
                        this.lista_mandos[id_mando - 1].respuesta = array_verdaderofalso[res];
                    }

                                      
                }
                Thread.Sleep(retardo);
            }           
        }

        private int calcula_respuesta_aleatoria(string clasif){
            char resultado; 
            Random r = new Random();
            int decisor = r.Next(0,100);
            int _1,_2,_3 = 0;

            switch (clasif.Length)
            {
                case 2:                    
                     _1 = r.Next(55,70);
                     _2 = 100 - _1;

                    if(Enumerable.Range(0,_2).Contains(decisor)){
                        resultado = clasif[1];                        
                    }else{
                        resultado = clasif[0];                        
                    }

                    break;
                case 3:                    
                     _1 = r.Next(45,55);
                     _2 = _1 - r.Next(15,20);
                     _3 = 100 - _1 - _2;

                     if (Enumerable.Range(0, _3).Contains(decisor))
                     {
                         resultado = clasif[2];
                     }
                     else if(Enumerable.Range(_3, _2).Contains(decisor))
                     {
                         resultado = clasif[1];
                     }else{
                         resultado = clasif[0];
                     }

                    break;               
                default:
                    return 0;
            }           
            return (int)resultado -49;
        }




        /*
         *  Actualiza el recuento de votos
         */
        private void actualiza_recuento()
        {           
            try{
                if (this.tipo_recuento == 0)
                {                   
                    double porcentaje = Convert.ToDouble(Decimal.Divide(recuenta_votados(), CAjustes.num_mandos)) * 100;
                    this.label_recuento.Text = Math.Round(porcentaje, 2) + "%";
                
                }
                else
                {                   
                    this.label_recuento.Text = Convert.ToString(recuenta_votados()) + "/" + Convert.ToString(CAjustes.num_mandos);
                } 
            }catch(Exception ex){}
            
        }


        /*
         *  Actualiza el gráfico según llegan los votos
         */
        private void actualiza_grafico( )
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
                        if (lista_mandos[j].respuesta == array_letras[i])
                        {
                            contador[i]++;
                        }
                    }
                    chart1.Series["Votos"].Points.AddXY(array_letras[i], contador[i]);
                    chart1.Refresh();
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
                    if(i == indice_ganador)
                        this.chart1.Series["Votos"].Points[i].Color = Color.LightGreen;
                    else
                        this.chart1.Series["Votos"].Points[i].Color = Color.Tomato;
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


        /*
         *  Guarda el resultado en un csv
         */ 
        private void guarda_resultados_csv()
        {
            int tiempo = CAjustes.tiempo_crono -  Convert.ToInt32(label_contador.Text);
            string resultados = "Tiempo de votacion: "+ tiempo +  " s\n";
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                resultados = resultados + (i+CAjustes.mando_inferior) + ";" +this.lista_mandos[i].respuesta + "\n";
            }
            this._ficherocsv.EscribeFichero(CAjustes.ruta_resultados+"Resultados.csv",false, resultados);
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
            if (File.Exists(CAjustes.ruta_ajustes))
            {
                this.numericUpDown_ajustes_mandos_inferior.Value = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "MandosInferior"));
                this.numericUpDown_ajustes_mandos_superior.Value = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "MandosSuperior"));
                this.numericUpDown_ajustes_tiempo_crono.Value = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono"));
                this.numericUpDown_base_antena.Value = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "BaseAntena"));
                                
                if (_filexml.LeerXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta") == "True")
                {
                    this.radioButton_ajustes_cambiar_resp_Si.Checked = true;
                }
                if (_filexml.LeerXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta") == "False")
                {
                    this.radioButton_ajustes_cambiar_resp_No.Checked = true;
                }

                this.numericUpDown_ajustes_num_opciones.Value = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones"));

                this.comboBox_ajustes_tipo_votacion.SelectedIndex = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion"));
                this.textBox_ajustes_resultados_path.Text = _filexml.LeerXml(CAjustes.ruta_ajustes, "RutaResultados");
               // CAjustes.comBaseChanel = Convert.ToInt32(_filexml.LeerXml(CAjustes.ruta_ajustes, "ComBaseChanel"));
                
                if (_filexml.LeerXml(CAjustes.ruta_ajustes, "ConexionGrafismo") == "True")
                {
                    this.radioButton_ajustes_conexion_grafismo_Si.Checked = true;
                }
                
                if (_filexml.LeerXml(CAjustes.ruta_ajustes, "ConexionGrafismo") == "False")
                {
                    this.radioButton_ajustes_conexion_grafismo_No.Checked = true;
                }
                this.textBox_ip.Text = _filexml.LeerXml(CAjustes.ruta_ajustes, "Ip");
            }
            
        }


        /*
         *   Handler boton aceptar ajustes
         */
        private void button_ajustes_aceptar_Click(object sender, EventArgs e)
        {
            if (this.unregistered)
            {
                MessageBox.Show(this.label_license.Text, "Error");
                return;
            }

            if (valida_ajustes())
            {
                // guarda en memoria
                CAjustes.mando_inferior = (int)this.numericUpDown_ajustes_mandos_inferior.Value;
                CAjustes.mando_superior = (int)this.numericUpDown_ajustes_mandos_superior.Value;
                CAjustes.num_mandos = CAjustes.mando_superior - CAjustes.mando_inferior + 1;
                CAjustes.base_antena = (int)this.numericUpDown_base_antena.Value;
                CAjustes.tiempo_crono = (int)this.numericUpDown_ajustes_tiempo_crono.Value;
                CAjustes.tipo_votacion = this.comboBox_ajustes_tipo_votacion.SelectedIndex;
                CAjustes.numero_opciones = (int)this.numericUpDown_ajustes_num_opciones.Value;
                CAjustes.ip = this.textBox_ip.Text;                
                if (this.radioButton_ajustes_cambiar_resp_Si.Checked)
                {
                    CAjustes.permitir_cambio_respuesta = true;
                }
                else
                {
                    CAjustes.permitir_cambio_respuesta = false;
                }
                CAjustes.ruta_resultados = this.textBox_ajustes_resultados_path.Text;
                if (this.radioButton_ajustes_conexion_grafismo_Si.Checked)
                {
                    CAjustes.conexion_grafismo = true;
                }
                else
                {
                    CAjustes.conexion_grafismo = false;
                }
               
                /*-----------------------------------------------------------------------------------------------*/

                // guarda fichero ajustes

                // En caso de no existir el fichero xml lo crea
                if (!this._filexml.siExiste(CAjustes.ruta_ajustes))
                {
                    // Añade los campos que contiene el xml a una lista
                    ArrayList lista = new ArrayList();
                    lista.Add("MandosInferior");
                    lista.Add("MandosSuperior");
                    lista.Add("TiempoCrono");
                    lista.Add("BaseAntena");
                    lista.Add("PermitirCambioRespuesta");
                    lista.Add("TipoVotacion");
                    lista.Add("NumeroOpciones");
                    lista.Add("RutaResultados");
                    lista.Add("ConexionGrafismo");
                    lista.Add("Ip");
                    _filexml.CreaFicheroVacio("Ajustes", lista, CAjustes.ruta_ajustes);
                }

                // Escribe el XML
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "MandosInferior", Convert.ToString(CAjustes.mando_inferior));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "MandosSuperior", Convert.ToString(CAjustes.mando_superior));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "TiempoCrono", Convert.ToString(CAjustes.tiempo_crono));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", Convert.ToString(CAjustes.base_antena));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta", Convert.ToString(CAjustes.permitir_cambio_respuesta));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "TipoVotacion", Convert.ToString(CAjustes.tipo_votacion));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "NumeroOpciones", Convert.ToString(CAjustes.numero_opciones));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "RutaResultados", Convert.ToString(CAjustes.ruta_resultados));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "ConexionGrafismo", Convert.ToString(CAjustes.conexion_grafismo));
                _filexml.EscribirXml(CAjustes.ruta_ajustes, "Ip", Convert.ToString(CAjustes.ip));


                reset_votacion();

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
               

                // Paso al panel principal
                cambia_panel("principal");
            }
        }

        /*
         *  Valida que los ajustes sean correctos
         */
        private bool valida_ajustes()
        {
            if (this.numericUpDown_ajustes_mandos_inferior.Value < 1 )
            {
                MessageBox.Show("El número de mandos no debe ser menor de 1", "Error");
                return false;
            }

            if (this.numericUpDown_ajustes_mandos_inferior.Value >= this.numericUpDown_ajustes_mandos_superior.Value)
            {
                MessageBox.Show("El rango de valores de los mandos no es válido", "Error");
                return false;
            }

            if (this.numericUpDown_ajustes_tiempo_crono.Value < 1 || this.numericUpDown_ajustes_tiempo_crono.Value > 1000)
            {
                MessageBox.Show("El tiempo del crono debe estar comprendido entre 1 y 1000", "Error");
                return false;
            }

            if (!this.radioButton_ajustes_cambiar_resp_Si.Checked && !this.radioButton_ajustes_cambiar_resp_No.Checked)
            {
                MessageBox.Show("Por favor, elija si se permite cambiar de respuesta o no", "Error");
                return false;
            }

            if (this.comboBox_ajustes_tipo_votacion.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, elija un tipo de votación", "Error");
                return false;
            }


            if (this.numericUpDown_ajustes_num_opciones.Value < 1 || this.numericUpDown_ajustes_num_opciones.Value > 10)
            {
                MessageBox.Show("EL número de opciones debe estar comprendido entre 1 y 10", "Error");
                return false;
            }


            if (!Directory.Exists(this.textBox_ajustes_resultados_path.Text))
            {
                MessageBox.Show("El directorio seleccionado no existe", "Error");
                return false;
            }
            return true;
        }


        /*
         *  Apaga todos los mandos
         */
        private void button_apagar_mandos_Click(object sender, EventArgs e)
        {
            this._keypadManage.RemoteOff(0);
        }        


        /*
         *  Cambia del panel de ajustes al principal y viceversa
         */
        public void cambia_panel(string nombre_panel)
        {
            if (nombre_panel == "ajustes")
            {
                this.WindowState = FormWindowState.Normal;
                this.Controls.Remove(this.panel_principal);
                this.Controls.Add(this.panel_ajustes);
                this.MinimumSize = new Size(800, 375);
                this.Size = new Size(800, 375);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
            }

            if (nombre_panel == "principal")
            {
                this.Controls.Remove(this.panel_ajustes);
                this.Controls.Add(this.panel_principal);
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
                this.textBox_ajustes_resultados_path.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }


        /*
         *  Para mostrar u ocultar el panel de número de opciones
         */
        private void comboBox_ajustes_tipo_votacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox_ajustes_tipo_votacion.SelectedIndex == 0 || this.comboBox_ajustes_tipo_votacion.SelectedIndex == 1)
            {
                this.panel_ajustes_num_opciones.Visible = true;
            }
            else
            {
                this.panel_ajustes_num_opciones.Visible = false;
            }
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
                    ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 01");
                    this.unregistered = true;
                    this.panel_license.BackColor = Color.LightPink;
                    this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 01";
                    this.panel_license.Visible = true;
                    this.label_license.Visible = true;
                    break;
                case "2":
                    ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 02");
                    this.unregistered = true;
                    this.panel_license.BackColor = Color.LightPink;
                    this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 02";
                    this.panel_license.Visible = true;
                    this.label_license.Visible = true;
                    break;
                case "3":
                    ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 03");
                    this.unregistered = true;
                    this.panel_license.BackColor = Color.LightPink;
                    this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 03";
                    this.panel_license.Visible = true;
                    this.label_license.Visible = true;
                    break;
                case "4":
                    ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 04");
                    this.unregistered = true;
                    this.panel_license.BackColor = Color.LightPink;
                    this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 04";
                    this.panel_license.Visible = true;
                    this.label_license.Visible = true;

                    break;
                case "5":
                    ToolTip1.SetToolTip(this.panel_info, "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 05");
                    this.unregistered = true;
                    this.panel_license.BackColor = Color.LightPink;
                    this.label_license.Text = "Ha habido un problema con la licencia. Por favor, póngase en contacto con su proveedor. #ERROR 05";
                    this.panel_license.Visible = true;
                    this.label_license.Visible = true;
                    break;
                default:
                    ToolTip1.SetToolTip(this.panel_info, "Licencia válida hasta el "+ ifLicense);
                    this.unregistered = false;
                    this.panel_license.BackColor = Color.Transparent;
                    this.label_license.ForeColor = Color.Black;
                    this.label_license.Text = "Licencia válida hasta el " + ifLicense;
                    this.panel_license.Visible = false;
                    this.label_license.Visible = false;  
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
            if(this.radioButton_ajustes_conexion_grafismo_Si.Checked){
                this.textBox_ip.Visible = true;
                this.textBox_puerto.Visible = true;
                this.label_ajustes_ip.Visible = true;
                this.label_ajustes_puerto.Visible = true;
            }
        }

        private void radioButton_ajustes_conexion_grafismo_No_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_ajustes_conexion_grafismo_No.Checked)
            {
                this.textBox_ip.Visible = false;
                this.textBox_puerto.Visible = false;
                this.label_ajustes_ip.Visible = false;
                this.label_ajustes_puerto.Visible = false;
            }
        }

       
    }
}
