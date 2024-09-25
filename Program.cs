namespace CashMashine_Lab4_SUT24;

class Program
{
    static void Main(string[] args)
    {
    //Initiate a multi dim array for storing user nr, id(personal nummber) and passsword of the five customers
        //need to be of type long to store the personal numbers
        long[,] customers = { {1, 8203053536, 5435}, {2, 6412235334, 6334}, {3, 9704304647, 2674}, 
            {4, 5901174536, 8512}, {5, 9310224650, 1452} };
        //An array of arrays is declared for storing variables for type double. Intended for storing bank
        //accounts later on. It will store bank accounts for 5 users. 
        double[][] bankAccounts = new double [5][];
        
        bool runApp = true;
        while (runApp)
        {
            Console.WriteLine("Välkommen till din internetbank\n\n" +
                              "[1] Logga in\n" +
                              "[2] Stäng ner"); 

            switch (Console.ReadLine())
            {
                case "1":
                    LogIn(customers);
                    break;
                case "2":
                    Console.WriteLine("Välkommen åter!");
                    runApp = false;
                    break;
                default:
                    Console.WriteLine("Felaktigt val! Försök igen!");
                    break;
            }

        }
    }
    /// <summary>
    /// A method for user login
    /// </summary>
    public static long LogIn(long[,] customer)
    {
        //need variable to store the user input
        
        long personalNr;
        bool correctPersNr = false;

        do
        {
            Console.Clear();
            Console.Write("Ange ditt personnummer (YYMMDDXXXX): ");
            string inputPersNr = Console.ReadLine();
            if ((inputPersNr.Length == 10) && (Int64.TryParse(inputPersNr, out personalNr)))
            {
                correctPersNr = true;
            }
            else
            {
                Console.WriteLine("Felaktig inmatning!\n\nTryck valfri tangent för att försöka igen");
                Console.ReadKey();
            }
        } while (!correctPersNr);

        int password = 0;
        bool correctPassword = false;

        do
        {
            Console.Clear();
            Console.Write("Ange ditt lösenord, 4 siffror: ");
            string inputPassword = Console.ReadLine();
            if ((inputPassword.Length == 4) && Int32.TryParse(inputPassword, out password))
            {
                correctPassword = true;
            }
            else
            {
                Console.WriteLine("Felaktig inmatning!\n\nTryck valfri tangent för att försöka igen");
                Console.ReadKey();
            }
        } while (!correctPassword);


    }
}