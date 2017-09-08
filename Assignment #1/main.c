#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

// For marco 'PRId64'.
// #define __STDC_FORMAT_MACROS
#include <inttypes.h>

#define TRUE 1
#define FALSE 0

//Return the number of digits in digit
int getSize( int64_t number )
{
	if (number < 0) return -1;
	if (number < 10) return 1;
	return 1 + getSize(number / 10);
}


//Return the first k number of digits from the number. If the number of digits in number is less than k, return number 
int64_t getPrefix( int64_t number, int k )
{
	int size;
	if (number == 0) return 0;
	if (k == 0) return 0;
	size = getSize(number);
	if (size <= k) return number;
	return getPrefix(number / 10, k);
}

//Return this number it is a single digit, otherwise, return the sum of the two //digits
int getDigit( int number )
{
	if (number < 0) return -1;
	if (number < 10) return number;
	return (number % 10) + getDigit(getPrefix(number, getSize(number) - 1));
}

//Get the result from step 2
int sumOfDoubleEvenPlace( int64_t number )
{
	int size;
	size = getSize(number);
	if (size < 2) return 0;
	if (size < 3) return getDigit(number / 10 * 2);
	return getDigit((number % 100) / 10 * 2) + sumOfDoubleEvenPlace(getPrefix(number, size - 2));
}

//Return sum of sumOfOddPlace( long number )
int sumOfOddPlace( int64_t number )
{
	int size;
	size = getSize(number);
	if (size < 2) return number;
	return (number % 10) + sumOfOddPlace(getPrefix(number, size - 2));
}

//Return if the card number is valid 
int isValid(int64_t number)
{
	int m_sumOfDoubleEvenPlace;
	int m_sumOfOddPlace;
    int size;
    int firstTwoNumber = getPrefix(number, 2);
    size = getSize(number);
    if (size < 13) return FALSE;
    if (size > 16) return FALSE;
	// Check if the prefix meets the assertion.
	if (firstTwoNumber != 37 ) // For American Express cards
	{
		int firstNumber = getPrefix(firstTwoNumber, 1);
		switch(firstNumber)
		{
			case 4: // For Visa cards
			case 5: // For Master cards
			case 6: // For Discover cards
				break;
			default:
				return FALSE; // Fails to meet the prefix assertion.
		}
	}
	m_sumOfDoubleEvenPlace = sumOfDoubleEvenPlace(number);
	m_sumOfOddPlace = sumOfOddPlace(number);
	if ((m_sumOfOddPlace+m_sumOfDoubleEvenPlace)%10)
		return FALSE;
	return TRUE;
}

int main(int argc, char** argv)
{
	FILE* inputFile = stdin;
	int redirected = FALSE;
	int64_t creditCardNumber = 0;
	if (argc > 1)
	{
		if (argc == 3)
		{
			if (!strcmp(argv[1], "-i"))
			{
				inputFile = fopen(argv[2], "r");
				if (inputFile)
					redirected = TRUE;
			}
		}
	}
	while (TRUE)
	{
		if (!redirected)
			printf("Enter a credit card number as a long integer\n> ");
		fscanf(inputFile, "%"PRId64, &creditCardNumber);
		if (creditCardNumber < 0) break;
		printf("%16"PRId64" is ", creditCardNumber);
		if (!isValid(creditCardNumber))
			printf("not ");
		printf("a valid credit card number.\n");
	}
	return 0;
}
