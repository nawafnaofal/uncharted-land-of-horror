INCLUDE globals.ink
...
{ player_name == "": -> main | -> already_chose }

-> main

=== main ===
wah kamu terlihat cantik, kenalan dong, namaku Dadang, kamu?
    + [Memperkenalkan diri]
        -> chosen("Kirana")
    + [*Bengong]
        -> chosen("...")
    + [Berbohong]
        -> chosen("Nengsih")

=== chosen(jawaban) ===
~ player_name = jawaban
{jawaban}
-> END

=== already_chose ===
Nama kamu tuh {player_name}
-> END