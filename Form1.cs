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

namespace 임베디드_어플리케이션_구현
{
    public partial class winform : Form
    {
        private SerialPort serialPort = new SerialPort();

        public winform()
        {
            InitializeComponent();
        }

        private void PortNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("HELLO WORLD");
            //Console.WriteLine("sender : " + sender);
            //Console.WriteLine("EventArgs : " + e);
            ComboBox cb = (ComboBox)sender;
            Console.Write("Selected Idx : " + cb.SelectedIndex + " ");
            Console.WriteLine("Selected Value : " + cb.Items[cb.SelectedIndex]);

        }
        private void SerialPort_DataRecv(object sender, EventArgs e)
        {
            String recvData = this.serialPort.ReadLine();
            Console.WriteLine(recvData);

            //스레드 생성 코드
            //Invoke(new Action( ()=>{ }));

            /* LED 점등 유무 확인 스레드 */
            if (recvData.StartsWith("LED:"))
            {
                Invoke(new Action(() => { Console.WriteLine(recvData); this.txtArea.AppendText(recvData + "\r\n"); }));
            }
            /* 온도 센서 확인 스레드 */
            if (recvData.StartsWith("TEMP :"))
            {
                Invoke(new Action(() => { this.TEMP_BOX.Text = ""; this.TEMP_BOX.Text = recvData.Replace("TEMP : ", ""); }));
            }
            /* 조도 센서 확인 스레드 */
            if (recvData.StartsWith("SUN :"))
            {
                Invoke(new Action(() => { this.sun_txt.Text = ""; this.sun_txt.Text = recvData.Replace("SUN : ", ""); }));
            }
            /* 초음파 센서 확인 스레드 */
            if (recvData.StartsWith("DIS :"))
            {
                Invoke(new Action(() => { this.DIS_TXT.Text = ""; this.DIS_TXT.Text = recvData.Replace("DIS : ", ""); }));
            }
        }

        private void conn_btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Conn_Btn Click : " + this.PortNumber.Items[this.PortNumber.SelectedIndex].ToString());
            try
            {
                this.serialPort.PortName = this.PortNumber.Items[this.PortNumber.SelectedIndex].ToString();
                this.serialPort.BaudRate = 9600;
                this.serialPort.DataBits = 8;
                this.serialPort.StopBits = System.IO.Ports.StopBits.One;
                this.serialPort.Parity = System.IO.Ports.Parity.None;
                this.serialPort.Open();
                Console.WriteLine("CONNECTION SUCCESS");
                this.txtArea.AppendText("Connected.. \r\n");
                this.serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataRecv);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                this.serialPort.Close();
                this.txtArea.AppendText("");
                this.txtArea.AppendText("Fail.. " + ex + "\r\n");
            }
        }

        private void txtInsert_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort.Write("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort.Write("0");
        }
    }
}
