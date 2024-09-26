//Jenny-Ann Hayward, SUT24

namespace CashMashine_Lab4_SUT24;

//========================================================CLASS========================================================
//A program class with the main method for running the program
//=====================================================================================================================
class Program
{
    static void Main(string[] args)
    {
        //Initiate a multi dim array for storing user nr, id(personal nummber) and passsword of the five customers
        //need to be of type long to store the personal numbers
        long[,] customers = { {1, 8204084647, 1234}, {2, 6412235334, 6334}, {3, 9704304647, 2674}, 
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
                    int customerIndex = MyMethods.LogIn(customers);
                    bool stayLoggedIn = MyMethods.HeadMenu(customerIndex);
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
}

//========================================================CLASS=========================================================
//A class for gathering all the methods for the program
//======================================================================================================================
class MyMethods
{
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for user login
    /// </summary>
    public static int LogIn(long[,] customer)
    {
        int indexRowCustomer = 0;
        bool runLogin = true;
        bool matchLogin = false;
        int numberTried = 0;

        while (runLogin)
        { 
            //need variable to store the user input
            long personNr = 0;
            bool correctPersNr = false;
        
            do
            {
                Console.Clear();
                Console.Write("Ange ditt personnummer (YYMMDDXXXX): ");
                string inputPersNr = Console.ReadLine();
                if ((inputPersNr.Length == 10) && (Int64.TryParse(inputPersNr, out personNr)))
                {
                    correctPersNr = true;
                }
                else
                {
                    Console.WriteLine("Felaktigt format på ditt personnummer!\n\nTryck valfri tangent för att försöka igen");
                    Console.ReadKey();
                }
            } while (!correctPersNr);

            long password = 0;
            bool correctPassword = false;

            do
            {
                Console.Clear();
                Console.Write("Ange ditt lösenord, 4 siffror: ");
                string inputPassword = Console.ReadLine();
                if ((inputPassword.Length == 4) && Int64.TryParse(inputPassword, out password))
                {
                    correctPassword = true;
                }
                else
                {
                    Console.WriteLine("Felaktigt format på lösenordet!\n\nTryck valfri tangent för att försöka igen");
                    Console.ReadKey();
                }
            } while (!correctPassword);
            
            for (int i = 0; i < customer.GetLength(0); i++)
            {
                if ((personNr == customer[i, 1]) && (password == customer[i, 2]))
                {
                    indexRowCustomer = i;
                    matchLogin = true;
                    runLogin = false;
                    break;
                }
            }
            numberTried++;

            if (!matchLogin)
            {
                Console.WriteLine("\nDitt personnummer eller lösenord stämmer inte");
                Console.WriteLine("\nTryck valfri tangent för att komma vidare!");
                Console.ReadKey();
            }

            if (numberTried == 3)
            {
                Console.Clear();
                Console.WriteLine("\nDu har dessvärre förbrukat dina 3 chanser att logga in!");
                Console.WriteLine("\nTryck valfri tangent för att komma vidare!");
                Console.ReadKey();
                runLogin = false;
            }
        }
        return indexRowCustomer;
    }
    
    //====================================================METHOD========================================================

    public static bool HeadMenu(int indexRowCustomer)
    {
        bool runMenu = true;

        while (runMenu)
        {
            Console.Clear();
            Console.WriteLine("Vad vill du göra?" +
                              "\n1. Se över konton och saldo" +
                              "\n2. Överföring mellan konton" +
                              "\n3. Ta ut Pengar" +
                              "\n4. Logga ut");

            bool testInput = Int32.TryParse(Console.ReadLine(), out int userChoice);

            while (!testInput)
            {
                Console.WriteLine("Felaktig input! Försök igen!");
            }

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Se över konton och saldon");
                    break;
                case 2:
                    Console.WriteLine("Överföring mellan konton");
                    break;
                case 3:
                    Console.WriteLine("Ta ut Pengar");
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Tack för denna gång! Ha en trevlig dag!\n");
                    runMenu = false; 
                    break;
            }
        }

        return runMenu;
    }
}
