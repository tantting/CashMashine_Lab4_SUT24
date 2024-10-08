//Jenny-Ann Hayward, SUT24

using System.Globalization;

namespace CashMashine_Lab4_SUT24;

//========================================================CLASS========================================================
//A program class with the main method for running the program
//=====================================================================================================================

class Program
{
    static string[,] customers =
    {
        { "Johan Ottosson", "1111111111", "1111" },
        { "Maria Isaksson", "2222222222", "2222" },
        { "Jörgen Persson", "3333333333", "3333" },
        { "Ann-Charlotte Svensson", "4444444444", "4444" }, 
        { "Ismail Mohamed", "5555555555", "5555" }
    };
    
    //A jagged 2D-array, storing account numbers and balances of five customers. 
    // each row representes one customer and display info:
    // { {balance account 1, accountnr 1} {balance account 2, accountnr 2} }; 
    static decimal[][,] bankAccounts = new decimal[5][,];
    
    //A jagged array of account names
    static string[][] accountNames = new string[5][];
    static void Main(string[] args)
    {
        
        bankAccounts[0] = new decimal[,] { { 1241234, 27030 }, { 4352365, 89232 }, { 3446564, 170212 } };
        bankAccounts[1] = new decimal[,] { { 3457843, 41621 } };
        bankAccounts[2] = new decimal[,] { { 7524563, 22011 }, { 7543453, 43000 } };
        bankAccounts[3] = new decimal[,]
            { { 5674343, 51000 }, { 4563455, 11232 }, { 6545421, 36021 }, { 4453212, 12421 }, { 5312321, 25000 } };
        bankAccounts[4] = new decimal[,] { {1232321, 29432}, {1232312, 42819}, {1232121, 8359}, {4567842, 23451} };
        
        accountNames[0] = new string[] { "Lönekonto", "buffer", "pension" };
        accountNames[1] = new string[] { "Lönekonto" };
        accountNames[2] = new string[] { "Lönekonto", "pension" };
        accountNames[3] = new string[] { "Lönekonto", "semesterspar", "bröllop", "pension", "ny bil"};
        accountNames[4] = new string[] { "Lönekonto", "spar", "syjuntan", "sparTillBarnen" };
        
        //declare ta variable for storing the customerIndex in the arryes bankaccountsand accountNames. This 
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
                    customerIndex = MyMethods.LogIn(customers);
                    //if customerIndex is -1, the user has not been able to login. 
                    if (customerIndex == -1)
                    {
                        runApp = false;
                    }
                    else
                    {
                        MyMethods.HeadMenu(customers, customerIndex, bankAccounts, accountNames);
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
    
    /// <summary>
    /// A method for user login
    /// </summary>
    /// <param name="customerArray"></param>
    /// <returns></returns>
    public static int LogIn(string[,] customers)
    {
        //need a variable that can hold and return the personalnumber if login succeed. 
        int customerIndex = 0; 
        bool runLogin = true;
        int numberTried = 0;

        while (runLogin)
        { 
            //need variable to store the user input and one to use for running a do...while loop that keeps on running 
            //until the personal number is only in digits and of the correct length. 
            string personNr = "";
            bool correctPersNr = false;
        
            do
            {
                Console.Clear();
                Console.Write("Ange ditt personnummer (YYMMDDXXXX): ");
                string inputPersNr = Console.ReadLine();
                if ((inputPersNr.Length == 10) && (Int64.TryParse(inputPersNr, out long personNrLong)))
                {
                    personNr = inputPersNr;
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
            string pinCode = AskForPinCode();
            
            for (int i = 0; i < customers.GetLength(0); i++)
            {
                if ((personNr == customers[i, 1]) && (pinCode == customers[i, 2]))
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
        //Console.WriteLine($"customerIndex är {customerIndex}");
        //Console.ReadKey();
        return customerIndex;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for asking for password. Used in login-method as well asd the Withdraw-method. 
    /// </summary>
    /// <returns></returns>
    public static string AskForPinCode()
    {
        string pinCode = "";
        bool correctPinCode = false;

        do
        {
            Console.Write("Ange din pinkod, 4 siffror: ");
            string inputPinCode = Console.ReadLine();
            if ((inputPinCode.Length == 4) && Int32.TryParse(inputPinCode, out int pinInt))
            {
                pinCode = inputPinCode; 
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
    /// <param name="customerArray"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <returns></returns>
    public static bool HeadMenu(string[,] customers, int customerIndex, decimal[][,] bankAccounts,
        string[][] accountNames)
    {
        bool runMenu = true;
        
        while (runMenu)
        {
            Console.Clear();
            Console.WriteLine($"Hej {customers[customerIndex, 0]}!\n");
            Console.WriteLine("Vad vill du göra?" +
                              "\n1. Se över konton och saldo" +
                              "\n2. Överföring mellan konton" +
                              "\n3. Ta ut Pengar" +
                              "\n4. Öppna ett nytt konto" +
                              "\n5. Logga ut");

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
                    AccountsAndBalance(customers, customerIndex, bankAccounts, accountNames);
                    Console.WriteLine("\nKlicka enter för att komma till huvudmenyn!");
                    Console.ReadKey();
                    break;
                case 2:
                    Console.WriteLine("ÖVERFÖRING");
                    Console.WriteLine("-------------------------------");
                    bankAccounts = MoneyTransfer(customers, customerIndex, bankAccounts, accountNames);
                    break;
                case 3:
                    Console.WriteLine("UTTAG");
                    Console.WriteLine("-------------------------------");
                    MoneyWithraw(customers, customerIndex, bankAccounts, accountNames);
                    break;
                case 4:
                    
                    break;
                case 5:
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
    /// <param name="customerArray"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    public static void AccountsAndBalance(string[,] customers, int customerIndex, decimal[][,] bankAccounts, 
        string[][] accountNames)
    {
        Console.Clear();
        Console.WriteLine($"{customers[customerIndex, 0]}, dina konton ser ut som följer:\n");
       
        for (int i = 0; i < bankAccounts[customerIndex].GetLength(0); i++)
        {
            decimal accountNr = bankAccounts[customerIndex][i, 0];
            decimal balance = bankAccounts[customerIndex][i, 1]; 
                Console.WriteLine($"[{i+1}]   {accountNames[customerIndex][i], -10} ({accountNr})          " +
                                  $"saldo: {balance.ToString("C3", CultureInfo.CurrentCulture), -10}");
        }
    }
    //====================================================METHOD========================================================
    /// <summary>
    /// 
    /// </summary>
    /// <param name="customerArray"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <returns></returns>
    
    public static decimal[][,] MoneyTransfer(string[,] customers, int customerIndex, decimal[][,] bankAccounts, 
        string[][] accountNames)
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
                AccountsAndBalance(customers, customerIndex, bankAccounts, accountNames);
                
                Console.Write("\nAnge det konto som du vill göra en överföring från: ");
                choiceFrom = Console.ReadLine().ToUpper();
                for (int i = 0; i < accountNames[customerIndex].Length; i++)
                {
                    if (accountNames[customerIndex][i].ToUpper() == choiceFrom)
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
                for (int i = 0; i < accountNames[customerIndex].Length; i++)
                {
                    if (accountNames[customerIndex][i].ToUpper() == choiceTo)
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
            //Since balance 
            Console.WriteLine($"Du har angett att du vill göra en överföring" +
                              $"\n\nfrån\n");
            const string format = "{0,-20} {1,30}";
            Console.WriteLine(string.Format(format, $"{choiceFrom} ({bankAccounts[customerIndex][accountIndexFrom, 0]})", 
                              $"saldo: {bankAccounts[customerIndex][accountIndexFrom, 1].ToString(
                                  "C3", CultureInfo.CurrentCulture)}"));
            Console.WriteLine("\ntill\n");
            Console.WriteLine(string.Format(format, $"{choiceTo} ({bankAccounts[customerIndex][accountIndexTo, 0]})", $"saldo: {bankAccounts[customerIndex][accountIndexTo, 1].ToString(
                "C3", CultureInfo.CurrentCulture)}"));
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
        decimal amountTransfer = AmountToHandle(bankAccounts, customerIndex, accountNames, accountIndexFrom, 
            "föra över");
    
        bankAccounts[customerIndex][accountIndexFrom, 1] -= amountTransfer;
        bankAccounts[customerIndex][accountIndexTo, 1] += amountTransfer;
        
        Console.Clear();
        Console.WriteLine($"Överföring lyckades! " +
                          $"\n{choiceFrom}    Nytt saldo: " +
                          $"{bankAccounts[customerIndex][accountIndexFrom, 1].ToString("C3", 
                              CultureInfo.CurrentCulture)})" +
                          $"\n{choiceTo}      Nytt saldo: {bankAccounts[customerIndex][accountIndexTo, 1].ToString(
                              "C3", CultureInfo.CurrentCulture)}) >");
        
        Console.WriteLine("\nTryck enter för att fortsätta");
        Console.ReadKey();
        return bankAccounts;
    }
    
    //====================================================METHOD========================================================
    
    public static decimal AmountToHandle(decimal[][,] bankAccounts, int customerIndex,  string[][] accountNames, 
        int accountIndexFrom, string action)
    {
        decimal amountToHandle = 0;
    
        bool correctAmount = false;
    
        while (!correctAmount)
        {
            bool amountWithinBalance = false;
            bool answerCheck = false;
    
            while (!answerCheck || !amountWithinBalance)
            {
                Console.Write($"Ange hur mycket du vill {action}: ");
                answerCheck = Decimal.TryParse(Console.ReadLine(), out amountToHandle);
    
                if (amountToHandle <= bankAccounts[customerIndex][accountIndexFrom, 1] &&
                    amountToHandle > 0)
                {
                    amountWithinBalance = true;
                }
                else if (amountToHandle > bankAccounts[customerIndex][accountIndexFrom, 1] || amountToHandle <= 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Nuvarande saldo på <" +
                                      $"{accountNames[customerIndex][accountIndexFrom]}> är " +
                                      $"{bankAccounts[customerIndex][accountIndexFrom, 1].ToString(
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
    /// <param name="customerArray"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <returns></returns>
    public static void MoneyWithraw(string[,] customers, int customerIndex, decimal[][,] bankAccounts, 
        string[][] accountNames)
    {
        //Variables that will be used and reused in the man loops below. 
        string choiceFrom = "";
        int accountIndexFrom = 0;
        
        //need a bool control the following loop that runs until we have a valid account to withraw from.
        bool correctAccountFrom = false;
    
        while (!correctAccountFrom)
        {
            AccountsAndBalance(customers, customerIndex, bankAccounts, accountNames);
    
            Console.Write("\nAnge det konto som du vill göra ett uttag från: ");
            choiceFrom = Console.ReadLine().ToUpper();
            for (int i = 0; i < accountNames[customerIndex].Length; i++)
            {
                if (accountNames[customerIndex][i].ToUpper() == choiceFrom)
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
        
        decimal amountToWithraw = AmountToHandle(bankAccounts, customerIndex, accountNames, accountIndexFrom, 
            "ta ut");
        
        Console.WriteLine("För att få göra uttag behöver du ange godkänd pinkkod. Tryck enter för att ange pinkod\n");
        Console.ReadKey();
        Console.Clear();
    
        bool runPinLoop = true;
        bool correctpin = false;
        int numberTried = 0;
        
    
        while (runPinLoop)
        {
            string pin = AskForPinCode();
            if (pin == customers[customerIndex, 2])
            {
                correctpin = true; 
                runPinLoop = false; 
                
                bankAccounts[customerIndex][accountIndexFrom, 1] -= amountToWithraw;
    
                Console.Clear();
                Console.WriteLine($"Uttaget lyckades! " +
                                  $"\n{choiceFrom}    Nytt saldo: " +
                                  $"{bankAccounts[customerIndex][accountIndexFrom, 1].ToString("C3",
                                      CultureInfo.CurrentCulture)})");
            }
            else
            {
                Console.WriteLine("Fel pinkod! Tryck enter för att försöka igen!");
                Console.ReadKey();
            }
            numberTried++;
            if (!correctpin && numberTried == 3)
            {
                Console.WriteLine("Du har dessvärre angett fel pinkod, 3 gånger!" +
                                  "\n\nTryck enter för att komma tillbaka till huvudmenyn!");
                Console.ReadKey();
                runPinLoop = false;
            }
        }
    }
    
    //====================================================METHOD========================================================

    public static void OpenAccounts(double[,,] bankAcounts)
    {
        
    }
}