#include "Table.h"

class TableCollection {
private:
	Table * tables;
	int maxTables;
	int numTables;
public:
	TableCollection(int maxTables = 100);
	TableCollection(TableCollection const &);
	~TableCollection();
	Table & operator[](int);
	Table const & operator[](int) const;
	void addTable(Table const &);
	int size() const;
	int getOccupiedTables();
	int getOpenOrders();
};
