package net.muststudio.assignment5;

public class Literal {
	Literal(double coefficient, int exponent)
	{
		this.coefficient = coefficient;
		this.exponent = exponent;
		next = null;
	}
	Literal(int exponent)
	{
		this.coefficient = 1;
		this.exponent = exponent;
		next = null;
	}
	Literal(Literal previous)
	{
		this.coefficient = previous.coefficient;
		this.exponent = previous.exponent;
		next = null;
	}
	Literal Add(Literal operatee)
	{
		if (exponent != operatee.exponent)
			throw new IllegalArgumentException("Exponent mismatch");
		Literal result;
		result = new Literal(coefficient + operatee.coefficient, exponent);
		return result;
	}
	Literal Multiply(Literal operatee)
	{
		Literal result;
		result = new Literal(coefficient * operatee.coefficient, exponent + operatee.exponent);
		return result;
	}
	double coefficient;
	int exponent;
	Literal next;
}

