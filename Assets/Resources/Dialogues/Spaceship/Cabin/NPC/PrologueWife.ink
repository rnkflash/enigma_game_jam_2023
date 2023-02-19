INCLUDE _Globals/Globals.ink

 -> FirstDialog


//###FIRST_DIALOG###
=== FirstDialog ===
Violette: Are you back again? Have you changed your mind after all?
* [I have returned for you.] -> Branch1
-> END



//###BRANCH_1###
== Branch1
Violette: Why did you leave me here all alone?
* [I was forced to do it. I had to move on.] -> Branch1_1
* [I didn't know things would turn out this way for us.] -> Branch1_1
-> END

== Branch1_1
Violette - Anyway, nothing can be changed now.
* [I miss you.] -> Branch1_3
-> END


== Branch1_3
Violette - Goodbye.
* [Please don't go!] 
#trigger wife_jump
-> END







