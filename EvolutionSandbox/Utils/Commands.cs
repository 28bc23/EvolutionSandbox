using System.Text;
using static EvolutionSandbox.Utils.Configuration;

namespace EvolutionSandbox.Utils
{
    internal static class Commands
    {
        static bool ReadingCommand = false;
        static StringBuilder CurrCommandBuilder = new StringBuilder();

        #region Events
        public static event System.Action OnGraphCommand;
        public static event System.Action OnCreateCheckpoint;
        #endregion

        public static void ReadCommand()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (ReadingCommand)
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine(CurrCommandBuilder.ToString());
                        ReadingCommand = false;

                        string[] command = CurrCommandBuilder.ToString().Split(" ");
                        CurrCommandBuilder.Clear();

                        switch (command[0].ToLower())
                        {
                            case ":graph":
                                OnGraphCommand?.Invoke();
                                break;
                            case ":grid-size-x": // will take effect after restarting, and **will not be recomended**
                                if (command.Length > 1 && uint.TryParse(command[1], out uint gridX)) Config.GridSizeX = gridX;
                                break;
                            case ":grid-size-y":
                                if (command.Length > 1 && uint.TryParse(command[1], out uint gridY)) Config.GridSizeY = gridY;
                                break;
                            case ":seed": // **not recomended changing**
                                if (command.Length > 1 && ulong.TryParse(command[1], out ulong seed)) { Config.Seed = seed; Random.Init(seed, true); }
                                break;
                            case ":fps-cap":
                                if (command.Length > 1 && uint.TryParse(command[1], out uint fps)) { Config.FpsCap = fps; Program.RecalculateTargetFrameTime(); }
                                break;
                            case ":tps":
                                if (command.Length > 1 && uint.TryParse(command[1], out uint tps)) { Config.TPS = tps; Program.RecalculateFixedDeltaTime(); }
                                break;
                            case ":max-ticks-per-frame":
                                if (command.Length > 1 && double.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double maxTicks)) Config.MaxTicksPerFrame = Math.Max(1.0, maxTicks);
                                break;
                            case ":num-agents": // takes effect on spawn of new gen
                                if (command.Length > 1 && uint.TryParse(command[1], out uint agents)) Config.NumAgents = agents;
                                break;
                            case ":max-food-in-env":
                                if (command.Length > 1 && uint.TryParse(command[1], out uint maxFood)) Config.MaxFoodInEnv = maxFood;
                                break;
                            case ":food-spawn-rate":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float foodRate) && foodRate >= 0f) Config.FoodSpawnRate = foodRate;
                                break;
                            case ":food-energy": // takes effect on new food
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float foodEnergy) && foodEnergy >= 0f) Config.FoodEnergy = foodEnergy;
                                break;
                            case ":agent-max-energy": // takes effect on new gen
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float agentMaxE) && agentMaxE > 0f) Config.AgentMaxEnergy = agentMaxE;
                                break;
                            case ":agent-energy-drate":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float decreaseRate) && decreaseRate >= 0f) Config.AgentEnergyDecreaseRate = decreaseRate;
                                break;
                            case ":agent-wall-penalty":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float wallPenalty) && wallPenalty >= 0f) Config.AgentWallCollisionEnergyPenalty = wallPenalty;
                                break;
                            case ":graph-rate":
                                if (command.Length > 1 && uint.TryParse(command[1], out uint graphRate)) Config.GraphRate = graphRate;
                                break;
                            case ":weight-mutation-chance":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float wMutChance)) Config.WeightMutationChance = Math.Clamp(wMutChance, 0f, 1f);
                                break;
                            case ":bias-mutation-chance":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float bMutChance)) Config.BiasMutationChance = Math.Clamp(bMutChance, 0f, 1f);
                                break;
                            case ":split-mutation-chance":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float splitMutChance)) Config.SplitMutationChance = Math.Clamp(splitMutChance, 0f, 1f);
                                break;
                            case ":new-connection-mutation-chance":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float newConnChance)) Config.NewConnectionMutationChance = Math.Clamp(newConnChance, 0f, 1f);
                                break;
                            case ":new-node-mutation-chance":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float newNodeChance)) Config.NewNodeMutationChance = Math.Clamp(newNodeChance, 0f, 1f);
                                break;
                            case ":weight-mutation-size-min":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float wSizeMin)) Config.WeightMutationSizeMin = wSizeMin;
                                break;
                            case ":weight-mutation-size-max":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float wSizeMax)) Config.WeightMutationSizeMax = wSizeMax;
                                break;
                            case ":bias-mutation-size-min":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float bSizeMin)) Config.BiasMutationSizeMin = bSizeMin;
                                break;
                            case ":bias-mutation-size-max":
                                if (command.Length > 1 && float.TryParse(command[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float bSizeMax)) Config.BiasMutationSizeMax = bSizeMax;
                                break;
                            case ":save":
                                OnCreateCheckpoint?.Invoke();
                                break;
                            case ":save-config":
                                SaveConfig();
                                break;
                            case ":quit":
                                OnCreateCheckpoint?.Invoke();
                                Environment.Exit(0);
                                break;
                            case ":quit!":
                                Environment.Exit(0);
                                break;
                            default:
                                break;
                        }

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

                }
                else if (keyInfo.KeyChar == ':')
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