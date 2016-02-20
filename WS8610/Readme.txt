WS8610

version 1.0

Copyright 2011 - Emanuele Iannone

Based on original C code of project Open8610, by
	Copyright 2003,2004 Kenneth Lavrsen
	Copyright 2005 Grzegorz Wisniewski, Sander Eerkes
	Copyright 2006 Philip Rayner
	Copyright 2007 Laurent Chauvin
	Copyright 2007 Philip Rayner -windows compile

This program is published under the GNU General Public License version 2.0
or later. Please read the file 'COPYING' for more info.

With WS8610 you can read and write to and from a WS-8610 weather station.
You may be able to do anything - maybe even harm the station or bring it into a mode it never comes out of again.
-whilst this is unlikely you have been warned..
It is a basic adaption of the open8610 packages, which are themselves a reworking of the open2310 and open 3600 packages.
The open2300 packages had more sub programs and features, it is maintained, but it would not talk to the ws-8610.
Open3600 did talk to the ws-8610, but the memory map was incorrect, and so taking this starting point the map has been worked out and the code altered to give temperature and humidity data for the station and additional external sensors (up to three).

The ws-8610 does hold the current time and date, but doesn't store the current reading in a fixed location, so the address position of the last reading is calculated in the memory map and it is returned as the latest reading. It is obvious that there are values for min,max, average and dewpoint readings and their times held somewhere in memory, but perhaps this is not accessible through the data download.
Once the memory is full the data is stored on a loop and this program attempts to locate the new point throughout the loop. If everything works fine, no need to reset the memory over time. When reaching the max capacity, the program will re-read from start of memory. Only potential issue I couldn't correct is on the DST change day where most probably the reading will give one hour ahead or behing depending on the case. Simplest way to correct it is to do a memory reset on that given day ;-)  Not nice, but works...

All programs are set to download the data for the number of sensors as per indicated in the memory map.

It is your choice if you want to take the risk then use it, otherwise don't.
The author takes no responsibility for any damage the use of this program may cause.

memory_map.txt is a very useful information file that tells all the currently known positions of data inside the weather station.
The information in this file may not be accurate. It is gathered by Phil Rayner and Emanuele Iannone by hours of experiments. None of the information has come from the manufacturer of the station. 
If you find something new please send email to the authors.
