INCLUDE _Globals/Globals.ink

{ has_programmer == true: ->has_programmer_tool  | -> FirstDialog}


===has_programmer_tool===
Van Lu: I brought your programmer, here you go.
Josef: Thank you, Van. I was already worried that I wouldn't be able to finish the work before arriving at the station.
* [No problem, I'm glad to help. How's the drone work going for you?] -> BranchProgrammer_1_1
-> END

=== BranchProgrammer_1_1 ===
Josef: I'm sorry for asking a personal question, but you mentioned your wife. What happened to her?

Van Lu: My wife...

Odin: Attention, we have entered the gravitational field of the Einomia station. However, I cannot establish contact with the station. I am not getting a response. I am concerned that something may have happened.

Josef: It may be possible that they have communication problems. It needs to be checked personally. Van, try to contact the station manually through the transmitter in the command bunker.
*  [Okay, I'll check.] -> BranchProgrammer_1_2
-> END

== BranchProgrammer_1_2
Josef: I will soon come to the command room.
*  Ok 
-> END




//###FIRST_DIALOG###
=== FirstDialog ===
Josef: Oh, Van, you look tired. Haven't you been getting enough sleep lately? Is something bothering you?
* [Yes, I really can't sleep well. I'm very nervous about arriving at the station.] -> Branch1
* [Nothing serious, Joseph. Just insomnia.] -> Branch2
*  -> 
-> END



//###BRANCH_1###
== Branch1
Josef: I remember you mentioned that you worked at that station before. If you're worried that your former colleagues will be unhappy with your role in closing the station, you shouldn't stress about it. They will find other places to work, so it doesn't really matter.
* [It's possible, you're right. I just can't seem to shake this unpleasant thought.] -> Branch1_1
* [I don't think my personal problems concern you.] -> Branch1_2
-> END

== Branch1_1
Josef: You have to understand that you are doing the right thing. The funding allocation by the committee is for the benefit of humanity. Don't forget that.
* [Thank you, Josef.] -> Branch1_3
* [I have a feeling that I'm not doing this for the sake of humanity, but for myself] -> Branch1_4
-> END

== Branch1_2
Josef: Sorry, I won't pry anymore. Hmm. I've got a problem here. I left the programmer in the command hub, and the contacts, damn them, are already closed. I don't want to change the wiring of this bucket again. Could you bring me the programmer? It's in a box on the bridge.
* [I'll see what I can do, but I make no promises.] -> Quest //###BEGIN_QUEST###
-> END

== Branch1_3
Josef: No problem, Van. We're just doing our job. Oh, by the way, I have a small issue. I'm working on this drone, and it's got me stuck. I've closed the wires on the circuit board, but the programmer is still on the bridge. Without it, I can't open the wiring and the board will burn out. Can you help me bring the programmer? It's in a box on the bridge.
* [Of course, you can count on me.] -> Quest //###BEGIN_QUEST###
-> END


== Branch1_4
Josef: Hmm... You're a difficult person, Van. In my opinion, you're putting too much pressure on yourself. Anyway, it's good that you came. I closed the wires on the circuit board, but I left the programmer in the command hub. Without it, if I open the wiring, the board will burn out. Could you bring it to me? It's in a box on the bridge.
* [I'll try my best to find it.] -> Quest //###BEGIN_QUEST###
* [I disagree with you.]
-> END




//###BRANCH_2###
== Branch2
Josef: Is everything okay? Why do you look so worried? Is it just insomnia?
* [I'm very anxious about arriving at the station.] -> Branch1
* [Maybe so, Joseph. But I need to calm down and focus on our tasks.] -> Branch2_3
-> END

== Branch2_3
Joseph: Great, Van! Then you came just in time. I left the programmer in the command hub, and the contacts are already closed. I'm afraid that if we don't find it soon, we'll have to replace all the equipment. Could you help me and bring the programmer? It's in a box on the command bridge.
* [Okay, I'll try to find it.] -> Quest //###BEGIN_QUEST###
-> END



//###QUEST###
== Quest
Find programmer
*  Ok 
-> END




