#include "Table.h"

Table::Table(int tableId, int maxSeats) : tableId(tableId), maxSeats(maxSeats), status(IDLE), waiter(NULL), tableOccupancyHistory(new int[300]), tableOccupancyHistorySize(0) {}
Table::Table(Table const & o) : tableId(o.tableId), maxSeats(o.maxSeats), status(o.status), waiter(o.waiter), tableOccupancyHistory(new int[300]), tableOccupancyHistorySize(o.tableOccupancyHistorySize) {
	for(int i=0; i<tableOccupancyHistorySize; i++) {
		tableOccupancyHistory[i] = o.tableOccupancyHistory[i];
	}
}
Table & Table::operator=(Table const & o) {
	tableId = o.tableId;
	maxSeats = o.maxSeats;
	status = o.status;
	waiter = o.waiter;
	delete [] tableOccupancyHistory;
	tableOccupancyHistory = new int[300];
	tableOccupancyHistorySize = o.tableOccupancyHistorySize;
	for(int i=0; i<tableOccupancyHistorySize; i++) {
		tableOccupancyHistory[i] = o.tableOccupancyHistory[i];
	}
	return *this;
}

void Table::assignWaiter(Waiter * waiter) {
	this->waiter = waiter;
}

void Table::partySeated(int numPeople) {
	if(status != IDLE) {
		throw string("Seating is only allowed at an IDLE table.");
	}
	if(waiter == NULL) {
		throw string("Cannot seat a table with no waiter.");
	}
	tableOccupancyHistory[tableOccupancyHistorySize++] = numPeople;
	this->numPeople = numPeople;
	status = SEATED;
}

void Table::partyOrdered(Order & order) {
	if(status != SEATED) {
		throw string("Table requires the SEATED status to order.");
	}
	status = ORDERED;
	this->order = order;
}

void Table::partyServed() {
	if(status != ORDERED) {
		throw string("Table must have people and must have ORDERED to be served.");
	}
	status = SERVED;
}

void Table::partyCheckout() {
	if(status != SERVED) {
		throw string("Table must have people and be SERVED to check out.");
	}
	Table::processedOrders++;
	status = IDLE;
	numPeople = 0;
}

int Table::getNumber() const {
	return tableId;
}

int Table::processedOrders = 0;

Table::~Table() {
	delete [] tableOccupancyHistory;
}

double Table::getAverageOccupancy() {
	int sum = 0;
	for(int i=0; i<tableOccupancyHistorySize; i++) {
		sum += tableOccupancyHistory[i];
	}
	return (double)sum/tableOccupancyHistorySize;
}


