using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB_Manager
{
    public static class gbl
    {
        public static byte[] bfr_sciA_rx = new byte[255];   static int ptr_sciA_rx = 0;
        public static byte[] bfr_sciA_tx = new byte[255];   static int ptr_sciA_tx = 0;

        public static byte[] bfr_sciB_rx = new byte[255];  
        static int ptr_sciB_rx = 0;
        public static bool sciB_received = false;


        public static byte[] bfr_sciB_tx;  static int ptr_sciB_tx = 0;
        public static byte[] bfr_sciC_rx = new byte[255];  static int ptr_sciC_rx = 0;
        public static byte[] bfr_sciC_tx = new byte[255];  static int ptr_sciC_tx = 0;

        public static string cmd_tower_lamp = "";

        public static string msg_debug = "";
    }
}
