include Makefile.helpers
modname = Proficiencies
dependencies =

assemble:
	echo "can't assemble dependency dll"

publish:
	echo "can't publish dependency dll"

install:
	(cd ../Vagabond && make install)
	(cd ../Juggernaut && make install)
	(cd ../CrusadersEquipment && make install)

play:
	(make install && cd .. && make play)
