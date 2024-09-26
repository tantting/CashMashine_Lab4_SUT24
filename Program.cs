//Jenny-Ann Hayward, SUT24

namespace CashMashine_Lab4_SUT24;

//========================================================CLASS========================================================
//A program class with the main method for running the program
//=====================================================================================================================
class Program
{
    static void Main(string[] args)
    {
        int customerIndex; 
        //Initiate a multi dim array for storing user nr, id(personal nummber) and passsword of the five customers
        //need to be of type long to store the personal numbers
        long[,] customers = { {1, 8204084647, 1234}, {2, 6412235334, 6334}, {3, 9704304647, 2674}, 
            {4, 5901174536, 8512}, {5, 9310224650, 1452} };
        //An array of arrays is declared for storing variables for type double. Intended for storing bank
        //accounts later on. It will store bank accounts for 5 users. 
        double[,,] bankAccounts = new double [customers.GetLength(0), 11, 2];
        

        for (int i = 0; i < customers.GetLength(0); i++)
        {
            for (int j = 0; j < bankAccounts.GetLength(1); j++)
            {
                bankAccounts[i, 0, 0] = customers[i, 1];
                
                if (j > 0)
                {
                    bankAccounts[i, j, 0] = j;
                }
                //Console.Write($"konto: {bankAccounts[i, j, 0]},   ");
            }
            //Console.WriteLine();
        }
        
        
        bool runApp = true;
        while (runApp)
        {
            Console.WriteLine("Välkommen till din internetbank\n\n" +
                              "[1] Logga in\n" +
                              "[2] Stäng ner"); 

            switch (Console.ReadLine())
            {
                case "1":
                    customerIndex = MyMethods.LogIn(customers);
                    bool stayLoggedIn = MyMethods.HeadMenu(customerIndex, bankAccounts);
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
        //need a variable that can hold and return the personalnumber if login succeed. 
        int customerIndex = 0; 
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
                    matchLogin = true;
                    return customerIndex;
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
        return 0;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A menthod for running the head menu of user options once logged in. 
    /// </summary>
    /// <param name="indexRowCustomer"></param>
    /// <returns></returns>
    public static bool HeadMenu(int customerIndex, double[,,] bankAccounts)
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

            int userChoice = 0;

            while (!Int32.TryParse(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("Felaktig input! Försök igen!");
            }

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Se över konton och saldon");
                    AccountsAndBalance(customerIndex, bankAccounts);
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
                default:
                    Console.WriteLine("Du måste ange en siffra mellan 1 och 4! Tryck på valfri tangent för att " +
                                      "försöka igen!");
                    break;
            }

            Console.ReadKey();
        }
        return runMenu;
    }
    
    //====================================================METHOD========================================================

    public static void AccountsAndBalance(int customerIndex, double[,,] bankAccounts)
    {
        Console.WriteLine("Du har följande aktiva konton:");
        
        //set i start at 1 since the personalnumber is on index 0, and i now want the accounts
        for (int i = 1; i < bankAccounts.GetLength(1); i++)
        {
            Console.WriteLine($"konto nr {i}.        saldo: {bankAccounts[customerIndex, i, 0]}");
        }

        Console.ReadKey();
    }
}
