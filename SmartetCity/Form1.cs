using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartetCity
{
    public partial class Form1 : Form
    {
        System.IO.Ports.SerialPort PuertoEntrada;
        bool abierto = true;
        string onOff="";
        int checarLumens=0;
        int triger = 1;
        
        public Form1()
        {
            InitializeComponent();
            PuertoEntrada = new System.IO.Ports.SerialPort();
            PuertoEntrada.PortName = "COM3";
            PuertoEntrada.BaudRate = 9600;
            PuertoEntrada.ReadTimeout = 500;
            PuertoEntrada.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Thread Hilo = new Thread(EvaluarInfo);
            Hilo.Start();
        }

        private void EvaluarInfo() {

            while (abierto)
            {
                try
                {
                    if (triger == 1)
                    {
                        triger = 2;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        string cadena = PuertoEntrada.ReadLine();
                        char[] cadenaFracmentada = cadena.ToCharArray();

                        for (int i = 0; i < cadenaFracmentada.Length; i++)
                        {
                            onOff += cadenaFracmentada[i];
                        }

                        cadena = "";
                        if (cadenaFracmentada[0] == 'F')
                        {
                            for (int i = 1; i < cadenaFracmentada.Length; i++)
                            {
                                cadena += cadenaFracmentada[i];
                            }
                            lumensFoco.Invoke(new MethodInvoker(
                             delegate
                             {
                                 lumensFoco.Text = cadena;
                                 checarLumens = Convert.ToInt32(lumensFoco.Text);
                             }
                             ));

                        }

                        if (cadenaFracmentada[0] == 'S')
                        {
                            for (int i = 1; i < cadenaFracmentada.Length; i++)
                            {
                                cadena += cadenaFracmentada[i];
                            }
                            lumensAmbiente.Invoke(new MethodInvoker(
                             delegate
                             {
                                 lumensAmbiente.Text = cadena;
                             }
                             ));
                        }

                        if (cadenaFracmentada[0] == 'P')
                        {
                            onOffBox.Image = Properties.Resources.switch_on;
                            onOffBox.Refresh();
                            onOffBox.Visible = true;

                        }

                        if (cadenaFracmentada[0] == 'A')
                        {
                            onOffBox.Image = Properties.Resources.switch_off;
                            onOffBox.Refresh();
                            onOffBox.Visible = true;
                        }

                        if (checarLumens > 960)
                        {
                            estadoChido.Image = Properties.Resources.palomita;
                            onOffBox.Refresh();
                            onOffBox.Visible = true;


                        }

                        if (checarLumens < 900 && checarLumens > 801)
                        {
                            estadoChido.Image = Properties.Resources.alerta;
                            onOffBox.Refresh();
                            onOffBox.Visible = true;
                        }

                        if (checarLumens < 800)
                        {

                            estadoChido.Image = Properties.Resources.tacha;
                            onOffBox.Refresh();
                            onOffBox.Visible = true;

                        }

                    }
                }
                catch
                {

                }

            }

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void lumensFoco_Click(object sender, EventArgs e)
        {

        }

        private void aver_Click(object sender, EventArgs e)
        {

        }
    }
}
