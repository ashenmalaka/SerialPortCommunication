using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortCommunication
{
    public partial class Form1 : Form
    {
        string dataOut;
        string dataIn;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cBoxComPort.Items.AddRange(ports);

            chBoxDTREnable.Checked = false;
            serialPort1.DtrEnable = false;
            chBoxRTSEnable.Checked = false;
            serialPort1.RtsEnable = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cBoxComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cBoxBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cBoxDataBits.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse((typeof(StopBits)), cBoxStopBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse((typeof(Parity)), cBoxParityBits.Text);

                serialPort1.Open();
                progressBar1.Value = 100;
                lblStatusCom.Text = @"ON";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatusCom.Text = @"OFF";
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
                lblStatusCom.Text = @"OFF";
            }
            else
            {
                MessageBox.Show("Serial Port is not opened", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                dataOut = tBoxDataOut.Text;
                serialPort1.Write(dataOut);
            }
        }

        private void chBoxDTREnable_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.DtrEnable = chBoxDTREnable.Checked;
        }

        private void chBoxRTSEnable_CheckedChanged(object sender, EventArgs e)
        {
            serialPort1.RtsEnable = chBoxRTSEnable.Checked;
        }

        private void btnClearDataOut_Click(object sender, EventArgs e)
        {
            if (tBoxDataOut.Text != "")
            {
                tBoxDataOut.Text = "";
                lblDataOutLength.Text = ""; 
            }
        }

        private void tBoxDataOut_TextChanged(object sender, EventArgs e)
        {
            var dataOutLength = tBoxDataOut.TextLength;
            lblDataOutLength.Text = $@"{dataOutLength:00}";
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(ShowData));
        }

        private void ShowData(object sender, EventArgs e)
        {
            tBoxDataIN.Text += dataIn;
            //tBoxDataIN.Text = dataIn;

            var dataInLength = tBoxDataIN.TextLength;
            lblDataINLength.Text = $@"{dataInLength:00}";
        }

        private void btnClearDataIN_Click(object sender, EventArgs e)
        {
            if (tBoxDataIN.Text != "")
            {
                tBoxDataIN.Text = "";
                lblDataINLength.Text = "";
            }
        }
    }
}
