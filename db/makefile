

all:
	@echo -e "\tdb - connects to database"
	@echo -e "\tgraph - create graphs"
	@echo -e "\tdata - run queries"
	@echo -e "\tclean - removes graphs"

db:
	mysql -hprd.db.centerforgamescience.com \
		--database "gc_sp21_theotherside_log" \
    	-u gc_sp21_202101 

data:
	mysql -hprd.db.centerforgamescience.com \
		--database "gc_sp21_theotherside_log" \
		-u gc_sp21_202101 \
		< graphs.sql > out/data.txt
		# < quick.sql > out/data.txt

graph:
	python3 process_graphs.py

clean:
	rm -rf ./out/graphs/*
