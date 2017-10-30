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
		head = new Literal(ptr);
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
		Polynomial result = new Polynomial();
		Literal ptr = rhs.head;
		while (ptr != null) {
			result = result.add(this.multiply(ptr));
			ptr = ptr.next;
		}
		return result;
	}

	private Polynomial multiply(Literal rhs) {
		if (rhs.coefficient == 0)
			return new Polynomial();
		Polynomial result = new Polynomial(this);
		Literal ptr = result.head;
		while (ptr != null) {
			ptr.coefficient *= rhs.coefficient;
			ptr.exponent += rhs.exponent;
			ptr = ptr.next;
		}
		return result;
	}

	public String toString() {
		// using “^” to signify exponents
		StringBuilder sb = new StringBuilder();
		Literal l = head;
		String prefix = "";
		while (l != null) {
			sb.append(prefix);
			if (l.exponent != 0) {
				if (l.coefficient != 1)
					sb.append(l.coefficient);
				sb.append('x');
				if (l.exponent != 1) {
					sb.append('^');
					sb.append(l.exponent);
				}
			} else {
				sb.append(l.coefficient);
			}
			prefix = " + ";
			l = l.next;
		}
		return sb.toString();
	}
}