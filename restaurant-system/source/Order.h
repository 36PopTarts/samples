#ifndef ORDER_H
#define ORDER_H

#include "MenuItem.h"
#include "Menu.h"

class Order 
{
private:
	int maxItems;  // # of items in the order
	int numItems;  // current # of items in the order
	MenuItem const ** menuItems;
	
public:
	Order(int count = 100); //allocates array of pointers to "selected" menu items
	Order(Order const &);
	Order & operator=(Order const &);
	void addItem(MenuItem & itemp); // add one item at a time to the order
	double getTotalPrice() const;
	~Order();
};
#endif
