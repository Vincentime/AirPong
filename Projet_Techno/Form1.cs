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

        string name;

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

            // Set default name
            name = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    // Create string to send
                    //string text = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + name + " : " + textBox1.Text + "\n";
                    string text = textBox1.Text;
                    int toSend = int.Parse(text);
                    // Write your message
                    richTextBox1.AppendText(text);
                    byte[] b = BitConverter.GetBytes(toSend);
                    serialPort1.Write(b, 0, 1);

                    // Send to Serial Port
                   // serialPort1.Write(int.Parse(text) + "" );
        
         
                    // Erase text box
                    textBox1.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a port", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 1)
            {
                char t = textBox1.Text.ElementAt(textBox1.Text.Length - 1);
                if (t == '\n')
                {
                    button1_Click(this, e);
                    textBox1.Text = "";
                }
            }
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lastPort = serialPort1.PortName;
            try
            {   
                if (serialPort1.IsOpen)
                    serialPort1.Close();
             
                serialPort1.PortName = Convert.ToString(comboBox1.Text);
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
            string indata = sp.ReadExisting();

          // Byte[] b = BitConverter.GetBytes(indata);

            Console.Write(indata); // Debug           
            this.BeginInvoke(new SetTextCallback(SetText),indata);
        }

        private void SetText(string text)
        {
            this.richTextBox1.Text = text ;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            name += textBox2.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
