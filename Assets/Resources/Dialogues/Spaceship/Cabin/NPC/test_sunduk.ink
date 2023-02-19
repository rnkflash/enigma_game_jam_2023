INCLUDE _Globals/Globals.ink

{ knows_password == true: ->knows_password_dialog | { knows_wrong_password == true: ->knows_wrong_password_dialog | -> FirstDialog}}



===FirstDialog===
Enter password:
* [Random password]
- Wrong password
->END


===knows_password_dialog===
Enter password:
* [Random password]
* [1234]
* [{password}]
    Password accepted
    Finally, I found the programmer. I need to take him to Joseph.
    You have received the programmer.
    ~ has_programmer = true
    #trigger sunduk_open
    ->END
- Wrong password
-> END


===knows_wrong_password_dialog===
Enter password:
* [Random password]
* [1234]
- Wrong password
~ tried_wrong_password = true
-> END