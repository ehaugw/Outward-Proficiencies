include Makefile.helpers
modname = Proficiencies
dependencies =

forceinstall:
	(cd ../Vagabond && make install)
	(cd ../Juggernaut && make install)
	(cd ../CrusadersEquipment && make install)

play:
	(make install && cd .. && make play)
