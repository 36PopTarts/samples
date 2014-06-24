#include <fstream>
#include <iostream>
#include <string>
#include <sstream>
#include "Menu.h"
#include "MenuItem.h"
#include "Order.h"
#include "Payment.h"
#include "Table.h"
//#include "Waiter.h"
#include "TableCollection.h"

string intToString(int);
string getWaiterString(Waiter const &);
int stringToInt(string const &);

int main() {
	TableCollection tables;
	int numWaiters = 0;
	Waiter * waiters[100];
	string temp;
	Menu menu;
	
	ifstream config("config.txt");
	getline(config, temp);
	int temp1, temp2;
	string temp3, temp4;
	double temp5;
	
	while(config >> temp1 >> temp2) {
		tables.addTable(Table(temp1, temp2));
	}
	
	config.clear();
	getline(config, temp);
	
	
	while((config >> temp3 >> temp4) && temp3!="Menu:") {
		Waiter * waiter = new Waiter(temp3);
		int comma = 0;
		while((comma = temp4.find(",")) >= 0) {
			temp1 = stringToInt(temp4.substr(0,comma));
			temp4 = temp4.substr(comma+1);
			waiter->addTable(tables[temp1]);
		}
		waiter->addTable(tables[stringToInt(temp4)]);
		waiters[numWaiters++] = waiter;
	}

	getline(config, temp);

	while(config >> temp3 >> temp4 >> temp5) {
		menu.addItem(MenuItem(temp3, temp4, temp5));
	}

	/*		READ ACTIVITY FILE		*/

	ifstream activity("activity.txt");
	string line;
	while(getline(activity, line)) {
		try {
			if(line == "")
				continue;
			istringstream issline(line);
			string buf;
			issline >> buf;
			if(buf[0] != 'T') {
				throw string("Incorrect activity format.");
			}
			Table & table = tables[stringToInt(buf.substr(1))];
			issline >> buf;
			switch(buf[0]) {
			case 'P':
				table.partySeated(stringToInt(buf.substr(1)));
				break;
			case 'O': {
				Order order;
				while(issline >> buf) {
					order.addItem(menu[buf]);
				}
				table.partyOrdered(order);
				break;
			}
			case 'S':
				table.partyServed();
				break;
			case 'C':
				table.partyCheckout();
				break;
			default:
				throw string("Incorrect activity format.");
				break;
			}
		} catch(string str) {
			cout << str << endl;
			activity.clear();
		}
	}
		try {
			cout << endl;
			cout << "1. How many tables are occupied right now?" << endl;
			cout << "2. How many orders are open right now?" << endl;
			cout << "3. How many orders have been processed so far?" << endl;
			cout << "4. What is the average occupancy of a particular table today?" << endl;
			cout << "5. What are the top 3 popular entries today?" << endl;

			int query;
			cin >> query;
			string s;
			switch(query) {
			case 1:
				cout << "Current number of occupied tables:  " << tables.getOccupiedTables() << endl;
				system("pause");
				break;
			case 2:
				cout << "Current number of open orders:  " << tables.getOpenOrders() << endl;
				system("pause");
				break;
			case 3:
				cout << "Current number of orders processed:  " << Table::processedOrders << endl;
				system("pause");
				break;
			case 4:
				cout << "Enter the table number of the table you want the average occupancy of:  ";
				cin >> query;
				cout << "Average occupancy for today:  " << tables[query].getAverageOccupancy() << endl;
				system("pause");
				break;
			case 5:
				Menu m = menu.getTop3();
				cout << "The top 3 popular entries are:  " << m.getMenuItems() << endl;
				system("pause");
				break;
			}
		} catch(string str) {
			cout << str << endl;
		}
	

	for(int i=0; i<numWaiters; i++)
		delete waiters[i];

	return 0;
}


int stringToInt(string const & str) {
	int ret;
	istringstream iss(str);
	iss >> ret;
	return ret;
}


string intToString(int num) {
	try{
		ostringstream oss;
		oss << num;
		return oss.str();
	} catch(...) {
		throw string("Incorrect number format");
	}
}
