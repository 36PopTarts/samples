#include "MenuItem.h"

MenuItem::MenuItem(string const & mcode, string const & mname, double mprice):
	code(mcode), name(mname), price(mprice), timesOrdered(0) {}
	
string const & MenuItem::getCode() const {
	return code;
}

string const & MenuItem::getName() const {
	return name;
}

double MenuItem::getPrice() const {
	return price;
}

int & MenuItem::getTimesOredered() {
	return timesOrdered;
}
