//Jenny-Ann Hayward, SUT24

using System.ComponentModel;
using System.Globalization;
using System.Security.Principal;

namespace CashMashine_Lab4_SUT24;

//========================================================CLASS========================================================
//A program class with the main method for running the program
//=====================================================================================================================
class Program
{
    static void Main(string[] args)
    {
        //A 3D-array, storing personalnumer, pincode, account numbers and balances of five customers. 
        // each row representes one customer and display the following info:
        // { {personalnumer, pincode} {balance account 1, accountnr 1} {balance account 2, accountnr 2} {} (} {)}; 
        double[,,] bankAccounts =
        {
            { {1111111111, 1234}, {27030, 1241234}, {89232, 4352365}, {170212, 3446564}, {0, 0}, {0, 0}, {0, 0}},
            { {6412235334, 6334}, {41621, 3457843}, {0, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0}},
            { {9704304647, 2674}, {22011, 7524563}, {43000, 7543453}, {0, 0}, {0, 0}, {0, 0}, {0, 0}},
            { {5901174536, 8512}, {51000, 5674343}, {11232, 4563455}, {36021, 6545421}, {12421, 4453212}, {25000, 5312321}, {0, 0}},
            { {9310224650, 1452}, {29432, 1232321}, {42819, 1232312}, {8359, 1232121}, {23451, 4567842}, {0, 0}, {0, 0}}
        };
        
        //An array of account names as well of name of the account owner
        string[,] accountNames =
        {
            { "Johan Ottosson", "Lönekonto", "buffer", "pension", "", "", "" },
            { "Maria Isaksson", "Lönekonto", "", "", "", "", "" },
            { "Jörgen Persson", "Lönekonto", "pension", "", "", "", ""},
            { "Ann-Charlotte Svensson", "Lönekonto", "semesterspar", "bröllop", "pension", "ny bil", "" },
            { "Ismail Mohamed", "Lönekonto", "spar", "syjuntan", "sparTillBarnen", "", "" }
        };
        
        //declare ta variable for storing the customerIndex in the arryes bankaccountsand accountNames. This is 
        //corresponds to the index number on of the first dimension in respective array. 
        int customerIndex;
        bool runApp = true;
        
        while (runApp)
        {
            Console.WriteLine("Välkommen till din internetbank\n\n" +
                              "[1] Logga in\n" +
                              "[2] Stäng ner"); 

            switch (Console.ReadLine())
            {
                case "1":
                    customerIndex = MyMethods.LogIn(bankAccounts);
                    //if customerIndex is -1, the user has not been able to login. 
                    if (customerIndex == -1)
                    {
                        runApp = false;
                    }
                    else
                    {
                        bool stayLoggedIn = MyMethods.HeadMenu(customerIndex, bankAccounts, accountNames);
                    }
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
    /// A method for my use - to get a quick overview of customers, balances and accounts statuses. 
    /// </summary>
    /// <param name="customers"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <param name="startbalances"></param>
    /// <returns></returns>
    public static void WriteCustomersAndAccounts(double[,,] bankAccounts, string[,] 
        accountNames)
    {
        int indexbalance = 0;
        
        for (int i = 0; i < bankAccounts.GetLength(0); i++)
        {
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine($"Personnummer: {bankAccounts[i, 0, 0]}");
            Console.WriteLine($"Lösenord:{bankAccounts[i, 0, 1]}");

            for (int j = 0; j < bankAccounts.GetLength(1); j++)
            {

                if (j > 0 && accountNames[i, j].Length > 0)
                {
                    indexbalance++;
                    Console.Write($"{accountNames[i, j]}: {bankAccounts[i, j, 0]} sek       ");
                }
            }
            Console.WriteLine();
        }
    }
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for user login
    /// </summary>
    public static int LogIn(double[,,] bankAccounts)
    {
        //need a variable that can hold and return the personalnumber if login succeed. 
        int customerIndex = 0; 
        bool runLogin = true;
        bool matchLogin = false;
        int numberTried = 0;

        while (runLogin)
        { 
            //need variable to store the user input and one to use for running a do...while loop that keeps on running 
            //until the personal number is only in digits and of the correct length. 
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
                    Console.WriteLine("Felaktigt format på ditt personnummer!\n\nTryck valfri tangent för att " +
                                      "försöka igen");
                    Console.ReadKey();
                }
            } while (!correctPersNr);
            
            Console.Clear();
            long pinCode = AskForPinCode();
            
            for (int i = 0; i < bankAccounts.GetLength(0); i++)
            {
                if ((personNr == bankAccounts[i, 0, 0]) && (pinCode == bankAccounts[i, 0, 1]))
                {
                    customerIndex = i;
                    runLogin = false;
                    break;
                }
            }
            numberTried++;
            //runLogin true means login did not succeed. Password and personal nr did not match
            if (runLogin)
            {
                Console.WriteLine("\nDitt personnummer eller pinkod stämmer inte");
                Console.WriteLine("\nTryck valfri tangent för att komma vidare!");
                Console.ReadKey();
            }

            if (numberTried == 3)
            {
                Console.Clear();
                Console.WriteLine("Du har dessvärre förbrukat dina 3 chanser att logga in!" +
                                  "\n\nDu behöver starta om din internetbank för att logga in på nytt!" +
                                  "\n\nHa en fin dag!");
                customerIndex = -1;
                runLogin = false;
            }
        }
        return customerIndex;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for asking for password. Used in login-method as well asd the Withdraw-method. 
    /// </summary>
    /// <returns></returns>
    public static long AskForPinCode()
    {
        long pinCode = 0;
        bool correctPinCode = false;

        do
        {
            Console.Write("Ange din pinkod, 4 siffror: ");
            string inputPassword = Console.ReadLine();
            if ((inputPassword.Length == 4) && Int64.TryParse(inputPassword, out pinCode))
            {
                correctPinCode = true;
            }
            else
            {
                Console.WriteLine("Felaktigt format på pinkoden!\n\nTryck valfri tangent för att försöka igen");
                Console.ReadKey();
            }
        } while (!correctPinCode);

        return pinCode;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A menthod for running the head menu of user options once logged in. 
    /// </summary>
    /// <param name="indexRowCustomer"></param>
    /// <returns></returns>
    public static bool HeadMenu(int customerIndex, double[,,] bankAccounts, string[,] accountNames)
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
                    Console.WriteLine("KONTON OCH SALDON");
                    Console.WriteLine("-------------------------------");
                    AccountsAndBalance(customerIndex, bankAccounts, accountNames);
                    Console.WriteLine("\nKlicka enter för att komma till huvudmenyn!");
                    Console.ReadKey();
                    break;
                case 2:
                    Console.WriteLine("ÖVERFÖRING");
                    Console.WriteLine("-------------------------------");
                    bankAccounts = MoneyTransfer(customerIndex, bankAccounts, accountNames);
                    break;
                case 3:
                    Console.WriteLine("UTTAG");
                    Console.WriteLine("-------------------------------");
                    bankAccounts = MoneyWithraw(customerIndex, bankAccounts, accountNames);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Tack för denna gång! Ha en trevlig dag!\n");
                    runMenu = false;
                    break;
                default:
                    Console.WriteLine("Du måste ange en siffra mellan 1 och 4! Tryck på valfri tangent för att " +
                                      "försöka igen!");
                    Console.ReadKey();
                    break;
            }
        }
        return runMenu;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// Writing the accounts and balances 
    /// </summary>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    public static void AccountsAndBalance(int customerIndex, double[,,] bankAccounts, string[,] accountNames)
    {
        Console.Clear();
        Console.WriteLine($"{accountNames[customerIndex, 0]}, dina konton ser ut som följer:\n");
        
        //set i start at 1 since the personalnumber is on index 0, and i now want the accounts
        for (int i = 1; i < bankAccounts.GetLength(1); i++)
        {
            if (accountNames[customerIndex, i].Length != 0)
            {
                double balance = bankAccounts[customerIndex, i, 0];  
                Console.WriteLine($"[{i}] {accountNames[customerIndex, i]}.        " +
                                  $"saldo: {balance.ToString("C3", CultureInfo.CurrentCulture)}");
            }
        }
    }
    //====================================================METHOD========================================================
    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <returns></returns>

    public static double[,,] MoneyTransfer(int customerIndex, double[,,] bankAccounts, string[,] accountNames)
    {
        //Variables that will be used and reused in the man loops below. 
        string choiceFrom = "";
        string choiceTo = "";
        int accountIndexFrom = 0;
        int accountIndexTo = 0;
        
        //need a bool control the following loop that runs until we have a valid account to transfer from and one
        //to transfer to
        bool correctaccounts = false;

        while (!correctaccounts)
        {
            bool correctAccountFrom = false;

            while (!correctAccountFrom)
            {
                AccountsAndBalance(customerIndex, bankAccounts, accountNames);
                
                Console.Write("\nAnge det konto som du vill göra en överföring från: ");
                choiceFrom = Console.ReadLine().ToUpper();
                for (int i = 0; i < accountNames.GetLength(1); i++)
                {
                    if (accountNames[customerIndex, i].ToUpper() == choiceFrom)
                    {
                        accountIndexFrom = i;
                        correctAccountFrom = true;
                    }
                }
                if (!correctAccountFrom)
                {
                    Console.WriteLine("Felaktig inmatning! Tryck enter för att försöka igen!");
                    Console.ReadKey();
                }
            }

            bool correctAccountTo = false;

            while (!correctAccountTo)
            {
                Console.Write("\nAnge det konto som du vill göra en överföring till: ");
                choiceTo = Console.ReadLine().ToUpper();
                for (int i = 0; i < accountNames.GetLength(1); i++)
                {
                    if (accountNames[customerIndex, i].ToUpper() == choiceTo)
                    {
                        accountIndexTo = i;
                        correctAccountTo = true;
                    }
                }
                if (!correctAccountTo)
                {
                    Console.WriteLine("\nFelaktig inmatning! Tryck enter för att försöka igen!");
                    Console.ReadKey();
                }
            }
            Console.Clear();
            Console.WriteLine($"Du har angett att du vill göra en överföring" +
                              $"\n\nfrån" +
                              $"\n{choiceFrom}  -  saldo: {bankAccounts[customerIndex, accountIndexFrom, 0].ToString(
                                  "C3", CultureInfo.CurrentCulture)} " +
                              $"\n\ntill" +
                              $"\n{choiceTo}  -  saldo: {bankAccounts[customerIndex, accountIndexTo, 0].ToString(
                                  "C3", CultureInfo.CurrentCulture)}");
            Console.Write("\nStämmer detta? (ja/nej): ");
            if (Console.ReadLine().ToLower() == "ja")
            {
                Console.WriteLine();
                correctaccounts = true;
            }
            else
            {
                Console.WriteLine("\nTryck enter för att försöka igen!");
                Console.ReadKey();
                Console.Clear();
            }
        }
        double amountTransfer = AmountToHandle(bankAccounts, customerIndex, accountNames, accountIndexFrom, 
            "föra över");

        bankAccounts[customerIndex, accountIndexFrom, 0] -= amountTransfer;
        bankAccounts[customerIndex, accountIndexTo, 0] += amountTransfer;
        
        Console.Clear();
        Console.WriteLine($"Överföring lyckades! " +
                          $"\n{choiceFrom}    Nytt saldo: " +
                          $"{bankAccounts[customerIndex, accountIndexFrom, 0].ToString("C3", 
                              CultureInfo.CurrentCulture)})" +
                          $"\n{choiceTo}      Nytt saldo: {bankAccounts[customerIndex, accountIndexTo, 0].ToString(
                              "C3", CultureInfo.CurrentCulture)}) >");
        
        Console.WriteLine("\nTryck enter för att fortsätta");
        Console.ReadKey();
        return bankAccounts;
    }
    
    //====================================================METHOD========================================================
    
    public static double AmountToHandle(double[,,] bankAccounts, int customerIndex,  string[,] accountNames, 
        int accountIndexFrom, string action)
    {
        double amountToHandle = 0;

        bool correctAmount = false;

        while (!correctAmount)
        {
            bool amountWithinBalance = false;
            bool answerCheck = false;

            while (!answerCheck || !amountWithinBalance)
            {
                Console.Write($"Ange hur mycket du vill {action}: ");
                answerCheck = Double.TryParse(Console.ReadLine(), out amountToHandle);

                if (amountToHandle <= bankAccounts[customerIndex, accountIndexFrom, 0] &&
                    amountToHandle > 0)
                {
                    amountWithinBalance = true;
                }
                else if (amountToHandle > bankAccounts[customerIndex, accountIndexFrom, 0] || amountToHandle <= 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Nuvarande saldo på <" +
                                      $"{accountNames[customerIndex, accountIndexFrom]}> är " +
                                      $"{bankAccounts[customerIndex, accountIndexFrom, 0].ToString(
                                          "C3", CultureInfo.CurrentCulture)}" +
                                      $"\n\nDen summa du anger måste vara över 0 och kan inte överstiga nuvarande " +
                                      $"saldo.");
                    Console.WriteLine("\nTryck enter för att försöka igen!\n");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nFelaktigt val! Tryck enter för att försöka igen!\n");
                    Console.ReadKey();
                }
            }

            Console.WriteLine($"\nDu har angett  {amountToHandle.ToString("C3", CultureInfo.CurrentCulture)}");
            Console.Write("\nStämmer detta? [ja/nej] ");
            string answer = Console.ReadLine().ToLower();

            if (answer == "ja")
            {
                correctAmount = true;
            }
            else if (answer == "nej")
            {
                Console.WriteLine("\nOkey! Var god och försök igen!");
            }
            else
            {
                Console.WriteLine("\nFelaktigt svar! Försök igen!");
            }
        }

        return amountToHandle;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for withdraw money from bank accounts
    /// </summary>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <returns></returns>
    public static double[,,] MoneyWithraw(int customerIndex, double[,,] bankAccounts, string[,] accountNames)
    {
        //Variables that will be used and reused in the man loops below. 
        string choiceFrom = "";
        int accountIndexFrom = 0;
        
        //need a bool control the following loop that runs until we have a valid account to withraw from.
        bool correctAccountFrom = false;

        while (!correctAccountFrom)
        {
            AccountsAndBalance(customerIndex, bankAccounts, accountNames);

            Console.Write("\nAnge det konto som du vill göra ett uttag från: ");
            choiceFrom = Console.ReadLine().ToUpper();
            for (int i = 0; i < accountNames.GetLength(1); i++)
            {
                if (accountNames[customerIndex, i].ToUpper() == choiceFrom)
                {
                    accountIndexFrom = i;
                    correctAccountFrom = true;
                }
            }
            if (!correctAccountFrom)
            {
                Console.WriteLine("Felaktig inmatning! Tryck enter för att försöka igen!");
                Console.ReadKey();
            }
        }
        
        double amountToWithraw = AmountToHandle(bankAccounts, customerIndex, accountNames, accountIndexFrom, 
            "ta ut");
        
        Console.WriteLine("För att få göra uttag behöver du ange godkänd pinkkod. Tryck enter för att ange pinkod\n");
        Console.ReadKey();
        Console.Clear();

        bool runPinLoop = true;
        bool correctpin = false;
        int numberTried = 0;
        

        while (runPinLoop)
        {
            long pin = AskForPinCode();
            if (pin == bankAccounts[customerIndex, 0, 1])
            {
                correctpin = true; 
                runPinLoop = false; 
            }
            else
            {
                Console.WriteLine("Fel pinkod! Tryck enter för att försöka igen!");
                Console.ReadKey();
            }
            numberTried++;
            if (!correctpin && numberTried == 3)
            {
                Console.WriteLine("Du har dessvärre angett fel, 3 gånger!" +
                                  "\n\nTryck enter för att komma tillbaka till huvudmenyn!");
                Console.ReadKey();
                runPinLoop = false;
            }
        }

        if (!correctpin)
        {
            HeadMenu(customerIndex, bankAccounts, accountNames);
        }
        
        bankAccounts[customerIndex, accountIndexFrom, 0] -= amountToWithraw;

        Console.Clear();
        Console.WriteLine($"Uttaget lyckades! " +
                          $"\n{choiceFrom}    Nytt saldo: " +
                          $"{bankAccounts[customerIndex, accountIndexFrom, 0].ToString("C3",
                              CultureInfo.CurrentCulture)})");

        Console.WriteLine("\nTryck enter för att fortsätta");
        Console.ReadKey();
        
        return bankAccounts;
    }
    
    //====================================================METHOD========================================================

    public static void OpenAccounts(double[,,] bankAcounts, string []accountNames)
    {
        
    }
}