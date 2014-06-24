#include <stdlib.h>
#include "Menu.h"

Menu::Menu(int items) : maxItems(items), numItems(0), menuItems(new MenuItem[items]) {}
Menu::Menu(Menu const & o) : maxItems(o.maxItems), numItems(o.numItems), menuItems(new MenuItem[o.maxItems]) {
	for(int i=0; i<o.numItems; i++) {
		menuItems[i] = o.menuItems[i];
	}
}

void Menu::addItem(MenuItem const & item) {
	if(numItems>=maxItems) {
		throw string("Menu::addItem - Too many menu items");
	}
	menuItems[numItems++] = item;
}

MenuItem & Menu::operator[](string const & code) const {
	for(int i=0; i<numItems; i++) {
		if(menuItems[i].getCode()==code)
			return menuItems[i];
	}
	
	throw string("Menu::findItem - Invalid MenuItem code");
}

Menu::~Menu() {
	delete [] menuItems;
}

int compare(const void * one, const void * two) {
	return ((MenuItem*)two)->getTimesOredered() - ((MenuItem*)one)->getTimesOredered();
}

Menu Menu::getTop3() {
	qsort(menuItems, numItems, sizeof(MenuItem), compare);
	Menu ret;
	ret.addItem(menuItems[0]);
	ret.addItem(menuItems[1]);
	ret.addItem(menuItems[2]);
	return ret;
}

string Menu::getMenuItems() const {
	string ret;
	for(int i=0; i<numItems; i++)
		ret += menuItems[i].getName()+" ";
	return ret;
}
