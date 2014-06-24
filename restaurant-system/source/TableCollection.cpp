#include "TableCollection.h"
#include <string>

using namespace std;

TableCollection::TableCollection(int maxTables) : maxTables(maxTables), numTables(0), tables(new Table[maxTables]) {}

TableCollection::TableCollection(TableCollection const & o) : maxTables(o.maxTables), numTables(o.numTables), tables(new Table[o.maxTables]) {
	for(int i=0; i<numTables; i++)
		tables[i] = o.tables[i];
}

TableCollection::~TableCollection() {
	delete [] tables;
}

Table & TableCollection::operator[](int tableNumber) {
	for(int i=0; i<numTables; i++) {
		if(tables[i].getNumber()==tableNumber)
			return tables[i];
	}
	throw string("TableCollection::operator[] - No Table exists with specified table number.");	
}

Table const & TableCollection::operator[](int tableNumber) const {
	for(int i=0; i<numTables; i++) {
		if(tables[i].getNumber()==tableNumber)
			return tables[i];
	}
	throw string("No Table exists with specified table number.");	
}

void TableCollection::addTable(Table const & table) {
	if(numTables>=maxTables)
		throw string("TableCollection::addTable - Bounds");
	tables[numTables++] = table;
}

int TableCollection::size() const {
	return numTables;
}

int TableCollection::getOccupiedTables() {
	int count = 0;
	for(int i=0; i<numTables; i++) {
		if(tables[i].status != IDLE) {
			count++;
		}
	}
	return count;
}

int TableCollection::getOpenOrders() {
	int count = 0;
	for(int i=0; i<numTables; i++) {
		if(tables[i].status == ORDERED) {
			count++;
		}
	}
	return count;
}
