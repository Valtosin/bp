/* 
 * Old BYOND C# patcher (originally written by vetwiz)
 * Only can work with 512 BYOND, sorry. But you can modify it according to bytes from this old guide: https://ohthis.valtos.now.sh/byond
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByondPatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get exe path
            string p = "dreamseeker.exe";
            if (args.Length > 0 && File.Exists(args[0]))
            {
                p = args[0];
            }
            else
            {
                Console.WriteLine("Input path to dreamseeker.exe file (or drag and drop dreamseeker.exe on this patcher.exe instead):");
                p = Console.ReadLine();
            }

            if (!File.Exists(p))
            {
                Console.WriteLine("Error: exe path didn't exist");
                Console.ReadLine();
                return;
            }

            //Load executable
            byte[] b0 = File.ReadAllBytes(p);
            byte[] b1 = new byte[b0.Length];
            b0.CopyTo(b1, 0);

            //Patch
            int pos = 0;
            PatchTriplet(b1, 0x0f, 0x45, 0xf9, 0x89, 0xCF, 0x90, ref pos);
            PatchPair(b1, 0x74, 0x48, 0x90, 0x90, ref pos);
            PatchFive(b1, 0x0f, 0x84, 0x44, 0x02, 0x00, 0x90, 0x90, 0x90, 0x90, 0x90, ref pos);
            PatchPair(b1, 0x74, 0x4A, 0x90, 0x90, ref pos);
            PatchPair(b1, 0x74, 0x3F, 0x90, 0x90, ref pos);
            PatchPair(b1, 0x74, 0x0E, 0x90, 0x90, ref pos);
            PatchPair(b1, 0x74, 0x0E, 0x90, 0x90, ref pos);
            PatchPair(b1, 0x74, 0x4F, 0x90, 0x90, ref pos);

            //Check old exe and new exe diff
            int diff = 0;
            for (int i = 0; i < b0.Length; i++)
            {
                if (b0[i] != b1[i])
                {
                    diff++;
                }
            }
            Console.WriteLine("Bytes changed = " + diff.ToString());

            //Save results
            File.WriteAllBytes(p, b1);
            Console.WriteLine("All done.");
            Console.ReadLine();
        }

        private static void PatchFive(byte[] b, byte A0, byte A1, byte A2, byte A3, byte A4, byte B0, byte B1, byte B2, byte B3, byte B4, ref int i)
        {
            for (; i < b.Length; i++)
            {
                if (b[i] == A0)
                {
                    if (b[i + 1] == A1)
                    {
                        if (b[i + 2] == A2)
                        {
                            if (b[i + 3] == A3)
                            {
                                if (b[i + 4] == A4)
                                {
                                    Console.WriteLine("Pattern " + 
                                        A0.ToString() + A1.ToString() + A2.ToString() + A3.ToString() + A4.ToString() +
                                        " Found at " + i.ToString() + ", patching with " +
                                        B0.ToString() + B1.ToString() + B2.ToString() + B3.ToString() + B4.ToString() +
                                        "...");

                                    b[i] = B0;
                                    b[i + 1] = B1;
                                    b[i + 2] = B2;
                                    b[i + 3] = B3;
                                    b[i + 4] = B4;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void PatchTriplet(byte[] b, byte A0, byte A1, byte A2, byte B0, byte B1, byte B2, ref int i)
        {
            for (; i < b.Length; i++)
            {
                if (b[i] == A0)
                {
                    if (b[i + 1] == A1)
                    {
                        if (b[i + 2] == A2)
                        {
                            Console.WriteLine("Pattern " + A0.ToString() + A1.ToString() + A2.ToString() +
                                " Found at " + i.ToString() + ", patching with " +
                                B0.ToString() + B1.ToString() + B2.ToString() + "...");

                            b[i] = B0;
                            b[i + 1] = B1;
                            b[i + 2] = B2;
                            break;
                        }
                    }
                }
            }
        }

        private static void PatchPair(byte[] b, byte A0, byte A1, byte B0, byte B1, ref int i)
        {
            for (; i < b.Length; i++)
            {
                if (b[i] == A0)
                {
                    if (b[i + 1] == A1)
                    {
                        Console.WriteLine("Pattern " + A0.ToString() + A1.ToString() +
                            " Found at " + i.ToString() + ", patching with " +
                            B0.ToString() + B1.ToString() + "...");

                        b[i] = B0;
                        b[i + 1] = B1;
                        break;
                    }
                }
            }
        }
    }
}
