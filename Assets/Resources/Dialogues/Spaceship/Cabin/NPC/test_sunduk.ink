INCLUDE _Globals/Globals.ink

{ knows_password == true: ->knows_password_dialog | -> FirstDialog}


===FirstDialog===
введите пароль:
* [qwerty]
* [cumshot]
- пароль неверный
->END


===knows_password_dialog===
введите пароль:
* [qwerty]
* [cumshot]
* [{password}]
    пароль принят
    #trigger sunduk_open
    ->END
- пароль неверный
-> END