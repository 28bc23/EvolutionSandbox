using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSandbox.Utils
{
    internal static class Commands
    {
        static bool ReadingCommand = false;
        static StringBuilder CurrCommandString = new StringBuilder();
        public static void ReadCommand()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if(ReadingCommand)
                {
                    if(keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine(CurrCommandString.ToString());
                        ReadingCommand = false;

                        CurrCommandString.Clear();
                        return;
                    }

                    if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        CurrCommandString.Length--;
                        if (CurrCommandString.Length == 0)
                            ReadingCommand = false;
                        return;
                    }

                    CurrCommandString.Append(keyInfo.KeyChar);

                }else if (keyInfo.KeyChar == ':')
                {
                    ReadingCommand = true;
                    CurrCommandString.Append(keyInfo.KeyChar);
                }
            }
        }

        public static string GetCurrCommand
        {
            get
            {
                return CurrCommandString.ToString();
            }
        }

    }
}
