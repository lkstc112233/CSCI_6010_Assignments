#include <stdio.h>
#include <stdlib.h>

int main(int argc,char* argv[])
{
int number = 1000;
if (argc > 1)
	number = atoi(argv[1]);
while(number > 0)
{
	int random;
	random = rand()%(99999999 - 1000000 + 1);
	random += 1000000;
	printf("%d", random);
	random = rand()%(99999999 - 1000000 + 1);
	random += 1000000;
	printf("%d\n", random);
	number -= 1;
}
return 0;
}
