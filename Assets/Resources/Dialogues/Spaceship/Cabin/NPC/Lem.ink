INCLUDE _Globals/Globals.ink

{ has_keycard == true: ->has_key_card | { knows_password == true: ->already_knows_password | -> FirstDialog} }

===already_knows_password===
ты уже знаешь пароль, отстань
-> END

===has_key_card===
о где ты взял этот ключ? попробуй использовать его на той двери
-> FirstDialog

=== FirstDialog ===
Van Lu, is that you? I am very busy now. If you have something to say, please say it quickly.
* [Hi, Andrei. I need the password for the cabinet on the bridge to retrieve the programmer.] -> Branch1
* [What are you doing?] -> Branch2
-> END

== Branch1
Password? Why didn't you ask Joseph? He's the robotics technician on board.
* [Josef forgot the programmer in the command center and asked me to retrieve it.] -> Branch1_1
* [What difference does it make to you, I just need the password for the cabinet.] -> Branch1_2
-> END

== Branch1_1
The password for the cabinet is 1540. But if Joseph asks you, don't tell him this code. I want to investigate him for having too simple passwords.
* [Thank you, Andrey. I won't tell the password to Josef.]
~ knows_password = true
Okay, get back to your work. I need to finish analyzing the data and I don't have much time for conversations.
-> END

== Branch1_2
I do care. I don't want Josef to know this password.
* [Why don't you like Josef?] -> Branch1_3
-> END

== Branch1_3
I don't like his approach to robotics. I believe too much attention is being paid to the development of artificial intelligence and autonomous systems, which could pose a threat to human life and freedom. I think robotics should be directed towards helping and improving people's lives, not replacing them.
* [But he's just doing his job.] -> Branch1_4
* [Let's talk about something else instead.] -> Branch1_3_fallback
-> END

== Branch1_3_fallback
#donttype
I don't like his approach to robotics. I believe too much attention is being paid to the development of artificial intelligence and autonomous systems, which could pose a threat to human life and freedom. I think robotics should be directed towards helping and improving people's lives, not replacing them.
* [Josef forgot the programmer in the command center and asked me to retrieve it.] -> Branch1_1
-> END

== Branch1_4
What work? He's just following orders, without even thinking about the consequences of his actions. He doesn't understand that robots shouldn't replace people, but rather help them. That's the very problem I'm talking about. Technologies are developing faster than we can realize their impact on our lives.
* [In your words there is sense.] -> END
* [I disagree with you.]
-> END

== Branch2
Password? Why didn't you ask Joseph? He's the robotics technician on board.
-> END
