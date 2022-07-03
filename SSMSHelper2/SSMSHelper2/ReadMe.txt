only reads file starts with !

pre parts
include (starts with +)
exclude (starts with -)

@
1st parts
input key (based on sendkeys syntax rules)
<1 line break>

2nd parts
output types

1:key 

2:text write 
2-1:text write
2-2:text write with paste
2-3:text write with paste and repeat lines

3:run batch file
3-1 run batch file
3-2 run batch file with parameters
3-3 run batch file with parameters and repeat lines
<1 line break>

3rd parts
output data

1:key macro (based on sendkeys syntax rules)

2:
2-1:text

2-2:text with {0} {1} {2} ... -> will replaced with texts in clipboard seperated by space

2-3:text with {0} {1} {2} ... -> will replaced with texts in clipboard seperated by space 
and repeated with line breaks

3:
3-1:batch file name -> just execute
3-2:read all batch files text and replace the {0}, {1}, {2} in clipboards seperated by space and execute
3-3:read all batch files text and replace the {0}, {1}, {2} in clipboards seperated by space
and repeated with line breaks and execute

