#ifndef MENU_H
#define MENU_H
#include "MenuItem.h"

class Menu 
{
private:
	int maxItems; // MAX capacity 
	int numItems; // current # of items in menu
	MenuItem *menuItems;

public:

	Menu(int items = 100); //constructor to allocate memory
	Menu(Menu const &);
	void addItem(MenuItem const &); //add one menu item at a time
	MenuItem & operator[](string const &) const;  //lookup operation
	~Menu(); //destructor
	Menu getTop3();
	string getMenuItems() const;
};
#endif
