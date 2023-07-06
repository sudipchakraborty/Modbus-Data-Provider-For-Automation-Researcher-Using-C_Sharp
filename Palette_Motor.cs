using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using Tools;
namespace TB_Manager
{
    public class Palette_Motor
    {
        int unicast_id;
        int multicast_id;

        public UInt16[] Holding_Registor = new UInt16[300];
        ModbusRtuClient rtu;

        public Palette_Motor(byte unicast_id, byte multicast_id)
        {
            this.unicast_id=unicast_id;
            this.multicast_id=multicast_id;
            rtu=new ModbusRtuClient(unicast_id, multicast_id);

            for (UInt16 i=0;i<Holding_Registor.Length;i++)
            {
                Holding_Registor[i]=i;
            }

        }

        public byte[] process(byte[] data, DataGridView dg) 
        {
            if (!rtu.healthy(data)) return null;

            rtu.purse_packet(data, data.Length);

            if(rtu.function_code==0x03)
            {
                string s= Read_Data_From_Grid(dg, rtu.starting_addr);
                Holding_Registor[rtu.starting_addr]=Convert.ToUInt16(s);
            }
            if (rtu.function_code==0x06)
            {
                Write_Data_To_Grid(dg, rtu.starting_addr, rtu.Data[0].ToString());
            }
            if (rtu.slave_addr_rx!=rtu.multicast_addr)
            {
                rtu.Prepare_Response_Packet_Header();
                rtu.push_data(Holding_Registor[rtu.starting_addr]);
                rtu.close_packet();
                byte[] b = rtu.Get_Response_Packet();
                rtu.clear_tx_buffer();
                return b;
            }
            else
            {
                return null;
            }
        }

        public void Write_Data_To_Grid(DataGridView dg, int address, string data)
        {
            if (dg == null) return;
            int r = address/10;
            int c = address%10;
            dg[c, r].Value= data;
            dg.Rows[r].Cells[c].Style.BackColor = Color.Orange;
        }

        public string Read_Data_From_Grid(DataGridView dg, int address)
        {
            if (dg == null) return "";
            int r = address/10;
            int c = address%10;
            string s = dg[c, r].Value.ToString();
            dg.Rows[r].Cells[c].Style.BackColor = Color.LightGreen;
            return s;
        }

        public void Fill_Data_Grid_With_Holding_Reg(DataGridView dg, UInt16[] bfr)
        {
            int r = 0; int c = 0;
            for (int i = 0; i< 300; i++)
            {
                dg[c, r].Value=bfr[i];
                c++;
                if (c>15)
                {
                    c=0;
                    r++;
                }
            }
        }
    }
}
