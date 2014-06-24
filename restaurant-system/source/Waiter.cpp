#include "Waiter.h"

Waiter::Waiter(string const & name, int maxTables) : name(name), maxTables(maxTables), numTables(0), tables(new Table * [maxTables]) {}

Waiter::~Waiter() {
	delete [] tables;
}

void Waiter::addTable(Table & table) {
	table.assignWaiter(this);
	tables[numTables++] = &table;
}
