modname = AntiAlchemyAbuse
gamepath = /mnt/c/Program\ Files\ \(x86\)/Steam/steamapps/common/Outward/Outward_Defed
pluginpath = BepInEx/plugins

assemble:
	echo "can't assemble dependency dll"
publish:
	echo "can't publish dependency dll"
install:
	(cd ../Vagabond && make install)
	(cd ../Juggernaut && make install)
	(cd ../CrusadersEquipment && make install)
clean:
	rm -f -r public
	rm -f -r thunderstore
	rm -f $(modname).rar
	rm -f $(modname)_thunderstore.zip
	rm -f resources/manifest.json
	rm -f README.md
info:
	echo Modname: $(modname)
play:
	(make install && cd .. && make play)
