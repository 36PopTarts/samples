#include "Order.h"

Order::Order(int count) : maxItems(count), menuItems(new MenuItem const * [count]), numItems(0) {}

Order::Order(Order const & o) : maxItems(o.maxItems), menuItems(new MenuItem const * [o.maxItems]), numItems(o.numItems) {
	for(int i=0; i<o.numItems; i++) {
		menuItems[i] = o.menuItems[i];
	}
}

Order & Order::operator=(Order const & o) {
	maxItems = o.maxItems;
	delete [] menuItems;
	menuItems = new MenuItem const * [o.maxItems];
	numItems = o.numItems;
	for(int i=0; i<o.numItems; i++) {
		menuItems[i] = o.menuItems[i];
	}
	return *this;
}

void Order::addItem(MenuItem & itemp) {
	if(numItems>=maxItems)
		throw string("Order::addItem - Too many MenuItems");
	itemp.getTimesOredered()++;
	menuItems[numItems++] = &itemp;
}

Order::~Order() {
	delete [] menuItems;
}

double Order::getTotalPrice() const {
	double total = 0;
	for(int i=0; i<numItems; i++)
		total += menuItems[i]->getPrice();
	return total;
}
