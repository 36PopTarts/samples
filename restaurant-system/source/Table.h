#ifndef TABLE_H
#define TABLE_H
#include "Order.h"
#include "Waiter.h"

enum TableStatus { IDLE, SEATED, ORDERED, SERVED };

class Waiter; // to take care of circular reference.

class Table
{
private:
	int tableId;		// table number
	int maxSeats;	// table seat capacity
	TableStatus status;	// current status, you can use assign like
				// status = IDLE;
	int numPeople;		// number of people in current party
	Order order;		// current party's order
	Waiter *waiter;		// pointer to waiter for this table

	int * tableOccupancyHistory;
	int tableOccupancyHistorySize;

public:
	static int processedOrders;
	friend class TableCollection;
	Table(int tblid = 0, int mseats = 1);	// initialization, IDLE
	Table(Table const &);
	~Table();
	Table & operator=(Table const &);
	void assignWaiter(Waiter * person); 	// initially no waiter
	void partySeated(int npeople);		// process IDLE --> SEATED
	void partyOrdered(Order & order);	// process SEATED --> ORDERED
	void partyServed(void);			// process ORDERED --> SERVED
	void partyCheckout(void);		// process SERVED --> IDLE
	int getNumber() const;
	double getAverageOccupancy();
	
};
#endif
