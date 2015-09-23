IN PROGRESS, NOT COMPLETED YET

PILOT.NET

by William Mortl
http://www.williammortl.com
(c)2015

Written in C# - Microsoft Visual Studio 2013 for Microsoft .NET 4.5
There is no warranty implied with this code, and it is not to be used for commercial purposes without proper recompence. Educational use is fine as long as appropriate citation is given.



NOTES:
------

This implemetation's permanent home is:

https://github.com/williammortl/Pilot.NET

The Facebook fan page for PILOT is:

https://www.facebook.com/PILOTProgrammingLanguage

A little about the PILOT language:

http://en.wikipedia.org/wiki/PILOT

A little about the creator of PILOT, Dr. John Starkweather:

http://en.wikipedia.org/wiki/John_Amsden_Starkweather

This PILOT interpreter was lovingly built to be as compatible as possible with the PILOT implementation released for Atari 8-bit computers. Please refer to these linked PDF user guides for more information about PILOT:

http://www.atarimania.com/documents/Atari-Student-PILOT-Reference-Guide.pdf

http://www.atarimania.com/documents/Atari_PILOT_Primer.pdf

Upon completion, this version of PILOT will contain Atari-style Turtle Graphics as well as the ability to create sound.



DIFFERENCES FROM ATARI PILOT:
-----------------------------

PILOT.NET differes from Atari Pilot insomuch as it does not allow for repeating graphics expressions or multiple expressions per GR statement. An example of this would be:

10 GR: 3(DRAW 50; TURN 120); FILL 24

This is an extremely ugly shorthand, and I flat-out refuse to implement it. The previous code would now be implemented in PILOT.NET as:

10 C: #I = 0
20 *FORI GR: DRAW 50
30 GR: TURN 120
40 C: #I = #I + 1
50 J(#I < 3): *FORI
60 GR: FILL 24

Since modern PC's do not have have the ability to generate the weird "fishing hook"-shaped character that Atari Pilot used in a T statement to clear the screen, I added a special T statement to clear the screen. It is written as:

10 T:{CLEAR}

