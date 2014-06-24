#ifndef MENUITEM_H
#define MENUITEM_H
#include <iostream>
#include <string>

using namespace std;

class MenuItem
{
private:
	string code; // See sample codes in config.txt
	string name; // Full name of the entry
	double price; // price of the item
	int timesOrdered;

public:
	MenuItem(string const & mcode = "", string const & mname= "", double mprice = 0);
	string const & getCode() const;
	string const & getName() const;
	double getPrice() const;
	int & getTimesOredered();
};
#endif
