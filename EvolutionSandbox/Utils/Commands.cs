using System;
using System.Collections.Generic;
using System.Text;

namespace EvolutionSandbox.Utils
{
    internal static class Commands
    {
        static bool ReadingCommand = false;
        static StringBuilder CurrCommandBuilder = new StringBuilder();
        public static void ReadCommand()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if(ReadingCommand)
                {
                    if(keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine(CurrCommandBuilder.ToString());
                        ReadingCommand = false;

                        switch (CurrCommandBuilder.ToString())
                        {
                            case ":graph":
                                Configuration.EventVariables.SaveGraph = true;
                                break;
                            case ":quit!":
                                Environment.Exit(0);
                                break;
                            default:
                                break;
                        }

                        CurrCommandBuilder.Clear();
                        return;
                    }

                    if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        CurrCommandBuilder.Length--;
                        if (CurrCommandBuilder.Length == 0)
                            ReadingCommand = false;
                        return;
                    }

                    CurrCommandBuilder.Append(keyInfo.KeyChar);

                }else if (keyInfo.KeyChar == ':')
                {
                    ReadingCommand = true;
                    CurrCommandBuilder.Append(keyInfo.KeyChar);
                }
            }
        }

        public static string GetCurrCommand
        {
            get
            {
                return CurrCommandBuilder.ToString();
            }
        }

    }
}
