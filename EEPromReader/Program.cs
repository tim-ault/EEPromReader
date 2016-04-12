using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITLNET;
using System.IO;

namespace EEPromReader
{
    class Program
    {

        static void Main(string[] args)
        {
            string ComPort = "COM9";
            byte Address = 0x10;
            byte[] Parameter = new byte[6] { 0x30, 0x05, 0x49, 0x01, 0x00, 0x00 };
            byte[] PageContents = new byte [64];

            CSSPComs SSPComs = new CSSPComs(Address);
            SSPComs.OpenComPort(ComPort);
            SSPComs.ResetSequenceFlag();
            SSPComs.LoadCommand(0x11);
            SSPComs.Transmit();

            for (byte b = 0x00; b < 0x20; b++)
            {
                Parameter[5] = b;
                SSPComs.LoadCommand(Parameter);
                SSPComs.Transmit();
                Array.Copy(SSPComs.RXPacket, 4, PageContents, 0, 64);
                FileStream fs = new FileStream("HopperPage" + b.ToString("00") + ".dat", FileMode.Create);
                foreach (byte b1 in PageContents)
                {
                    fs.WriteByte(b1);
                }
                fs.Close();
            }

            Parameter[4] = 0x01;
            for (byte b = 0x00; b < 0x20; b++)
            {
                Parameter[5] = b;
                SSPComs.LoadCommand(Parameter);
                SSPComs.Transmit();
                Array.Copy(SSPComs.RXPacket, 4, PageContents, 0, 64);
                FileStream fs = new FileStream("FeederPage" + b.ToString("00") + ".dat", FileMode.Create);
                foreach (byte b1 in PageContents)
                {
                    fs.WriteByte(b1);
                }
                fs.Close();
            }

        }
    }
}
