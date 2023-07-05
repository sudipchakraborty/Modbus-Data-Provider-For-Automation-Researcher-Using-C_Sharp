using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO.Ports;
using System.Threading;

namespace Tools
{
    public class SERIAL
    {
        string commPort;
        int BaudRate;
        long TimeOut_reg = 0;
        bool TimeCout = false;
        int count_BkUp = 0;
        SerialPort sp;

        public bool IsOpen=false;
        Thread th;
        public byte[] bfr_rx = new byte[1024];
        public int bfr_rx_ptr;
        public bool received=false;
        public bool exit = false;
       
        public SERIAL(string commPort, int BaudRate)
        {
            this.commPort = commPort;   
            this.BaudRate = BaudRate;
        }

        public bool open()
        {
            try
            {
                sp = new SerialPort(this.commPort, this.BaudRate, Parity.None, 8, StopBits.One);
                sp.Open();
                sp.ReadTimeout = 10000; // set the read timeout to 1 second
                IsOpen=true;
                th=new Thread(Read_Serial);
                th.Start();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool open(string commPort, int BaudRate)
        {
            try
            {
                sp = new SerialPort(commPort, BaudRate, Parity.None, 8, StopBits.One);
                sp.Open();
                sp.ReadTimeout = 10000; // set the read timeout to 1 second
                IsOpen=true;
                th=new Thread(Read_Serial);
                th.Start();
            }
            catch (Exception ex) 
            {
                return false;          
            }
            return true;
        }

        void Read_Serial()
        {
            while (!exit)
            {
                try
                {
                    int k = sp.BytesToRead;
                    if (k > 0)
                    {
                        if (k == 1)
                        {
                            TimeCout = true;
                        }
                        /////////////////////
                        if (k != (int)count_BkUp)
                        {
                            count_BkUp = k;
                            TimeOut_reg = 0;
                        }
                        else
                        {
                            Thread.Sleep(1);
                            TimeOut_reg++;
                            if (TimeOut_reg > 0)
                            {
                                byte[] buffer = new byte[sp.BytesToRead];
                                bfr_rx_ptr = sp.Read(buffer, 0, buffer.Length);
                                buffer.CopyTo(bfr_rx,0);
                                received= true;
                            }
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                }
            }// while (true)
        }// void Read_Serial()
    
        public byte[] Get_Bytes()
        {
            byte[] b = new byte[bfr_rx_ptr];
            Buffer.BlockCopy(bfr_rx, 0, b, 0, bfr_rx_ptr);
            received = false;
            return b;
        }

        public string Get_Bytes_String()
        { 
            byte[] b = new byte[bfr_rx_ptr];
            Buffer.BlockCopy(bfr_rx, 0, b, 0, bfr_rx_ptr);
            received = false;
            return BitConverter.ToString(b);
        }

        public void send(byte[] b)
        {
            sp.Write(b,0, b.Length);
        }

       public void purge()
        {
            bfr_rx_ptr=0;
        }

        public void close()
        {
            sp.Dispose();
            exit = true;
        }
    }
}
