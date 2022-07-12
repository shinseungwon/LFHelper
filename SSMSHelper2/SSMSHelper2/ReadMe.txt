only reads file starts with !

pre parts
include (starts with +)
exclude (starts with -)

-----Example-----

@Part0 //Name (Just for tag, if it's starts with !, it doesn't loaded)
{F9} //Key which to hook (Only 1 key, shift + / control ^)
1 //Action type (Please refer to below)
{END}+{HOME}{BACKSPACE} //Content (Depends on action type)

@!Part1
{F9}
1
{END}+{HOME}{BACKSPACE}

-----~Eample-----

-----Action Types-----

1 -> Key conversion
Content : sendkey string to key out

2 -> text write 

SubCommands
	2-1:Just write text in content

	2-2:Write text in content after replace {%n} parts with data in clipboard(spliietd by space 0 1 2 ..)

	2-3:3 lines for content, if we paste,
	line1 -> just line 1 in content
	line2 -> if data in clipboard like 2-2 and also multilines, repeat as many as lines in clipboard
	line3 -> just line 3 in content

	3:run exe file
~SubCommands

SubCommands
	3-1:run with command prompt + parameters

	3-2:do 3-1 without command prompt but standard output 
	(has subtypes depends on how to return output)

	SubSubCommands
		3-2-1:export as file and write filename to console
		3-2-2:export as file and open file using notepad
		3-2-3:storer output text in clipboard
		3-2-4:write output text
	~SubSubCommands

	3-3:open exe file with parameters (no console, no standard output)
~SubCommands

-----~Action Types-----