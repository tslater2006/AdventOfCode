using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Misc
{
    public enum ElfOp
    {
        ADDR, ADDI, MULR, MULI, BANR, BANI, BORR, BORI, SETR, SETI, GTIR, GTRI, GTRR, EQIR, EQRI, EQRR
    }
    public struct ElfOpCode
    {
        public ElfOp Operation;
        public int A;
        public int B;
        public int C;

        public override string ToString()
        {
            return Enum.GetName(typeof(ElfOp), Operation) + " " + A + " " + B + " " + C;
        }
    }
    public class ElfCPU
    {
        public bool Debug = false;
        public override string ToString()
        {
            return "[" + String.Join<int>(',',Registers) + "]";
        }
        public ElfOpCode[] OpCodes;
        public int[] Registers = new int[6];
        private int IP;
        private int IPReg;
        public ElfCPU(string[] program)
        {
            List<ElfOpCode> parsedOpCodes = new List<ElfOpCode>();
            foreach(var s in program)
            {
                var parts = s.ToUpper().Split(' ');
                /* parse string */
                if (parts[0].Equals("#IP"))
                {
                    /* set the IP Register */
                    IPReg = int.Parse(parts[1]);
                    continue;
                }
                
                var opc = new ElfOpCode();
                opc.Operation = (ElfOp)Enum.Parse(typeof(ElfOp), parts[0]);
                opc.A = int.Parse(parts[1]);
                if (parts.Length > 2)
                {
                    opc.B = int.Parse(parts[2]);
                    opc.C = int.Parse(parts[3]);
                }
                parsedOpCodes.Add(opc);
            }
            OpCodes = parsedOpCodes.ToArray();
            IP = 0;
        }
        public void Run()
        {
            while (true)
            {
                Step();
                if (IP >= OpCodes.Length)
                {
                    break;
                }
            }
        }

        public void RunToIP(int ipTarget)
        {
            while (true)
            {
                Step();
                if (IP == ipTarget)
                {
                    return;
                }
            }
        }

        public void Step()
        {
            Registers[IPReg] = IP;

            if (Debug)
            {
                Console.Write("ip=" + IP + " " + this.ToString());
                Console.Write(" ");
                Console.Write(OpCodes[IP]);
            }

            ExecuteOp(OpCodes[IP]);

            /* read the IP from register */
            IP = Registers[IPReg];

            /* increment the IP */
            IP++;
            if (Debug)
            {
                Console.Write(" ");
                Console.WriteLine(this.ToString());
            }
        }

        public void ExecuteOp(ElfOpCode opc)
        {
            switch (opc.Operation)
            {
                case ElfOp.ADDR:
                    Registers[opc.C] = Registers[opc.A] + Registers[opc.B];
                    break;
                case ElfOp.ADDI:
                    Registers[opc.C] = Registers[opc.A] + opc.B;
                    break;
                case ElfOp.MULR:
                    Registers[opc.C] = Registers[opc.A] * Registers[opc.B];
                    break;
                case ElfOp.MULI:
                    Registers[opc.C] = Registers[opc.A] * opc.B;
                    break;
                case ElfOp.BANR:
                    Registers[opc.C] = Registers[opc.A] & Registers[opc.B];
                    break;
                case ElfOp.BANI:
                    Registers[opc.C] = Registers[opc.A] & opc.B;
                    break;
                case ElfOp.BORR:
                    Registers[opc.C] = Registers[opc.A] | Registers[opc.B];
                    break;
                case ElfOp.BORI:
                    Registers[opc.C] = Registers[opc.A] | opc.B;
                    break;
                case ElfOp.SETR:
                    Registers[opc.C] = Registers[opc.A];
                    break;
                case ElfOp.SETI:
                    Registers[opc.C] = opc.A;
                    break;
                case ElfOp.GTIR:
                    if (opc.A > Registers[opc.B])
                    {
                        Registers[opc.C] = 1;
                    }
                    else
                    {
                        Registers[opc.C] = 0;
                    }
                    break;
                case ElfOp.GTRI:
                    if (Registers[opc.A] > opc.B)
                    {
                        Registers[opc.C] = 1;
                    }
                    else
                    {
                        Registers[opc.C] = 0;
                    }
                    break;
                case ElfOp.GTRR:
                    if (Registers[opc.A] > Registers[opc.B])
                    {
                        Registers[opc.C] = 1;
                    }
                    else
                    {
                        Registers[opc.C] = 0;
                    }
                    break;
                case ElfOp.EQIR:
                    if (opc.A == Registers[opc.B])
                    {
                        Registers[opc.C] = 1;
                    }
                    else
                    {
                        Registers[opc.C] = 0;
                    }
                    break;
                case ElfOp.EQRI:
                    if (Registers[opc.A] == opc.B)
                    {
                        Registers[opc.C] = 1;
                    }
                    else
                    {
                        Registers[opc.C] = 0;
                    }
                    break;
                case ElfOp.EQRR:
                    if (Registers[opc.A] == Registers[opc.B])
                    {
                        Registers[opc.C] = 1;
                    }
                    else
                    {
                        Registers[opc.C] = 0;
                    }
                    break;
            }
        }
    }
}
