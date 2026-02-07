SET /A index=0 
:Loop 
IF %index%==20 GOTO completed
.\priserka.exe <vstup%index%.txt >vystup%index%.txt
SET /A index=%index%+1
GOTO Loop
:completed