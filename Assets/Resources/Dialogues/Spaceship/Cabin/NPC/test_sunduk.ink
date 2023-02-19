INCLUDE _Globals/Globals.ink

{ knows_password == true: ->knows_password_dialog | -> FirstDialog}


===FirstDialog===
Enter password:
* [Random password]
* [Old password]
- Wrong password
->END


===knows_password_dialog===
введите пароль:
* [Random password]
* [Old password]
* [{password}]
    Password accepted
    #trigger sunduk_open
    ->END
- Wrong password
-> END