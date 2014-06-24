#ifndef PAYMENT_H
#define PAYMENT_H
#include "Order.h"
#include "Menu.h"
#include "Waiter.h"

class Payment 
{
private:
	Table * table; 	// table number
	Waiter *waiterp;	// pointer to waiter

public:
	Payment(Table const & table, Waiter const & waiter);
};
#endif
