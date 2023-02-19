INCLUDE _Globals/Globals.ink


{ command_room == true: -> CommandRoomActivated  | { tried_wrong_password == true: -> wrong_password  | { knows_about_pragrammer == true: -> FirstDialogWithProgrammer  | -> FirstDialogWihoutProgrammer }}}

//{ command_room = true: -> CommandRoomActivated | -> FirstDialogWihoutProgrammer }

command_room == true: -> CommandRoomActivated
tried_wrong_password == true: -> wrong_password
knows_about_pragrammer == true: -> FirstDialogWithProgrammer 
===wrong_password===
Van Lu: The password you gave me did not work for the cabinet. Are there any other options?

Odin: It is possible that the password was changed by Andrei Lem. He often complained about the simplicity of the cabinet passwords.
* [Good, where can I find him?] -> wrong_password_1_1
* [I will go ask him.]
~ knows_about_lem = true
-> END


== wrong_password_1_1
Odin: Andre Lem is currently in the laboratory. He has been there since yesterday.
*  I'll go look for him.
~ knows_about_lem = true
-> END




//###FIRST_DIALOG###
=== FirstDialogWihoutProgrammer ===
Odin: Greetings, Van Lu. The analysis shows that all systems are in order.
* [How much time do we have left until we reach the station?] -> Branch1
* [Tell me about the ship.] -> Branch2
//* [Do you know the password for the locker on the bridge?] -> Branch3
* [I think I'll go.] -> END
-> END


=== FirstDialogWithProgrammer ===
Odin: Greetings, Van Lu. The analysis shows that all systems are in order.
* [How much time do we have left until we reach the station?] -> Branch1
* [Tell me about the ship.] -> Branch2
* [Do you know the password for the locker on the bridge?] -> Branch3
* [I think I'll go.] -> END
-> END



//###BRANCH_1###
== Branch1
Odin: We will enter the gravitational field of the Eynomia station in 12 minutes. After that, we will dock with the station.
* [Okay, thank you.] -> FirstDialogWithProgrammer
-> END




//###BRANCH_2###
== Branch2
Odin: The "Odyssey" is a cutting-edge scientific research spaceship equipped with advanced systems and technologies for exploring the uncharted depths of space. On board, there are two independent artificial intelligences, One and Icarus, which ensure the safety and comfort of the crew. The laboratory and autonomous drones allow for the investigation of various celestial objects.
* [Why does the ship have two artificial intelligences?] -> Branch1
* [Good. I wanted to ask something else.] -> FirstDialogWithProgrammer
-> END

== Branch2_1
Odin: The two artificial intelligences on board the "Odyssey" spacecraft are designed to ensure the safety and seamless operation of all systems. In case of failure of one AI, the other can take over its functions and prevent problems. Additionally, each AI is responsible for certain systems: I am responsible for navigation and control over the generator, while Ikar is responsible for research and the life support system. This provides more reliable operation of the ship and increases the safety of the entire crew.
* [There is something to think about.] -> FirstDialogWithProgrammer
-> END


//###BRANCH_3###
== Branch3
Odin: I don't have access to personal items, but I still have passwords in my database. Try 1234.
* [What a silly password, but I will check it.] 
~ knows_wrong_password = true
-> END


===CommandRoomActivated===
Odin: Connect to the station via the transmitter. It is located in the middle of the command deck, near the window.
* [Okay, got it.] -> CommandRoomActivated_1
-> END

== CommandRoomActivated_1
Van Lu: This is the ship "Odyssey". Requesting permission to land.

Station: ...
* [Can anyone hear me?] -> CommandRoomActivated_2
* [Station Einomia, this is the "Odyssey" spacecraft. Requesting permission to dock.] -> CommandRoomActivated_2
-> END


== CommandRoomActivated_2
Station: ...rece... here... Einomi...

Station: ...
* [Reception! Einomiya Station, I can't hear you! Repeat!] -> CommandRoomActivated_3
* [What is going on?] -> CommandRoomActivated_3
-> END

== CommandRoomActivated_3
Station: ...Mega... Eshar structure...

Station: ...
* [I don't understand, repeat!] -> CommandRoomActivated_4
-> END

== CommandRoomActivated_4
Station: Van, is that you?

Station: ...
* [With whom am I speaking?] -> CommandRoomActivated_5
* [This voice...] -> CommandRoomActivated_5
-> END

== CommandRoomActivated_5
Station: Why did you leave me, Van?

Station: ...
* [What's going on here?] -> CommandRoomActivated_6
* [Violette, is that you? But how...] -> CommandRoomActivated_6
-> END

== CommandRoomActivated_6
Transmitter: Communication interrupted.

Van Lu: This is some kind of madness. It can't be that she... One, did you hear that too?

Odin: There were strong interference on the station frequency, but there definitely is someone there.
~ gateway_room = true
#trigger gatewayTrigger
-> END

