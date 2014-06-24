Description
-----------
This is a simple system coded in C++ which keeps track of waiters, tables, and orders; 
this program might be installed on a terminal at the host's or hostess' podium at the 
front of the restaurant, as well as at the manager's desk. The program can keep track 
of which tables are assigned to whom, how much a table owes at the end of the dinner, 
what the most popular entrees are, which orders need to be filled, and which tables
are unoccupied. 

This program requires the Microsoft Visual C++ Runtime libraries.

Input
-----------
config.txt - This is the input that tells the program which waiters are assigned 
to what tables, what the menu for today is and the prices of the menu items, and the
number of tables and their associated number of seats available. 

activity.txt - This file is the input that might be given by the host or manager;
it is the activity of the restaurant for that day, shortened to a simple syntax.
T#: Indicates which table is being addressed. Each table has a unique number.
P#: Party of # amount is being seated at the table indicated.
O: The table has ordered the following items.
These are the identifiers for the menu items. They can be seen in config.txt as well,
with full names of the menu items.
A#: Appetizer number #.
E#: Entree number #.
S#: Beverage number #.
D#: Dessert number #. (not used in current setup; there are dessert menu items but nobody orders them)


S: Served; indicates that the table has been served its orders and won't be 
needing anything else for the evening.
C: The table has called for the check and paid in full.

Output
-----------
The system this program is built upon is robust enough for me to modify it to display
many different statistics, but I have left it the way it was when I turned it in 
for my one of my college assignments. As it is, the user can choose from five options,
which are quite self-explanatory:
1. How many tables are occupied right now?
2. How many orders are open right now?
3. How many orders have been processed so far?
4. What is the average occupancy of a particular table today?
5. What are the top 3 popular entries today?

The first two options will always return "zero" because the activity file ends before
the prompt comes up; that is to say, the restaurant has closed by the time this prompt
appears. This is trivial to change and if the reader goes through my code, he or she
will see that I clearly did not "cheat" or hard-code any output.
