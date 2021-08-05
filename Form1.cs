using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MecaniqueUK;

namespace WindowsFormsApplication1
{
    public partial class USB_FORM : Form
    {

/***************************************************/
// Variables definidas por el usuario.
        UInt32 controlador;
        uint ADCValue1 = 0;
        uint ADCValue2 = 0;
        uint ADCValue3 = 0;
        uint ADCValue4 = 0;
        bool tick = false, run = false;
        int LSTO = 0, count = 0;
       byte[] BufferOUT = new byte[32];
       byte[] a = new byte[1200];
       string comando,x1,x2,x3,x4,x5,x6,x7,x8,x9,x10,x11,x12;
       string pathc;
       string[] Buffer1;
       byte r;
/***************************************************/
// Pos de actuadores
        byte[] pos = new byte[12];
        byte[] spos = new byte[1200];
        public int upos, apos;
        
 
/***************************************************/

        private void bres(byte x,byte y)
        {
            
            if (x > y)
                x--;
            if (x < y)
                x++;

        }
       private void lee(string dir)
       {
           pathc = System.AppDomain.CurrentDomain.BaseDirectory + dir + ".Robot";
           if (System.IO.File.Exists(pathc))
           {
               Buffer1 = System.IO.File.ReadAllLines(pathc);
               MENSAJES_USB.Items.Add(Buffer1.Length.ToString());
               MessageBox.Show("Se termino de leer Comandos");
               richTextBox2.Text = string.Join(".", Buffer1);
           }
           else
           {
               try
               {
                   /*
                   string[] def = { iplc, iplcpath, tagdint, tagdintleng, tagstring, tagstrinleng, folderimg, password };
                   System.IO.File.WriteAllLines(pathc, def);
                   conf = System.IO.File.ReadAllLines(pathc);
                    */
                   MessageBox.Show("No Existe comando");
               }
               catch
               {
                   MessageBox.Show("Ocurrio un Error al leer comando.");
             
               }
           }
       }

       private void scrib(string dir)
       {
           try
           {
               pathc = System.AppDomain.CurrentDomain.BaseDirectory + dir + ".Robot";
               System.IO.File.WriteAllLines(pathc, Buffer1);
           }
           catch {
               MessageBox.Show("Ocurrio un Error al Excribir comando.");
           }


       }

        public USB_FORM()
        {
            InitializeComponent();
            MENSAJE_TOOL.SetToolTip(this.CONTENEDOR_ANALOGICAS, 
            "Barras de progreso que muestran el valor medido en el CAD.");
            MENSAJE_TOOL.SetToolTip(this.CONTENEDOR_PWM, 
            "Control con el cuál variamos el ciclo de trabajo de los PWM.");
            MENSAJE_TOOL.SetToolTip(this.CONTENEDOR_SALIDAS, 
            "Control con el cuál cambiamos el estado lógico de los led's de salida.");
            MENSAJE_TOOL.SetToolTip(this.CONTENEDOR_IN_DIGITALES, 
            "Control que muestra el estado de los diferentes pulsadores.");
            MENSAJE_TOOL.SetToolTip(this.CONECTAR_DISPOSITIVO, 
            "Botón que nos permite enlazar el dispositivo al controlador.");
            MENSAJE_TOOL.SetToolTip(this.MENSAJES_USB, 
            "En esta ventana van apareciendo los diferentes sucesos ocurridos en la aplicación.");
        }

        private void FormMain_Load_1(object sender, EventArgs e)
        {
            deshabilita_controles();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            EasyHID.Disconnect();    
        }

        private void Dispositivo_Conectado(UInt32 handle)
        {
            if (EasyHID.GetVendorID(handle) == EasyHID.VENDOR_ID && EasyHID.GetProductID(handle) == EasyHID.PRODUCT_ID)
            {
                // Si el handler ha encontrado un dispositivo conectado...
                EasyHID.SetReadNotify(handle,true); // Activa el sistema de notificaciones.
              
                controlador = handle;
                textBox1.Text = Convert.ToString(controlador);
                LSTO = 1;
                MENSAJES_USB.Items.Add ("Dispositivo USB, Listo y conectado.");     
            }
        }

        private void Dispositivo_desconectado(UInt32 handle)
        {
            if (EasyHID.GetVendorID(handle) == EasyHID.VENDOR_ID && EasyHID.GetProductID(handle) == EasyHID.PRODUCT_ID)
            {
                LSTO = 0;
                MENSAJES_USB.Items.Add("Dispositivo USB, desconectado.");
                CONECTAR_DISPOSITIVO.BackColor = Color.Red;
                CONECTAR_DISPOSITIVO.ForeColor = Color.White;
                CONECTAR_DISPOSITIVO.Text = "CONECTAR DISPOSITIVO";
                deshabilita_controles();
                EasyHID.Disconnect();
               
            }
        }

       private void Lee_datos(UInt32 In_handle)
        {
            byte[] BufferINP = new byte[EasyHID.BUFFER_IN_SIZE]; // Declaramos el buffer de entrada.
            if ((EasyHID.Read(In_handle, out BufferINP)) == true) // Si hay datos, los procesamos...
            {
                BUFFO.Text = Convert.ToString(BufferINP[0]) + "-" + Convert.ToString(BufferINP[1]) + "-" + Convert.ToString(BufferINP[2])
+ "-" + Convert.ToString(BufferINP[3]) + "-" + Convert.ToString(BufferINP[4]) + "-" + Convert.ToString(BufferINP[5])
+ "-" + Convert.ToString(BufferINP[6]) + "-" + Convert.ToString(BufferINP[7]) + "-" + Convert.ToString(BufferINP[8])
+ "-" + Convert.ToString(BufferINP[9]) + "-" + Convert.ToString(BufferINP[10]) + "-" + Convert.ToString(BufferINP[11])
+ "-" + Convert.ToString(BufferINP[12]) + "-" + Convert.ToString(BufferINP[13]) + "-" + Convert.ToString(BufferINP[14])
+ "-" + Convert.ToString(BufferINP[15]) + "-" + Convert.ToString(BufferINP[16]) + "-" + Convert.ToString(BufferINP[17])
+ "-" + Convert.ToString(BufferINP[18]) + "-" + Convert.ToString(BufferINP[19]) + "-" + Convert.ToString(BufferINP[20])
+ "-" + Convert.ToString(BufferINP[21]) + "-" + Convert.ToString(BufferINP[22]) + "-" + Convert.ToString(BufferINP[23])
+ "-" + Convert.ToString(BufferINP[24]) + "-" + Convert.ToString(BufferINP[25]) + "-" + Convert.ToString(BufferINP[26])
+ "-" + Convert.ToString(BufferINP[27]) + "-" + Convert.ToString(BufferINP[28]) + "-" + Convert.ToString(BufferINP[29])
+ "-" + Convert.ToString(BufferINP[30]) + "-" + Convert.ToString(BufferINP[31]);

                // Según se haya presionado un pulsador, indicará el evento de forma gráfica.
                if (BufferINP[7] == 0x01) { IN_DIGITAL_1.BackColor = Color.GreenYellow; 
                    MENSAJES_USB.Items.Add("Botón 1 presionado."); }
                else { IN_DIGITAL_1.BackColor = Color.Green; }
                if (BufferINP[8] == 0x01) { IN_DIGITAL_2.BackColor = Color.GreenYellow; }
                //  MENSAJES_USB.Items.Add("Botón 2 presionado."); }
                else { IN_DIGITAL_2.BackColor = Color.Green; }
                if (BufferINP[9] == 0x01) { IN_DIGITAL_3.BackColor = Color.GreenYellow; }
                  //  MENSAJES_USB.Items.Add("Botón 3 presionado."); }
                else { IN_DIGITAL_3.BackColor = Color.Green; }
                if (BufferINP[10] == 0x01) { IN_DIGITAL_4.BackColor = Color.GreenYellow; 
                    MENSAJES_USB.Items.Add("Botón 4 presionado."); }
                else { IN_DIGITAL_4.BackColor = Color.Green; }
                
                // Une la parte alta y baja de la conversión y guarda el dato en la variable
                // ADCxValue.
                ADCValue1 = (uint)(BufferINP[0] << 8) + BufferINP[1];
                ADCValue2 = (uint)(BufferINP[2] << 8) + BufferINP[3];
                ADCValue3 = (uint)(BufferINP[4] << 8) + BufferINP[5];
                ADCValue4 = (uint)(BufferINP[6]);
                // Muestra los valores en las barras de progreso.
                ADC1_VALUE.Value = (int)ADCValue1;
                ADC2_VALUE.Value = (int)ADCValue2;
                ADC3_VALUE.Value = (int)ADCValue3;
                ADC4_VALUE.Value = (int)ADCValue4;
            }
                                                                   
        }

        protected override void WndProc(ref Message message)
        {
            // Interceptamos los mensajes de windows.
            if (message.Msg == EasyHID.WM_HID_EVENT) // Si ha ocurrido algún evento...
            {
                switch (message.WParam.ToInt32()) // Intercepta el mensaje y opera según el valor recibido....
                {
                    case EasyHID.NOTIFY_PLUGGED:
                       Dispositivo_Conectado((UInt32)message.LParam.ToInt32()); // Se ha conectado un dispositivo.
                        break;
                    case EasyHID.NOTIFY_UNPLUGGED:
                       Dispositivo_desconectado((UInt32)message.LParam.ToInt32()); // Se ha desconectado un dispositivo.
                        break;
                    case EasyHID.NOTIFY_READ:
                        Lee_datos((UInt32)message.LParam.ToInt32()); // Hay datos en el buffer de entrada.
                        break;
                }
            }
            base.WndProc(ref message);
        }

        private void OUT_DIGITAL_1_Click_1(object sender, EventArgs e)
        { 
          // byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0x00;      // Report ID
            if (BufferOUT[14] == 0)
                BufferOUT[14] = 1;
            else
                BufferOUT[14] = 0;
          // EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: SALIDA1.");
           
        }
        private void OUT_DIGITAL_2_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0x00;      // Report ID
            if (BufferOUT[15] == 0)
                BufferOUT[15] = 1;
            else
                BufferOUT[15] = 0;
            EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: SALIDA2.");

        }
        private void OUT_DIGITAL_3_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            if (BufferOUT[16] == 0)
                BufferOUT[16] = 1;
            else
                BufferOUT[16] = 0;
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: SALIDA3.");

        }
        private void OUT_DIGITAL_4_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            if (BufferOUT[17] == 0)
                BufferOUT[17] = 1;
            else
                BufferOUT[17] = 0;
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: SALIDA4.");

        }
        private void OUT_DIGITAL_5_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            if (BufferOUT[18] == 0)
                BufferOUT[18] = 1;
            else
                BufferOUT[18] = 0;
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: Led Rojo.");

        }
        private void OUT_DIGITAL_6_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            if (BufferOUT[19] == 0)
                BufferOUT[19] = 1;
            else
                BufferOUT[19] = 0;
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: Led Verde.");

        }
        private void OUT_DIGITAL_7_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            if (BufferOUT[20] == 0)
                BufferOUT[20] = 1;
            else
                BufferOUT[20] = 0;
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado: Led Azul.");

        }
        private void OUT_DIGITAL_8_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            if(BufferOUT[21] == 0)
            BufferOUT[21] = 1;
            else
                BufferOUT[21] = 0;

          //  EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Lectura Sonic enviada");

        }
        private void OUT_DIGITAL_9_Click_1(object sender, EventArgs e)
        {
           // byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[14] = 1;
            BufferOUT[15] = 1;
            BufferOUT[16] = 1;
            BufferOUT[17] = 1;
            BufferOUT[18] = 1;
            BufferOUT[19] = 1;
            BufferOUT[20] = 1;
            BufferOUT[21] = 1;  
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado:Full ON.");

        }
        private void OUT_DIGITAL_10_Click_1(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[14] = 0; 
            BufferOUT[15] = 0;
            BufferOUT[16] = 0;
            BufferOUT[17] = 0;
            BufferOUT[18] = 0;
            BufferOUT[19] = 0;
            BufferOUT[20] = 0;
            BufferOUT[21] = 0;
        
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
            MENSAJES_USB.Items.Add("Cambia estado:Reset.");

        }

        private void CONECTAR_DISPOSITIVO_Click(object sender, EventArgs e)
        {
            if (tick == true)
            {
                EasyHID.Connect(Handle);
               // LSTO = 1;
                habilita_controles();
                CONECTAR_DISPOSITIVO.Text = "CONTROLES ACTIVOS";
                CONECTAR_DISPOSITIVO.BackColor = Color.GreenYellow;
                CONECTAR_DISPOSITIVO.ForeColor = Color.Black;
                tick = false;
                richTextBox2.Enabled = false;
            }
            else
            {
                EasyHID.Disconnect();
                LSTO = 0;
                deshabilita_controles();
                CONECTAR_DISPOSITIVO.Text = "HABILITAR CONTROLES DE  DISPOSITIVO";
                CONECTAR_DISPOSITIVO.BackColor = Color.DarkRed;
                CONECTAR_DISPOSITIVO.ForeColor = Color.White;
                tick = true;
                richTextBox2.Enabled = true;

            }

        }

        private void PWM_CONTROL_1_ValueChanged(object sender, EventArgs e)
        {
           // byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[2] = Convert.ToByte(PWM_CONTROL_1.Value);   // Luego enviamos los datos 
                                                                // del duty_cicle del PWM1.
           // EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }

        private void PWM_CONTROL_2_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[3] = Convert.ToByte(PWM_CONTROL_2.Value);   // Luego enviamos los datos 
                                                                // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_3_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[4] = Convert.ToByte(PWM_CONTROL_3.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_4_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[5] = Convert.ToByte(PWM_CONTROL_4.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_5_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[6] = Convert.ToByte(PWM_CONTROL_5.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_6_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[7] = Convert.ToByte(PWM_CONTROL_6.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_7_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[8] = Convert.ToByte(PWM_CONTROL_7.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_8_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[9] = Convert.ToByte(PWM_CONTROL_8.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_9_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[10] = Convert.ToByte(PWM_CONTROL_9.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_10_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[11] = Convert.ToByte(PWM_CONTROL_10.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_11_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[12] = Convert.ToByte(PWM_CONTROL_11.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }
        private void PWM_CONTROL_12_ValueChanged(object sender, EventArgs e)
        {
            //byte[] BufferOUT = new byte[EasyHID.BUFFER_OUT_SIZE];
            BufferOUT[0] = 0;      // Report ID
            BufferOUT[13] = Convert.ToByte(PWM_CONTROL_12.Value);   // Luego enviamos los datos 
            // del duty_cicle del PWM2.
            //EasyHID.Write(controlador, BufferOUT); // Envía los datos.
        }




        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            EasyHID.Disconnect();  
            this.Close();

        }

        private void acercaDeHIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("USB Pic HID con visual C# \n\n" +
                            "Diseño: Holotopo.\n" +
                            "Contacto: j.olivarez@hotmail.com\n" +
                            "Licencia: FREEWARE. \n", "Gracias internet", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void contactoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gracias a los foros jajaja", "Proyecto Creado:", 
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void habilita_controles() {

            // Habilita salidas digitales.
            OUT_DIGITAL_1.Enabled = true;
            OUT_DIGITAL_2.Enabled = true;
            OUT_DIGITAL_3.Enabled = true;
            OUT_DIGITAL_4.Enabled = true;
            OUT_DIGITAL_5.Enabled = true;
            OUT_DIGITAL_6.Enabled = true;
            OUT_DIGITAL_7.Enabled = true;
            OUT_DIGITAL_8.Enabled = true;
            OUT_DIGITAL_9.Enabled = true;
            OUT_DIGITAL_10.Enabled = true;

            // Habilita controles PWM.
            PWM_CONTROL_1.Enabled = true;
            PWM_CONTROL_2.Enabled = true;
            PWM_CONTROL_3.Enabled = true;
            PWM_CONTROL_4.Enabled = true;
            PWM_CONTROL_5.Enabled = true;
            PWM_CONTROL_6.Enabled = true;
            PWM_CONTROL_7.Enabled = true;
            PWM_CONTROL_8.Enabled = true;
            PWM_CONTROL_9.Enabled = true;
            PWM_CONTROL_10.Enabled = true;
            PWM_CONTROL_11.Enabled = true;
            PWM_CONTROL_12.Enabled = true;
            
            // Habilita entradas digitales.
            IN_DIGITAL_1.Enabled = true;
            IN_DIGITAL_2.Enabled = true;
            IN_DIGITAL_3.Enabled = true;
            IN_DIGITAL_4.Enabled = true;

            // Habilita entradas analógicas.
            ADC1_VALUE.Enabled = true;
            ADC2_VALUE.Enabled = true;
            ADC3_VALUE.Enabled = true;
            ADC4_VALUE.Enabled = true;

            // Habilita casilla de mensajes USB.
            MENSAJES_USB.Enabled = true;
        }

        private void deshabilita_controles() {
            // deshabilita salidas digitales.
            OUT_DIGITAL_1.Enabled = false;
            OUT_DIGITAL_2.Enabled = false;
            OUT_DIGITAL_3.Enabled = false;
            OUT_DIGITAL_4.Enabled = false;
            OUT_DIGITAL_5.Enabled = false;
            OUT_DIGITAL_6.Enabled = false;
            OUT_DIGITAL_7.Enabled = false;
            OUT_DIGITAL_8.Enabled = false;
            OUT_DIGITAL_9.Enabled = false;
            OUT_DIGITAL_10.Enabled = false;

            // deshabilita controles PWM.
            PWM_CONTROL_1.Enabled = false;
            PWM_CONTROL_2.Enabled = false;
            PWM_CONTROL_3.Enabled = false;
            PWM_CONTROL_4.Enabled = false;
            PWM_CONTROL_5.Enabled = false;
            PWM_CONTROL_6.Enabled = false;
            PWM_CONTROL_7.Enabled = false;
            PWM_CONTROL_8.Enabled = false;
            PWM_CONTROL_9.Enabled = false;
            PWM_CONTROL_10.Enabled = false;
            PWM_CONTROL_11.Enabled = false;
            PWM_CONTROL_12.Enabled = false;

            // deshabilita entradas digitales.
            IN_DIGITAL_1.BackColor = Color.Gray;
            IN_DIGITAL_1.Enabled = false;
            IN_DIGITAL_2.BackColor = Color.Gray;
            IN_DIGITAL_2.Enabled = false;
            IN_DIGITAL_3.BackColor = Color.Gray;
            IN_DIGITAL_3.Enabled = false;
            IN_DIGITAL_4.BackColor = Color.Gray;
            IN_DIGITAL_4.Enabled = false;


            // deshabilita entradas analógicas.
            ADC1_VALUE.Value = 0;
            ADC1_VALUE.Enabled = false;
            ADC2_VALUE.Value = 0;
            ADC2_VALUE.Enabled = false;
            ADC3_VALUE.Value = 0;
            ADC3_VALUE.Enabled = false;
            ADC4_VALUE.Value = 0;
            ADC4_VALUE.Enabled = false;

            // deshabilita casilla de mensajes USB.
            MENSAJES_USB.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CONTENEDOR_SALIDAS.Visible = false;
            CONTENEDOR_IN_DIGITALES.Visible = false;
            CONTENEDOR_PWM.Visible = false;
            CONTENEDOR_ANALOGICAS.Visible = false;
            CONECTAR_DISPOSITIVO.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            MENSAJES_USB.Visible = false;
            button1.Visible = false;
            label34.Visible = false;
            BUFF.Visible = false;
            BUFFO.Visible = false;
            button2.Visible = true;
            textBox1.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CONTENEDOR_SALIDAS.Visible = true;
            CONTENEDOR_IN_DIGITALES.Visible = true;
            CONTENEDOR_PWM.Visible = true;
            CONTENEDOR_ANALOGICAS.Visible = true;
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            CONECTAR_DISPOSITIVO.Visible = true;
            MENSAJES_USB.Visible = true;
            BUFF.Visible = true;
            BUFFO.Visible = true;
            label34.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = false;
        }

        
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (LSTO == 1 && checkBox4.Checked)
                if (checkBox1.Checked)
                {
                    timer3.Interval = Convert.ToInt16(numericUpDown1.Value);
                    if (BufferOUT[19] == 0)
                    {
                        BufferOUT[19] = 1;
                    }
                    else
                    {
                        BufferOUT[19] = 0;
                    }

                }
                else
                {
                    if (BufferOUT[19] == 1)
                    {
                        BufferOUT[19] = 0;
                    }
                }

            
        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            if (LSTO == 1 && checkBox4.Checked)
                if (checkBox2.Checked)
                {
                    timer4.Interval = Convert.ToInt16(numericUpDown2.Value);
                    if (BufferOUT[18] == 0)
                    {
                        BufferOUT[18] = 1;
                    }
                    else
                    {
                        BufferOUT[18] = 0;
                    }
                }
                else
                {
                    if (BufferOUT[18] == 1)
                    {
                        BufferOUT[18] = 0;
                    } 
                }
        }
       private void timer5_Tick(object sender, EventArgs e)
        {
            if (LSTO == 1 && checkBox4.Checked)
                if (checkBox3.Checked )
                {
                    timer5.Interval = Convert.ToInt16(numericUpDown3.Value);
                    if (BufferOUT[20] == 0)
                    {
                        BufferOUT[20] = 1;
                    }
                    else
                    {
                        BufferOUT[20] = 0;
                    }
                }
                else
                {
                    if (BufferOUT[20] == 1)
                    {
                        BufferOUT[20] = 0;
                    }
                }

        }
        //AUTOMODE
        private void TMR_Auto_Tick(object sender, EventArgs e)
        {
            if (Buffer1 != null)
            {
               // MENSAJES_USB.Items.Add(Buffer1.Length.ToString());

                if (numericUpDown4.Value >= 50)
                    TMR_Auto.Interval = Convert.ToInt16(numericUpDown4.Value);

                else { TMR_Auto.Interval = 50; }

                if (run && count < (Buffer1.Length-12) && count <= 1187)
                {

                    BufferOUT[2] = Convert.ToByte(Buffer1[count]);
                    BufferOUT[3] = Convert.ToByte(Buffer1[count + 1]);
                    BufferOUT[4] = Convert.ToByte(Buffer1[count + 2]);
                    BufferOUT[5] = Convert.ToByte(Buffer1[count + 3]);
                    BufferOUT[6] = Convert.ToByte(Buffer1[count + 4]);
                    BufferOUT[7] = Convert.ToByte(Buffer1[count + 5]);
                    BufferOUT[8] = Convert.ToByte(Buffer1[count + 6]);
                    BufferOUT[9] = Convert.ToByte(Buffer1[count + 7]);
                    BufferOUT[10] = Convert.ToByte(Buffer1[count + 8]);
                    BufferOUT[11] = Convert.ToByte(Buffer1[count + 9]);
                    BufferOUT[12] = Convert.ToByte(Buffer1[count + 10]);
                   BufferOUT[13] = Convert.ToByte(Buffer1[count + 11]);
                    count = count + 12;
                }  

                if  (count> (Buffer1.Length-12) || count > 1187)
                {
                    MENSAJES_USB.Items.Add("TERMINADO");
                    BufferOUT[2] = 0;
                    BufferOUT[3] = 0;
                    BufferOUT[4] = 0;
                    BufferOUT[5] = 0;
                    BufferOUT[6] = 0;
                    BufferOUT[7] = 0;
                    BufferOUT[8] = 0;
                    BufferOUT[9] = 0;
                    BufferOUT[10] = 0;
                    BufferOUT[11] = 0;
                    BufferOUT[12] = 0;
                    BufferOUT[13] = 0;
                    
                  
                    run = false;
                    count = 0;
                    TMR_Auto.Enabled = false;
                }


              
               
            }
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            comando = textBox2.Text;
            if (comando.Length != 0)
            {
                run = false;
                lee(comando);
               

                
            }
            else
            {
                MENSAJES_USB.Items.Add("Ingrese comando");
            }



        }

        private void button6_Click(object sender, EventArgs e)
        {

            comando = textBox2.Text;
            if (comando.Length != 0)
            {
                Buffer1 = richTextBox2.Text.Split('.');
               

                scrib(comando);
                //Buffer1 = text;
            }
            else
            {
                MENSAJES_USB.Items.Add( "Ingrese comando");
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (Buffer1.Length!=0)
            {

                MENSAJES_USB.Items.Add("EJECUTANDO");
               
                run = true;
                count = 0;
                TMR_Auto.Enabled = true;
            }
            else
            {
                MENSAJES_USB.Items.Add("No hay comandos en buffer");
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {

            string[] clean = null;
            Buffer1 = clean;
            run = false;
            count = 0;
            MENSAJES_USB.Items.Add("Se a limpiado");
            richTextBox2.Text = null;
            TMR_Auto.Enabled = false;

        }

        private void button7_Click(object sender, EventArgs e)
        {      
      //  richTextBox2.Text[Convert.ToInt16( numericUpDown5.Value)] = Convert.ToChar(Convert.ToByte(numericUpDown2.Value));

            x1 =Convert.ToString( Convert.ToByte(PWM_CONTROL_1.Value));
           // x1 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_1.Value)));
            x2 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_2.Value)));
            x3 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_3.Value)));
            x4 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_4.Value)));
            x5 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_5.Value)));
            x6 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_6.Value)));
            x7 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_7.Value)));
            x8 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_8.Value)));
            x9 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_9.Value)));
            x10 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_10.Value)));
            x11 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_11.Value)));
            x12 = Convert.ToString(Convert.ToChar(Convert.ToByte(PWM_CONTROL_12.Value)));
       
            textBox3.Text= x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10 + x11 + x12;
              

           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            statusStrip1.Items[0].Text = DateTime.Now.ToLongTimeString();

            if (MENSAJES_USB.Items.Count == 14)
                MENSAJES_USB.Items.Clear();


            BufferOUT[0] = 0x00;
            if (LSTO == 1)
            {
                EasyHID.Write(controlador, BufferOUT); // Envía los datos.
                BUFF.Text = Convert.ToString(BufferOUT[0]) + "-" + Convert.ToString(BufferOUT[1]) + "-" + Convert.ToString(BufferOUT[2])
     + "-" + Convert.ToString(BufferOUT[3]) + "-" + Convert.ToString(BufferOUT[4]) + "-" + Convert.ToString(BufferOUT[5])
     + "-" + Convert.ToString(BufferOUT[6]) + "-" + Convert.ToString(BufferOUT[7]) + "-" + Convert.ToString(BufferOUT[8])
     + "-" + Convert.ToString(BufferOUT[9]) + "-" + Convert.ToString(BufferOUT[10]) + "-" + Convert.ToString(BufferOUT[11])
     + "-" + Convert.ToString(BufferOUT[12]) + "-" + Convert.ToString(BufferOUT[13]) + "-" + Convert.ToString(BufferOUT[14])
     + "-" + Convert.ToString(BufferOUT[15]) + "-" + Convert.ToString(BufferOUT[16]) + "-" + Convert.ToString(BufferOUT[17])
     + "-" + Convert.ToString(BufferOUT[18]) + "-" + Convert.ToString(BufferOUT[19]) + "-" + Convert.ToString(BufferOUT[20])
     + "-" + Convert.ToString(BufferOUT[21]) + "-" + Convert.ToString(BufferOUT[22]) + "-" + Convert.ToString(BufferOUT[23])
     + "-" + Convert.ToString(BufferOUT[24]) + "-" + Convert.ToString(BufferOUT[25]) + "-" + Convert.ToString(BufferOUT[26])
     + "-" + Convert.ToString(BufferOUT[27]) + "-" + Convert.ToString(BufferOUT[28]) + "-" + Convert.ToString(BufferOUT[29])
     + "-" + Convert.ToString(BufferOUT[30]) + "-" + Convert.ToString(BufferOUT[31]);
            }
        }

        private void PWM_CONTROL_1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }




 







 }


}

