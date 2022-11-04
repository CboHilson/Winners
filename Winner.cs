using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace MutliplayerCardApplication
{
    class Winner
    {
        // card values
        public const int A = 1;
        public const int CLUB = 1;
        public const int DIAMOND = 2;
        public const int HEART = 3;
        public const int SPADE = 4;
        public const int J = 11;
        public const int Q = 12;
        public const int K = 13;
        //Create method  to textfile
        public static async Task WriteFile()
        {
            string[] lines =
            {
               "Namel:AH,3C,8C,2S,JD",
               "Name2:KD,QH,10C,4C,AC",
               "Name3:6S,8D,3D,JH,2D",
               "Name4:5H,3S,KH,AS,9D",
               "Name5:JS,3H,2H,2C,4D"           
                };

            await File.WriteAllLinesAsync("abc.txt", lines);
        }

        // keeps the information about the players
        public class Player
        {
            private string name { get; set; }
            private int cardScore { get; set; }
            private int suitScore { get; set; }

            // constructor
            public Player(string name, int cardScore, int suitScore)
            {
                this.name = name;
                this.cardScore = cardScore;
                this.suitScore = suitScore;
            }

            // getters and setters
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
           
            public int CardScore
            {
                get { return cardScore; }
                set { cardScore = value; }
            }
            public int SuitScore
            {
                get { return suitScore; }
                set { suitScore = value; }
            }
        }


        // retrveie the value of the card
        public static int getCardValue(String cardLetter)
        {
            int cardValue = 0;
            switch (cardLetter)
            {
                case "A":
                    cardValue = A;
                    break;
                case "J":
                    cardValue = J;
                    break;
                case "Q":
                    cardValue = Q;
                    break;
                case "K":
                    cardValue = K;
                    break;
                default:
                    try
                    {
                        cardValue = int.Parse(cardLetter);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid card value " + cardLetter);
                        Environment.Exit(0);
                    }
                    break;
            }
            return cardValue;
        }

        // retrieve the suit of the card
        public static int getCardSuit(String cardSuit)
        {
            int cardSuitValue = 0;
            switch (cardSuit)
            {
                case "C":
                    cardSuitValue = CLUB;
                    break;
                case "D":
                    cardSuitValue = DIAMOND;
                    break;
                case "H":
                    cardSuitValue = HEART;
                    break;
                case "S":
                    cardSuitValue = SPADE;
                    break;
                default:
                    Console.WriteLine("Invalid card suit " + cardSuit);
                    Environment.Exit(0);
                    break;
            }
            return cardSuitValue;
        }


        // reads the file and constructs the ojects of the player class
        public static List<Player> readFile(string filename)
        {
            // list of players
            List<Player> players = new List<Player>();

            // read the file

            string executableLocation = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);
            filename = Path.Combine(executableLocation, "abc.txt");
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {

                        string[] parts = line.Split(":");
                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Error: invalid line in file");
                            Environment.Exit(0);
                        }

                        string playerName = parts[0];
                        string[] cards = parts[1].Split(",");
                        if (cards.Length != 5)
                        {
                            Console.WriteLine("Error: invalid line in file");
                            Environment.Exit(0);
                        }

                        int[] cardValues = new int[cards.Length];
                        int[] cardSuits = new int[cards.Length];
                        for (int i = 0; i < cards.Length; i++)
                        {
                            int length = cards[i].Length;
                            string cardValue = cards[i].Substring(0, length - 1);
                            string cardSuit = cards[i].Substring(length - 1);
                            cardValues[i] = getCardValue(cardValue);
                            cardSuits[i] = getCardSuit(cardSuit);
                        }

                        int cardScore = 0;
                        int suitScore = 0;
                        for (int i = 0; i < cards.Length; i++)
                        {
                            cardScore += cardValues[i];
                            suitScore += cardSuits[i];
                        }

                        Player player = new Player(playerName, cardScore, suitScore);
                        players.Add(player);


                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


            return players;
        }

        // finds the list of winners
        public static List<Player> printWinner(List<Player> players)
        {
            // find the maximum card score and suit score
            int maxCardScore = 0;
            int maxSuitScore = 0;
            foreach (Player player in players)
            {
                if (player.CardScore > maxCardScore)
                {
                    maxCardScore = player.CardScore;
                    maxSuitScore = player.SuitScore;
                }
                else if (player.CardScore == maxCardScore)
                {
                    if (player.CardScore > maxSuitScore)
                    {
                        maxSuitScore = player.SuitScore;
                    }
                }
            }

            // find the list of winners
            List<Player> winners = new List<Player>();
            foreach (Player player in players)
            {
                if (player.CardScore == maxCardScore && player.SuitScore == maxSuitScore)
                {
                    winners.Add(player);
                }
            }

            // return the list of winners
            return winners;
        }


        // prints the winners
        public static void displayWinners(List<Player> winners, string outfilename)
        {
            string executableLocation = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);
            outfilename = Path.Combine(executableLocation, "xyz.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(outfilename))
                {
                    writer.WriteLine("Winner Name, Card Score");
                    foreach (Player player in winners)
                    {
                        writer.WriteLine(player.Name + ", " + player.CardScore);
                    }
                }  
           
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: unable to write to file:");
                Console.WriteLine(e.Message);
            }
        }


       public static async Task Main(string[] args)
        {
            await WriteFile();

          
            // check if the arguments are valid

            if (args.Any(a => a.Equals("--in") || args.Any(a => a.Equals("--out"))))
            {
                Console.WriteLine("Error: invalid argument " + args[0]);
                Environment.Exit(0);
            }

            // get input and output filenames based on the arguments
            string filename, outfilename;
            if (args.Any(a => a.Equals("--in")))
            {
                filename = args.FirstOrDefault(a => a.Equals(3));
                outfilename = args.FirstOrDefault(a => a.Equals(7));
            }
            else
            {
                filename = args.FirstOrDefault(a => a.Equals(4));
                outfilename = args.FirstOrDefault(a => a.Equals(1));
            }

            // process input file and write output
            List<Player> players = readFile(filename);
            List<Player> winners = printWinner(players);
            displayWinners(winners, outfilename);
            
        }

    }
}
