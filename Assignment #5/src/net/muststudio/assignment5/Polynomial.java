package net.muststudio.assignment5;

public class Polynomial {
	private Literal head;

	public Polynomial() {
		head = null;
	}

	public Polynomial(Polynomial previous) {
		if (previous.head == null) {
			head = null;
			return;
		}
		Literal ptr = previous.head;
		head = new Literal(head);
		Literal tail = head;
		while (ptr.next != null) {
			tail.next = new Literal(ptr.next);
			tail = tail.next;
			ptr = ptr.next;
		}
	}

	public void insertTerm(double coef, int exp) {
		Literal term = new Literal(coef, exp);
		if (head == null) {
			head = term;
			return;
		}
		if (head.exponent < exp) {
			term.next = head;
			head = term;
			return;
		}
		Literal ptr = head;

	}

	public Polynomial add(Polynomial rhs) {
		// to be implemented
		return this;

	}

	public Polynomial multiply(Polynomial rhs) {
		// to be implemented
		return this;
	}

	public String toString() {
		// to be implemented use “^” to signify exponents
		return super.toString();
	}
}