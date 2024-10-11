# Internet bank
Program: Systemutveckling .NET  
Course: Utveckling med C# och .NET (final assignment)  

## About the program  
This program simulates an "internet bank" with five pre-set users.  

The head meny contains the following actions:  
- Check accounts and balances
- Transfer money between accounts
- Withdraw money from accounts
- Open a new account
- Log out

## Overall structure of the program
The arcitecture is centered around a number of arrays: 
- A 2D string array called customers for storing names, personal number and pincode. two jagged arrays.
- A jagged array with 2D-arrays for storing account numbers and balances
- A jagged array for storing accounts names.

When the customer is logging in, a customer index is saved and returned. This will later be used to connect 
these arrays and customer data. 

## Reflections 
### Choices made and why
In the first version of a working program, I've created a 3D-array that stored the following data:  
- personal numbers on index [i, 0, 0]
- pinCode on [i, 1, 0]
- account balances on [i, y, 0]
- account nr on [i, y, 1].
  
This was complemented with one 2D string array storing name of customer [i, 0] and bank accounts [i, y]. The idea was 
to have as much customer data as possible in as few arrays as possible. Less arrays to enter in methods and 
keep updated etc. However, this system meant that the array had to be as big as needed to room the customer with the
most accounts. In other word it was not a 100% space effective set-up. 

Instead I changed to the current set-up, that enabels me to have different amounts of customer accounts for each 
customer. It was then also easy to create more accounts in "Open Accounts Method) since I could create the accounts 
of one customer as one 2D-arrray that, once updated with and additonal element, could replace the accoutn array at 
the specific customer index. If keeping the 3D array, I would need to make the entire array of all customers one element 
bigger and replace all array info etc. 

An additonal plus was that the information is now structured in a more clear way I think. Accounts in one place, 
customer info in another etc. 

The reason I wanted to have account numbers as well as balance was (except the fact that it is more "realistic") 
that I wanted to have it in case I'd like to make the additonal task of transfering money to accounts of 
another customer. I ended up not doing it but ot would be straight forward to do, using the account nr.

### Improvement potential
Using classes and lists etc was not allowed so that wont be further mentioned. 

There is potenatial for making smaller methods and to do more of them, There are repetitions in the code currently,
and a number of the methods are quite big. 

One of the very last thing I entered was the option to quit once in one of the meny choices. This was not done in an optimal
way. I was worse when I entered the assignment but with a more thought-throug plan for how to solve this throughout the entire
program, it could be nicer from a user perspective and clutter the code less. 

The interface could be nicer of course and some messages etc could be even more streamlined. 



