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
       // SERIAL device; 
        MSG msg;
        //  Palette_Motor lm = new Palette_Motor(1, 255);
        ModbusRtuClient rtu;

        public frm_main()
        {
            InitializeComponent();
            msg=new MSG(lst_msg);
            rtu = new ModbusRtuClient(1, 255);

            for (UInt16 i = 0; i < rtu.Holding_Registor.Length; i++)
            {
                rtu.Holding_Registor[i] = i;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dg_register.DefaultCellStyle.ForeColor = Color.Black;
            dg_register.Rows.Add(30);
            utilGrid.set_datagrid_row_header_width(dg_register, 3);
            utilGrid.Fill_Grid_Index(dg_register);
            Fill_Data_Grid_With_Holding_Reg(dg_register, rtu.Holding_Registor);
            dg_register.ClearSelection();

           // device.open();
           RS232_Minimal.open(txt_comm_port.Text, 9600);

            if (RS232_Minimal.IsOpen)
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
            RS232_Minimal.close();
            msg.push("Device disconnected");
            timer1.Enabled= false;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            RS232_Minimal.open(txt_comm_port.Text,9600);

            if (RS232_Minimal.IsOpen)
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
            if(RS232_Minimal.received)
            {
                RS232_Minimal.received = false;
                byte[] b = rtu.Get_Response_Packet(RS232_Minimal.bfr_rx, RS232_Minimal.ptr_rx);              
                RS232_Minimal.send(b);
            } 
        }

        public void Write_Data_To_Grid(DataGridView dg, int address, string data)
        {
            if (dg == null) return;
            int r = address / 10;
            int c = address % 10;
            dg[c, r].Value = data;
            dg.Rows[r].Cells[c].Style.BackColor = Color.Orange;
        }

        public string Read_Data_From_Grid(DataGridView dg, int address)
        {
            if (dg == null) return "";
            int r = address / 10;
            int c = address % 10;
            string s = dg[c, r].Value.ToString();
            dg.Rows[r].Cells[c].Style.BackColor = Color.LightGreen;
            return s;
        }

        public void Fill_Data_Grid_With_Holding_Reg(DataGridView dg, UInt16[] bfr)
        {
            int r = 0; int c = 0;
            for (int i = 0; i < 300; i++)
            {
                dg[c, r].Value = bfr[i];
                c++;
                if (c > 9)
                {
                    c = 0;
                    r++;
                }
            }
        }

        private void dg_register_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode == Keys.Enter)
            //{
            //    int r=dg_register.CurrentCell.RowIndex;
            //    int c=dg_register.CurrentCell.ColumnIndex;
            //    string val = dg_register[c, r].Value.ToString();
            //}
        }

        private void dg_register_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int r = dg_register.CurrentCell.RowIndex;
            int c = dg_register.CurrentCell.ColumnIndex;
            string val = dg_register[c, r].Value.ToString();
            try
            {
                rtu.Holding_Registor[r * 10 + c] = Convert.ToUInt16(val);
            }
            catch (Exception ex)
            {

            }
        }

        private void txt_multicast_id_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                rtu.multicast_addr=Convert.ToByte(txt_multicast_id.Text);
            }
        }

        private void txt_unicast_id_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                rtu.slave_addr=Convert.ToByte(txt_unicast_id.Text);
            }
        }
    }
}
