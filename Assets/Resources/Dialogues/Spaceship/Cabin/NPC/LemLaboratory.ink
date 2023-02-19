INCLUDE _Globals/Globals.ink

{ knows_about_lem == true: -> FirstDialogWithPassword| { knows_password == true: ->already_knows_password | -> FirstDialogNoPassword} }

===already_knows_password===
Lem: You already know the password, leave me alone.
-> END




=== FirstDialogNoPassword ===
Lem: Van Lu, is that you? I am very busy now. If you have something to say, please say it quickly.
* [What are you doing?] -> Branch2
*  -> 
-> END



=== FirstDialogWithPassword ===
Lem: Van Lu, is that you? I am very busy now. If you have something to say, please say it quickly.
* [Hi, Andrei. I need the password for the cabinet on the bridge to retrieve the programmer.] -> Branch1
* [What are you doing?] -> Branch2
*  -> 
-> END



== Branch1
Lem: Password? Why didn't you ask Joseph? He's the robotics technician on board.
* [Josef forgot the programmer in the command center and asked me to retrieve it.] -> Branch1_1
* [What difference does it make to you, I just need the password for the cabinet.] -> Branch1_2
-> END

== Branch1_1
Lem: The password for the cabinet is 1540. But if Joseph asks you, don't tell him this code. I want to investigate him for having too simple passwords.
* [Thank you, Andrey. I won't tell the password to Josef.]
~ knows_password = true
~ password = "1540"
Lem: Okay, get back to your work. I need to finish analyzing the data and I don't have much time for conversations.
-> FirstDialogWithPassword

== Branch1_2
I do care. I don't want Josef to know this password.
* [Why don't you like Josef?] -> Branch1_3
-> END

== Branch1_3
Lem: I don't like his approach to robotics. I believe too much attention is being paid to the development of artificial intelligence and autonomous systems, which could pose a threat to human life and freedom. I think robotics should be directed towards helping and improving people's lives, not replacing them.
* [But he's just doing his job.] -> Branch1_4
* [Let's talk about something else instead.] -> Branch1_3_fallback
-> END

== Branch1_3_fallback
#donttype
Lem: I don't like his approach to robotics. I believe too much attention is being paid to the development of artificial intelligence and autonomous systems, which could pose a threat to human life and freedom. I think robotics should be directed towards helping and improving people's lives, not replacing them.
* [Josef forgot the programmer in the command center and asked me to retrieve it.] -> Branch1_1
-> END

== Branch1_4
Lem: What work? He's just following orders, without even thinking about the consequences of his actions. He doesn't understand that robots shouldn't replace people, but rather help them. That's the very problem I'm talking about. Technologies are developing faster than we can realize their impact on our lives.
* [In your words there is sense.] -> END
* [I disagree with you.]
-> END

== Branch2
Lem: I am studying data about the human brain and its connection to the Noosphere. This is a very important area of research that can expand our knowledge of the possibilities of the human mind and its potential. But as far as I know, this subject no longer interests you?
* [I was disappointed in this field. Noetics has hit a dead end.] -> Branch2_2
-> END

== Branch2_2
Lem: I disagree. I believe that we are only beginning to understand what the human mind can really do. And the data that I am currently analyzing can lead us to new discoveries.
* [Andrey, you are chasing after a ghost. We have many other areas of research that can also lead to significant scientific breakthroughs.] -> Branch2_3
* [Do you understand that the commission will still cut funding anyway?] -> Branch2_4
-> END

== Branch2_3
Lem: Well, everyone is entitled to their own opinion. But I'm sure that the research I'm conducting can make a deep and useful contribution to science and our understanding of the world.
* [Alright, as you say.] -> END
-> END

== Branch2_4
Lem: I don't think we should limit our research because of financial considerations. Our goal is to expand our knowledge and understanding of the world around us. If we are afraid to take risks, we will never reach new discoveries or solve complex problems facing humanity.
* [I hope you find what you are looking for.] -> Branch2_5
-> END

== Branch2_5
Lem: Thank you, I am confident that this will lead to new discoveries and expand our knowledge of the human brain and its potential in noetics.
* [I want to ask some more.] -> FirstDialogNoPassword
-> END






