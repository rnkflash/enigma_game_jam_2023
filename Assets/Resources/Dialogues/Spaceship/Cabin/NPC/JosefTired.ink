INCLUDE _Globals/Globals.ink

 -> FirstDialog


//###FIRST_DIALOG###

=== FirstDialog ===
 Developers - Welcome to the world of I.S.E, player! The development team thanks you for your interest in our game and we hope you enjoy it. We would appreciate if you could leave your feedback and suggestions on our game's pages. We have tried to create an engaging and complete prologue in the form of a game jam, but unfortunately, some of our ideas could not be realized. We continue to work on improving and developing our game, and hope you will stay with us on this journey.
* [Do you plan to complete the game?] -> Branch1
* [I'll go.] 
-> END

=== Branch1_3 ===
 Developers - Our team plans to expand the game's story, add a more engaging combat system, and a variety of monsters. Additionally, we intend to completely overhaul the prologue and improve the game's visual design
* [Who are the developers of this game?] -> Branch2
* [Thanks for the game!] -> END



//###BRANCH_1###

== Branch1
 Developers - We plan to continue developing and refining I.S.E after the game jam. However, as game development is a labor-intensive process, we are taking a small break to recharge and gather new ideas.
* [What do you plan to add to the game?] -> Branch1_3
* [Who are the developers of this game?] -> Branch2
-> END





== Branch2
The developers of this game are: 
Programmer - Stepan Maximov 
Technical Artist - Evgeniy Petrov
Location Designer - Artur Solodov
 Sound Designer - Artem Kalganov and Arsenii Khodulov
 Narrative Designer - Evgeniy Petrov and Artur Solodov
 Text Editor - Artem Kalganov and Chardot 
Text Translator - Chardot
* [Thanks for the game] -> END
-> DONE







