using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Projet_Techno
{
    public partial class Form1 : Form
    {
       

        delegate void SetTextCallback(string text);

        public Form1()
        {
   
            InitializeComponent();
            // Init Ports
            string[] PortsArray = null;
            int i = 0;

            PortsArray = SerialPort.GetPortNames();
            foreach (string c in PortsArray) 
            {
                comboBox1.Items.Add(PortsArray[i]);
                i++;
            }

            // Add Data receiver to the serial port
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

             }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lastPort = serialPort1.PortName;
            try
            {   
                if (serialPort1.IsOpen)
                    serialPort1.Close();

                serialPort1.PortName = "COM5";// Convert.ToString(comboBox1.Text);
                Console.Write(serialPort1.PortName);
                serialPort1.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("This port is already used", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (serialPort1.IsOpen)
                    serialPort1.Close();
                
                serialPort1.PortName = lastPort;
                Console.Write(serialPort1.PortName); // Debug
                serialPort1.Open();
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // Get the sender
            SerialPort sp = (SerialPort)sender;
            // Get Data from the sender
            string indata = sp.ReadLine();

           this.BeginInvoke(new SetTextCallback(SetText), indata);
        }

        private void SetText(string text)
        {

            /*
                We can say that a good range from the sensor is 8-30
                As we have a window with 500 height, we need to scale 
                8-30 is slided to 0->22
                0->0
                22->500-70 = 430
                So the transformation is -8 then *(430/22) = 19.5 
           
    */


            int newPos;
            bool r;
            bool sucess = int.TryParse(text, out newPos);
            newPos -= 8;
            newPos *= 19;
            if (newPos >= 390)
                newPos = 390; 

            
                Player1.Location = new Point(8, newPos);
            
        
        }


    }
}
