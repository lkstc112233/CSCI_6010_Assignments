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
		Literal ptrThis = head;
		Literal ptrLast = null;
		while (ptrThis != null) {
			if (ptrThis.exponent < exp) {
				ptrLast.next = term;
				term.next = ptrThis;
				return;
			}
			ptrLast = ptrThis;
			ptrThis = ptrThis.next;
		}
		ptrLast.next = term;
	}

	public Polynomial add(Polynomial rhs) {
		if (rhs.head == null)
			return new Polynomial(this);
		else if (head == null)
			return new Polynomial(rhs);
		Polynomial result;
		Literal ptrThat;
		if (head.exponent >= rhs.head.exponent) {
			result = new Polynomial(this);
			ptrThat = rhs.head;
		} else {
			result = new Polynomial(rhs);
			ptrThat = head;
		}
		Literal ptrThis = result.head;
		while (ptrThat != null) {
			if (ptrThis.exponent == ptrThat.exponent) {
				ptrThis.coefficient += ptrThat.coefficient;
				ptrThat = ptrThat.next;
			} else if (ptrThis.next == null) {
				while (ptrThat != null) {
					ptrThis.next = new Literal(ptrThat);
					ptrThis = ptrThis.next;
					ptrThat = ptrThat.next;
				}
				break;
			} else if (ptrThis.next.exponent < ptrThat.exponent) {
				Literal post = ptrThis.next;
				ptrThis.next = new Literal(ptrThat);
				ptrThis.next.next = post;
				ptrThat = ptrThat.next;
			} else {
				ptrThis = ptrThis.next;
			}
		}
		return result;
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