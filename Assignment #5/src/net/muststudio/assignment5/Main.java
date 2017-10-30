package net.muststudio.assignment5;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.InputMismatchException;
import java.util.Scanner;

public class Main {
	public static void main(String[] args) {
		if (args.length < 2) {
			System.err.println("usage: "
					+ new java.io.File(Main.class.getProtectionDomain().getCodeSource().getLocation().getPath())
							.getName()
					+ " Polynomial_1 Polynomial_2");
			System.exit(-1);
		}
		File arg1 = new File(args[0]);
		File arg2 = new File(args[1]);
		Polynomial poly1 = new Polynomial();
		Polynomial poly2 = new Polynomial();
		Scanner scr = null;
		try {
			scr = new Scanner(arg1);
			while (scr.hasNextDouble()) {
				double d = scr.nextDouble();
				int i = scr.nextInt();
				poly1.insertTerm(d, i);
			}
		} catch (FileNotFoundException e) {
			System.err.println("File '" + arg1.getPath() + "' Not Exist.");
			System.exit(-1);
		} catch (InputMismatchException e) {
			System.err.println("File '" + arg1.getPath() + "' Has Syntax Errors.");
			System.exit(-1);
		} finally {
			if (scr != null)
				scr.close();
			scr = null;
		}
		try {
			scr = new Scanner(arg2);
			while (scr.hasNextDouble()) {
				double d = scr.nextDouble();
				int i = scr.nextInt();
				poly2.insertTerm(d, i);
			}
			scr.close();
		} catch (FileNotFoundException e) {
			System.err.println("File '" + arg2.getPath() + "' Not Exist.");
			System.exit(-1);
		} catch (InputMismatchException e) {
			System.err.println("File '" + arg2.getPath() + "' Has Syntax Errors.");
			System.exit(-1);
		} finally {
			if (scr != null)
				scr.close();
			scr = null;
		}
		System.out.println("Polynomial 1: "+poly1.toString());
		System.out.println("Polynomial 2: "+poly2.toString());
		System.out.println("Polynomial 1 + Polynimial 2: "+poly1.add(poly2));
		System.out.println("Polynomial 1 * Polynimial 2: "+poly1.multiply(poly2));
		System.out.println("Polynomial 1 + Polynimial 1: "+poly1.add(poly1));
		System.out.println("Polynomial 2 + Polynimial 2: "+poly2.add(poly2));
		System.out.println("Polynomial 1 * Polynimial 1: "+poly1.multiply(poly1));
		System.out.println("Polynomial 2 * Polynimial 2: "+poly2.multiply(poly2));
		System.out.println("Polynomial 2 + Polynimial 1: "+poly2.add(poly1));
		System.out.println("Polynomial 2 * Polynimial 1: "+poly2.multiply(poly1));
	}
}
