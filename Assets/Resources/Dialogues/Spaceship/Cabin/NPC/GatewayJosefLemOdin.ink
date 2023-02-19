INCLUDE _Globals/Globals.ink

 -> FirstDialog


//###FIRST_DIALOG###
=== FirstDialog ===
Lem: What happened? Did we make contact with the station?
Odin: Communication with the station was established, but they have obvious equipment problems. They may be in a emergency situation.
Josef: Did they confirm the docking request?
Odin: The request has been sent, but the station is not responding.
Lem: What are we going to do? Is there any protocol for action in such situations?
Josepf: Usually, we would establish communication with the station and receive instructions from the on-site team, but we don't have any contact with them. We need to determine what has happened at the station. We can manually dock with the station and go inside.
* [I can exit to the station if the docking is successful.] -> Branch1
-> END



//###BRANCH_1###
== Branch1
Odin: Van Lu is the best candidate for this role, as he is already familiar with the station and its personnel. But someone, One, must stay on the ship to maintain communication
* [I will go with Josef.] -> Branch1_1
-> END

== Branch1_1
Odin: Docking is in progress. I hope the station's defensive systems won't interfere. But before you enter the station, you need to put on your space suits. There's a risk of a biological threat or decontamination. Safety comes first.
* [Okay let's move out.] -> Branch1_3
-> END


== Branch1_3
Get off at Van Station, I'll be right behind you.
* [Ok.] 
#trigger station_open
-> DONE







