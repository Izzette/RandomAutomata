# RandomAutomata

Simple random sequence generation library.

Based on a next and next nearest neighboured cellular automaton.
Utilizes absolute rule number 1436965290, skiping 4 generations on each get next call.

Interfaces through class RandomSequence.
Initialized with a UInt64 random seed.
Returns time-spaced array of bool or byte.
Length of bool array is 64.
Length of byte array is 8.
Can also return a single UInt64 or Int64.

Utilizes parallelism for fast run-time performance.

The one public class from the RandomAutomata project, RandomAutomata.RandomSequence is used much like the System.Random class from Mono / .NET.
It has the constructors RandomSequence (ulong seed), which is allows the specification of a random seed.
It has the overloaded constructor RandomSequence () which produces a random random-seed from System.Random using the parameterless constructor seeded with system time.

Skips next 4 CA evolution steps on each call!

bool[] RandomSequence.GetNextBools () returns 64 bools as an array.
bool[] RandomSequence.GetNextBools (int length) returns an array of bools of specified length.

byte[] RandomSequence.GetNextBytes () returns 8 bytes as an array.
byte[] RandomSequence.GetNextBytes (int length)

ulong RandomSequence.GetNextULong ()
long RandomSequence.GetNextLong ()

void RandomSequence.Skip (int steps) skip the next stpes, not the next 4 steps!

static int BoolsLength returns 64, the length of the array in the parameterless GetNextBools ()
static int BytesLength returns 8, the length of the array in the parameterless GetNextBytes ()

The two other perojects are used to test RandomAutomata.RandomSequence.

RandomnessBitmap compiles to an excecuteable, accepts command line arguments:
<width> <height> <path/to/output> <"color"/"bw">
Produces a bitmap from the RandomSequence class that is of specified width and height.
The last argument determines whether the bitmap will use GetNextBools (width * height) producing a black and white bitmap or GetNextBytes (3 * width * height) and produces a color bitmap from three bytes for each pixel.
It measures the time it takes to complete each task.

SequenceHang also compiles to an excecuteable, accepts command line arguments:
<iterations> <"bools"/"bytes"> [<length>]
excecutes iterations of GetNextBools (length) or GetNextBytes (length), if no specified length it will use the parameterless equivalents.
It measures the time it takes to complete all iterations.
