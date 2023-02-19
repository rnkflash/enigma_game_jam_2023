INCLUDE _Globals/Globals.ink

{ knows_password == true: ->knows_password_dialog | -> FirstDialog}


===FirstDialog===
введите пароль:
* [1234]
* [qwerty]
* [cumshot]
- пароль неверный
->END


===knows_password_dialog===
введите пароль:
* [{password}]
    пароль принят
    ->END
* [qwerty]
* [cumshot]
- пароль неверный
-> END