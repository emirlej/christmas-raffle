using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace christmas_raffle
{
    class Program
    {
        public class Pair
        {
            public string FirstName { get; set; }
            public string SecondName { get; set; }
        }

        public class PairJson
        {
            public List<Pair> Pairs { get; set; }
        }

        public static string ReadPairsFile(string FilePath)
        {
            //throw new NotImplementedException();
            return "Hello World";
        }

        public static List<string> AddAllNamesToList(PairJson data)
        {
            // Creates a list of all names in the JSON data
            List<string> names = new List<string>();

            foreach (var pair in data.Pairs)
            {
                names.Add(pair.FirstName);
                names.Add(pair.SecondName);
            }
            return names;
        }

        public static void WriteResultsToFile(List<Tuple<string, string>> Results)
        {
            StringBuilder csvcontent = new StringBuilder();
            string FileName = @"output.csv";
            // Header
            csvcontent.AppendLine("Giver, Receiver");

            foreach (var item in Results)
            {
                csvcontent.AppendLine($"{item.Item1},{item.Item2}");
            }


            // CAME HERE: Appends to csv instead of making a new one
            File.AppendAllText(FileName, csvcontent.ToString());
            //File.CreateText(FileName, csvcontent.ToString());

            Console.WriteLine($"File written to: {FileName}");
        }

        public class Lottery
        {
            public PairJson Data { get; }
            public List<Tuple<string, string>> Results { get; }
            public List<string> Givers;
            public List<string> Receivers;
            public int RestartCounter { get; set; }

            // Constructor
            public Lottery(PairJson data)
            {
                Data = data;
                Results = new List<Tuple<string, string>>();
                RestartCounter = 0;
            }

            private void CreateLists()
            {
                //Todo: Need to be same length
                Givers = AddAllNamesToList(this.Data);
                Receivers = new List<string>(Givers);
            }

            private void ShuffleLists()
            {
                Givers.Shuffle();
                Receivers.Shuffle();
            }
          
            private int CountListsTotal()
            {
                return Givers.Count + Receivers.Count;
            }

            private bool CheckIfPair(string giver, string receiver)
            {
                bool IsPair = false;

                foreach (Pair pair in Data.Pairs)
                {
                    string[] Names = {pair.FirstName, pair.SecondName};
                    //var Names = new List<string>{pair.FirstName, pair.SecondName};

                    if (Names.Contains(giver) & Names.Contains(receiver))
                        IsPair = true;
                }
                return IsPair;
            }

            public void Run()
            {
                // Initialize the lists and shuffle them
                CreateLists();
                ShuffleLists();

                // Should this be inside/outside the for loop?
                Random rng = new Random();

                                
                              
                // Loop until the lottery is done
                while (CountListsTotal() > 0)
                {
                    int NumGivers = Givers.Count;
                    int NumReceivers = Receivers.Count;
                    
                    // Fetch random items
                    int IRandomGiver = rng.Next(NumGivers);
                    int IRandomReceiver = rng.Next(NumReceivers);

                    // Pick random people
                    string Giver = Givers[IRandomGiver];
                    string Receiver = Receivers[IRandomReceiver];

                    bool IsPair = CheckIfPair(Giver, Receiver);

                    if (IsPair)
                    {
                        // Reset if two items left and they are in same pair
                        if (CountListsTotal() == 2)
                        {
                            Results.Clear();
                            CreateLists();
                            ShuffleLists();
                            // Count each time the restart is done
                            RestartCounter++;
                            continue;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        Results.Add(new Tuple<string, string>(Giver, Receiver));
                        // Remove items
                        Givers.RemoveAt(IRandomGiver);
                        Receivers.RemoveAt(IRandomReceiver);

                        //Console.WriteLine($"{Giver}, {Receiver}, {IsPair}");
                    }   
                }
            }
        }


        static void Main(string[] args)
        {
            // Read in the pairs json file
            //string JsonString = File.ReadAllText(@"../../input/pairs.json"); // W
            string JsonString = File.ReadAllText(@"pairs.json"); // W
            //Console.WriteLine(JsonString);

            // Convert to csharp object
            PairJson JsonData = JsonConvert.DeserializeObject<PairJson>(JsonString);

            // Start the lottery
            var Lottery = new Lottery(data: JsonData);
            Lottery.Run();

            // Results
            Console.WriteLine($"Restarted num times: {Lottery.RestartCounter}");
            var LotteryResults = Lottery.Results;
            foreach (var item in LotteryResults)
            {
                Console.WriteLine(item);
            }

            // Write results to a file
            WriteResultsToFile(LotteryResults);

            Console.ReadKey();
        }

    }

    static class MyExtensions
    {
        // Info about static classes: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members

        // Shuffle code found here: https://stackoverflow.com/questions/273313/randomize-a-listt
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list) 
        // this is similar to self in python. means that the function can be used such as this.Shuffle()
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                //Console.WriteLine($"n={n}, k={k}");
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
