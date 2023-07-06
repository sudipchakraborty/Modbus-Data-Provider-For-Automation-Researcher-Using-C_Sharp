using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TB_Manager;

using Tools;

namespace Modbus_Data_Provider
{
    public partial class frm_main : Form
    {
        SERIAL device; 
        MSG msg;
        Palette_Motor lm = new Palette_Motor(1, 255);

        public frm_main()
        {
            InitializeComponent();
            msg=new MSG(lst_msg);
            device = new SERIAL(txt_comm_port.Text, 9600);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dg_register.Rows.Add(30);
            utilGrid.set_datagrid_row_header_width(dg_register, 3);
            utilGrid.Fill_Grid_Index(dg_register);
            lm.Fill_Data_Grid_With_Holding_Reg(dg_register, lm.Holding_Registor);
            dg_register.ClearSelection();

            device.open();

            if (device.IsOpen)
            {
                msg.push("Device connected OK");
                timer1.Interval= 1;
                timer1.Enabled= true;
            }
            else
            {
                msg.push("Unable to connect with device!!!");
                timer1.Enabled= false;
            }
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            device.close();
            msg.push("Device disconnected");
            timer1.Enabled= false;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            device.open(txt_comm_port.Text,9600);

            if (device.IsOpen)
            {
                msg.push("Device connected OK");
                timer1.Interval= 1;
                timer1.Enabled= true;
            }
            else
            {
                msg.push("Unable to connect with device!!!");
                timer1.Enabled= false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(device.received)
            {
                device.received = false;
                byte[] buffer = new byte[device.bfr_rx_ptr];
                Array.Copy(device.bfr_rx,0, buffer, 0,  device.bfr_rx_ptr);
                byte[] b= lm.process(buffer, dg_register);
                if(b !=null)device.send(b);
            }

 
        }
    }
}
