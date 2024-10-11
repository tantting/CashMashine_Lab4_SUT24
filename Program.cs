//Jenny-Ann Hayward, SUT24

using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace CashMashine_Lab4_SUT24;

//========================================================CLASS========================================================
//A program class with the main method for running the program. 

//This program is an internetbank with 5 customers.As a starting point they all have different amount of accounts
//=====================================================================================================================

class Program
{
    //Store the name[index 0], personal number [index 1] and password [index 2] of all customers in a 2D array.
    static string[,] customers =
    {
        { "Johan Ottosson", "8204084647", "1234" },
        { "Ismail Mohamed", "9612304648", "4321" },
        { "Jörgen Persson", "2107205323", "3333" },
        { "Ann-Charlotte Svensson", "6906254673", "4444" }, 
        { "Britta Isaksson", "5611236434", "5555" }
    };
    
    //A jagged 2D-array, storing account numbers and balances of five customers.Index of the outer array corresponds to  
    // the index in the customer-array.
    static decimal[][,] bankAccounts = new decimal[5][,];
    
    //A jagged array of account names, indexes are related, eg bankAccounts[i][j,] and accountNames[i][j].
    static string[][] accountNames = new string[5][];
    
    static void Main(string[] args)
    {
        // Intitiate the jagged arrays declared above. Each row represents one customer and display the following
        // info in the bank account array:
        // { { accountnr 1, balance account 1 } {accountnr 2, balance account 2}  etc }; 
        bankAccounts[0] = new [,] { { 1241234, 27030.50m }, { 4352365, 89232.73m }, { 3446564, 170212.20m } };
        bankAccounts[1] = new [,] { { 3457844, 41620.70m } };
        bankAccounts[2] = new [,] { { 7524563, 22011m }, { 7543453, 43000.99m } };
        bankAccounts[3] = new [,] { { 5674343, 51000.35m }, { 4563455, 11232.50m }, { 6545421, 36021.10m }, 
            { 4453212, 12421.83m }, { 5312321, 25000.00m } };
        bankAccounts[4] = new [,] { {1232321, 29432.20m}, {1232312, 42819.60m}, {1232121, 8359.05m}, 
            {4567842, 23451.50m} };
        
        accountNames[0] = [ "Lönekonto", "buffer", "pension" ];
        accountNames[1] = [ "Lönekonto" ];
        accountNames[2] = [ "Lönekonto", "pension" ];
        accountNames[3] = [ "Lönekonto", "semesterspar", "bröllop", "pension", "ny bil" ];
        accountNames[4] = [ "Lönekonto", "spar", "syjuntan", "sparTillBarnen" ];
        
        //declare a variable for storing the customerIndex for the arryes customers,  bankaccounts and accountNames. 
        int customerIndex;
        
        bool runApp = true;
        
        while (runApp)
        {
            Console.Clear();
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
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for user login
    /// </summary>
    /// <param name="customers"></param>
    /// <returns></returns>
    public static int LogIn(string[,] customers)
    {
        //need a golden variable that hold and return the customer index if login succeed. 
        int customerIndex = 0; 
        int numberTried = 0;
        bool runLogin = true;

        while (runLogin)
        { 
            //use a bool for running a do...while loop that keeps on running until the personal number is only in
            //digits and of the correct length. 
            string personNr = "";
            bool correctPersNr = false;
        
            do
            {
                Console.Clear();
                Console.Write("Ange ditt personnummer (YYMMDDXXXX): ");
                string inputPersNr = Console.ReadLine();
                //Test that the user enters numbers and of the right length (personalNrLong is not used other contexts).
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
    /// A method for asking for password. Checks that the user input 4 digits. Does not check if the password is
    /// correct. 
    /// </summary>
    /// <returns>the entered pin-code if entered in the correct form</returns>
    public static string AskForPinCode()
    {
        string pinCode = "";
        bool correctPinCode = false;

        do
        {
            Console.Write("Ange din pinkod, 4 siffror: ");
            string inputPinCode = Console.ReadLine();
            //even if I do not need to parse the input, I use TryParse to check that the user input 4 digits. 
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
    /// A menthod for running the "head"/main menu of actions. 
    /// </summary>
    /// <param name="customers"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    /// <returns></returns>
    public static void HeadMenu(string[,] customers, int customerIndex, decimal[][,] bankAccounts,
        string[][] accountNames)
    {
        bool runMenu = true;
        
        while (runMenu)
        {
            Console.Clear();
            Console.WriteLine($"Hej {customers[customerIndex, 0]}!\n");
            Console.WriteLine("Vad vill du göra?" +
                              "\n[1] Se över konton och saldo" +
                              "\n[2] Överföring mellan konton" +
                              "\n[3] Ta ut Pengar" +
                              "\n[4] Öppna ett nytt konto" +
                              "\n[5] Logga ut");

            int userChoice = 0;

            while (!Int32.TryParse(Console.ReadLine(), out userChoice))
            {
                Console.WriteLine("Felaktig input! Försök igen! Ange en siffra för aktuellt menyval");
            }

            switch (userChoice)
            {
                case 1:
                    AccountsAndBalance(customers, customerIndex, bankAccounts, accountNames);
                    break;
                case 2:
                    MoneyTransfer(customers, customerIndex, bankAccounts, accountNames);
                    break;
                case 3:
                    MoneyWithraw(customers, customerIndex, bankAccounts, accountNames);
                    break;
                case 4:
                    OpenAccounts(bankAccounts, customerIndex, accountNames);
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Tack för denna gång! Ha en trevlig dag!\n");
                    runMenu = false;
                    continue;
                default:
                    Console.WriteLine("Du måste ange en siffra mellan 1 och 4! Tryck på valfri tangent för att " +
                                      "försöka igen!");
                    Console.ReadKey();
                    break;
            }

            ConsoleKey key;
            do
            {
                Console.WriteLine("\nTryck [enter] för att komma till huvudmenyn");
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Enter);
        }
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// Writing the accounts and balances 
    /// </summary>
    /// <param name="customers"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    public static void AccountsAndBalance(string[,] customers, int customerIndex, decimal[][,] bankAccounts, 
        string[][] accountNames)
    {
        Console.Clear();
                    
        Console.WriteLine($"{customers[customerIndex, 0]}, dina konton ser ut som följer:\n");

        //The for-loop steps through all bank accounts and bank account names for customer at index 'customerIndex'
        //and print print out name, account number and balance at each step. 
        for (int i = 0; i < bankAccounts[customerIndex].GetLength(0); i++)
        {
            decimal accountNr = bankAccounts[customerIndex][i, 0];
            decimal balance = bankAccounts[customerIndex][i, 1];
            //use string.Format to adjust the text into columns and align the text to the left of these columns.  
            const string format = "{0,-5} {1,-30} {2, -20}";
            Console.WriteLine(format, $"[{i + 1}]", $"{accountNames[customerIndex][i]} ({accountNr})",
                $"saldo: {balance:C}");
        } 
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// A method for transfering money between accounts of the logged in customer. 
    /// </summary>
    /// <param name="customers"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    public static void MoneyTransfer(string[,] customers, int customerIndex, decimal[][,] bankAccounts,
        string[][] accountNames)
    {
        //Variables that will be used and reused in the man loops below. 
        string choiceFrom = "";
        string choiceTo = "";
        int accountIndexFrom = 0;
        int accountIndexTo = 0;
        //use a format variable again to format the print-outs in this method. 
        const string format = "{0,-20} {1,30}";

        //use bools to control the following loops that runs until we have a valid account to transfer from and one
        //to transfer to. One outer loop runs until the user confirms that he/she is content or enters quit. Inner loops
        //ask for inpt and validate that there are correcponding accounts. 
        bool correctaccounts = false;
        bool correctAccountFrom = false;
        bool correctAccountTo = false;
        bool quit = false; 


        while (!correctaccounts && !quit)
        {
            while (!correctAccountFrom)
            {
                //Start with a print-out of the accounts and balances for the customer
                AccountsAndBalance(customers, customerIndex, bankAccounts, accountNames);

                Console.Write(
                    "\nAnge det konto som du vill göra en överföring från! " +
                    "\n(skriv [quit] om du vill avbryta och återgå till huvudmenyn)" +
                    "\nKonto från: ");
                choiceFrom = Console.ReadLine().ToUpper();

                if (choiceFrom == "QUIT")
                {
                    //if user enters "quit" the loop breaks
                    quit = true; 
                    break; 
                }
                //The ValidAccounts compare the input choiceFrom to all the account names. Returns index of account
                //(dimension 0 for bankaccounts) or -1 if there is no match.
                accountIndexFrom = ValidAccount(accountNames, customerIndex, choiceFrom);

                if (accountIndexFrom != -1)
                {
                    correctAccountFrom = true;
                }
                else
                {
                    Console.WriteLine("Felaktig inmatning! Tryck enter för att försöka igen!");
                    Console.ReadKey();
                }
            }

            while (!correctAccountTo && !quit)
            {
                Console.Write("\nAnge det konto som du vill göra en överföring till! " +
                              "\n(skriv [quit] om du vill avbryta och återgå till huvudmenyn)" +
                              "\nKonto till: ");
                choiceTo = Console.ReadLine().ToUpper();

                if (choiceTo == "QUIT")
                {
                    //when quit is entered, we break the loop. 
                    quit = true; 
                    break;
                }

                //same procedure as for accountsFrom
                accountIndexTo = ValidAccount(accountNames, customerIndex, choiceTo);

                if (accountIndexTo != -1)
                {
                    correctAccountTo = true;
                }
                else
                {
                    Console.WriteLine("\nFelaktig inmatning! Tryck enter för att försöka igen!");
                    Console.ReadKey();
                }
            }

            if (correctAccountFrom && correctAccountTo)
            {
                Console.Clear();
                Console.WriteLine($"Du har angett att du vill göra en överföring" +
                                  $"\n\nfrån\n");

                Console.WriteLine(format, $"{choiceFrom} ({bankAccounts[customerIndex][accountIndexFrom, 0]})",
                    $"saldo: {bankAccounts[customerIndex][accountIndexFrom, 1]:C}");

                Console.WriteLine("\ntill\n");

                Console.WriteLine(format, $"{choiceTo} ({bankAccounts[customerIndex][accountIndexTo, 0]})",
                    $"saldo: {bankAccounts[customerIndex][accountIndexTo, 1]:C}");

                //When there are matching inputs for both account in and out, the program pose a control question to the
                //user. If yes, the correctaccounts bool is set to true and we can exit the "parent" while-loop. 
                Console.Write("\nStämmer detta? (ja/nej): ");
                if (Console.ReadLine().ToLower() == "ja")
                {
                    Console.WriteLine();
                    correctaccounts = true;
                }
                else
                {
                    //since the user realise he/she has entered wrong accounts on either - correctAccountsTo/From
                    //is changed again to false and the user gets to try again. 
                    correctAccountFrom = false;
                    correctAccountTo = false; 
                    Console.WriteLine("\nTryck enter för att försöka igen!");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        if (correctaccounts)
        {
            //Use a method for asking the user for the amounts to transfer.
            decimal amountTransfer = AmountToHandle(bankAccounts, customerIndex, accountNames, accountIndexFrom,
                "föra över");

            bankAccounts[customerIndex][accountIndexFrom, 1] -= amountTransfer;
            bankAccounts[customerIndex][accountIndexTo, 1] += amountTransfer;

            Console.Clear();
            Console.WriteLine($"Överföring lyckades!\n");
            Console.WriteLine(format, $"{choiceFrom}", $"Nytt saldo: {bankAccounts[customerIndex][accountIndexFrom,
                1]:C}");
            Console.WriteLine();
            Console.WriteLine(format, $"{choiceTo}", $"Nytt saldo: {bankAccounts[customerIndex][accountIndexTo, 1]:C}");
        }
    }

    //====================================================METHOD========================================================
    /// <summary>
    /// A method for validate that the account name the user has entered matches an actual account name in the
    /// accountName array. 
    /// </summary>
    /// <param name="accountNames"></param>
    /// <param name="customerIndex"></param>
    /// <param name="choice"></param>
    /// <returns>accountIndex</returns>
    public static int ValidAccount(string[][] accountNames, int customerIndex, string choice)
    {
        int accountIndex = -1;
        //Use a for-loop to step through all the account names of the actual customer to se if theres a match with the
        //account name the user has entered in an earlier step (choice).If match, i is returnd as account index. 
        for (int i = 0; i < accountNames[customerIndex].Length; i++)
        {
            if (accountNames[customerIndex][i].ToUpper() == choice)
            {
                accountIndex = i;
                return accountIndex;
            }
        }
        return accountIndex;
    }
    
    //====================================================METHOD========================================================
    /// <summary>
    /// Ask for amount to transfer of withraw and validate that the amount is larger than 0 and less than the actual
    /// balance of the account that money will be withrawn from. Returns and decimal amount.
    /// </summary>
    /// <param name="bankAccounts"></param>
    /// <param name="customerIndex"></param>
    /// <param name="accountNames"></param>
    /// <param name="accountIndexFrom"></param>
    /// <param name="action"></param>
    /// <returns>decimal amountToHandle</returns>
    public static decimal AmountToHandle(decimal[][,] bankAccounts, int customerIndex,  string[][] accountNames, 
        int accountIndexFrom, string action)
    {
        decimal amountToHandle = 0;
    
        bool askForAmountLoop = true;
        
        //run a loop until a valid amount to transfer or withraw from account has been entered by the user. 
        while (askForAmountLoop)
        {
            bool amountWithinBalance = false;
            bool answerCheck = false;
            
            //this while loop will run as long as the TryParse-method of the answers returns false (input not numbers)
            //or the amount is less than 0 or larger then the balance of the account. 
            while (/*!answerCheck || */!amountWithinBalance)
            {
                Console.Write($"Ange hur mycket du vill {action}!" +
                              $"\n(skriv [quit] om du vill avbryta och återgå till huvudmenyn)" +
                              $"\n\nSumma att {action}: ");
                string answerString = Console.ReadLine();

                if (answerString.ToLower() == "quit")
                {
                    return -1; 
                }
                
                answerCheck = Decimal.TryParse(answerString, out amountToHandle);
    
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
                                      $"{bankAccounts[customerIndex][accountIndexFrom, 1]:C}" +
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
            
            Console.Write($"\nDu har angett  {amountToHandle:C}      Stämmer detta? [ja/nej]: ");
            string answer = Console.ReadLine().ToLower();
    
            if (answer == "ja")
            {
                askForAmountLoop = false;
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
    /// <param name="customers"></param>
    /// <param name="customerIndex"></param>
    /// <param name="bankAccounts"></param>
    /// <param name="accountNames"></param>
    public static void MoneyWithraw(string[,] customers, int customerIndex, decimal[][,] bankAccounts, 
        string[][] accountNames)
    {
        //Variables that will be used and reused in the man loops below. 
        string choiceFrom = "";
        int accountIndexFrom = 0;
        
        //need a bool control the following loop that runs until we have a valid account to withraw from.
        bool correctAccountFrom = false;
        
        //the same procedure as in Moneytransfer, but here I only need to validate a "from"-account, 
        while (!correctAccountFrom)
        {
            AccountsAndBalance(customers, customerIndex, bankAccounts, accountNames);
    
            Console.Write("\nAnge det konto som du vill göra ett uttag från! " +
                          "\n([quit] om du istället önskar återgå till huvudmenyn):" +
                          "\n\nKonto från: ");
            choiceFrom = Console.ReadLine().ToUpper();

            if (choiceFrom == "QUIT")
            {
                break; 
            }

            accountIndexFrom = ValidAccount(accountNames, customerIndex, choiceFrom);
            
            if (accountIndexFrom != -1)
            {
                Console.Write($"\nDu har angett att du vill göra ett uttag från " +
                              $"{accountNames[customerIndex][accountIndexFrom]}" +
                              $"\nStämmer det [ja/nej]?: ");
                if (Console.ReadLine().ToLower() == "ja")
                {
                    correctAccountFrom = true; 
                }
                else
                {
                    Console.WriteLine("Ok! Tryck valfri för att komma vidare och lägga in ett konto på nytt!");
                }
            }
            else
            {
                Console.WriteLine("Felaktig inmatning! Tryck valfri tangent för att försöka igen!");
                Console.ReadKey();
            }
        }

        if (correctAccountFrom)
        {
            Console.WriteLine();
            decimal amountToWithraw = AmountToHandle(bankAccounts, customerIndex, accountNames, accountIndexFrom,
                "ta ut");

            Console.Clear();
            Console.WriteLine(
                "För att få göra uttag behöver vi bekräfta din identitet!\n");

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
                                      $"{bankAccounts[customerIndex][accountIndexFrom, 1]:C})");
                }
                else
                {
                    Console.WriteLine("Fel pinkod! Tryck valfri tangent för att försöka igen!");
                    Console.ReadKey();
                }

                numberTried++;
                if (!correctpin && numberTried == 3)
                {
                    Console.WriteLine("Du har dessvärre angett fel pinkod, 3 gånger!" +
                                      "\n\nTryck valfri för att komma tillbaka till huvudmenyn!");
                    Console.ReadKey();
                    runPinLoop = false;
                }
            }
        }
    }

    //====================================================METHOD========================================================
    /// <summary>
    /// A method for open a new account.
    /// </summary>
    /// <param name="bankAccounts"></param>
    /// <param name="customerIndex"></param>
    /// <param name="accountNames"></param>
    public static void OpenAccounts(decimal[][,] bankAccounts, int customerIndex, 
        string [][] accountNames)
    {
        Console.Clear();

        bool runLoop = true;

        while (runLoop)
        {
            Console.Write("Önskar du öppna ett nytt konto? [ja / nej]: ");
            
            string userChoice = Console.ReadLine();
            
            if (userChoice.ToLower() == "ja")
            {
                //new temporary arrays are created to bank acccounts account names
                decimal[][,] newBankAccounts = new decimal[bankAccounts.Length][,];
                string[][] newAccountNames = new string[accountNames.Length][];
                
                //new arrays for storing the actual bank accounts and names of accounts of the logged-in customer
                //The arrays are giving an additional element position than the original arrays by adding 1. 
                newBankAccounts[customerIndex] = new decimal[bankAccounts[customerIndex].GetLength(0) + 1, 2];
                newAccountNames[customerIndex] = new string[accountNames[customerIndex].Length + 1];
                
                //Polpulate the new array with info from corresponding position at the orignial arrays. The only
                //diff when done is an array that are the same as the origial one but with one empty position at the end. 
                for (int i = 0; i < bankAccounts[customerIndex].GetLength(0); i++)
                {
                    for (int j = 0; j < bankAccounts[customerIndex].GetLength(1); j++)
                    {
                        newBankAccounts[customerIndex][i, j] = bankAccounts[customerIndex][i, j];
                        newAccountNames[customerIndex][i] = accountNames[customerIndex][i];
                    }
                }
                //assing the value of the newarrays to the original array variables. 
                bankAccounts[customerIndex] = newBankAccounts[customerIndex];
                accountNames[customerIndex] = newAccountNames[customerIndex];
                
                //Use the random class to create a new bank account nr that is then assign to the new account nr position. 
                Random random = new Random();
                decimal newAccountNr = random.Next(999999, 10000000);
                bankAccounts[customerIndex][bankAccounts[customerIndex].GetLength(0) - 1, 0] = newAccountNr;
                Console.WriteLine($"Nytt kontonummer: {newAccountNr}");
                
                Console.Write("\nVad är namnet på ditt nya konto?:");
                accountNames[customerIndex][accountNames[customerIndex].Length - 1] = Console.ReadLine();
                Console.WriteLine("\nDitt nya konto är skapat!");
                runLoop = false; 
            }
            //when user does not wish to create a new account, the loop ends. 
            else if (userChoice.ToLower() == "nej")
            {
                runLoop = false; 
            }
            else
            {
                Console.WriteLine("Uppfattar inte vad du vill göra. Försök igen!");
            }
        }
    }
}