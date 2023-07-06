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
        SERIAL device = new SERIAL("COM6", 9600);
        MSG msg;
        Palette_Motor lm = new Palette_Motor(11, 51);

        public frm_main()
        {
            InitializeComponent();
            msg=new MSG(lst_msg);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gbl.bfr_sciB_tx != null)
            {
                if ((gbl.bfr_sciB_tx[0]== 81) || (gbl.bfr_sciB_tx[0] == 59))
                {
                    gbl.bfr_sciB_rx = lm.process(gbl.bfr_sciB_tx, dg_register);
                }
                gbl.sciB_received = true;
                gbl.bfr_sciB_tx = null;
            }
        }
    }
}
