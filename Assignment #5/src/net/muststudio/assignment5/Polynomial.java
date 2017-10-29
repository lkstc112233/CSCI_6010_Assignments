package net.muststudio.assignment5;

public class Polynomial {
	private List terms ; // A list of literals
	// if you roll your own, then “private Literal head;”
	public Polynomial() { 
	 // constructor to be implemented
	}
	
	public void insertTerm(double coef, int exp){
		// to be implemented
	}
	
	public Polynomial add(Polynomial rhs) {

		// to be implemented
		return this;

	}

	public Polynomial multiply(Polynomial rhs) {
		// to be implemented
		return this;
	}

	public String toString(){
		// to be implemented use “^” to signify exponents
		return super.toString();
	}
}