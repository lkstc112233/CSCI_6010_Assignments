/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.muststudio.assignment3;

/**
 *
 * @author Kevin
*/
public class Matrix {
    private final double SMALL_VALUE = 1e-200;
    private double[][] mat;
    public Matrix(int size) {
        int i = 0;
        mat = new double[size][size + 1];
    }
    // Switch two rows.
    public void elementaryRowOperations(int row_id, int row_id2) {
        double[] temp = mat[row_id];
        mat[row_id] = mat[row_id2];
        mat[row_id2] = temp;
    }
    // Multiple one row.
    public void elementaryRowOperations(int row_id, double raito) {
        for (int i = 0; i < mat[row_id].length; ++i)
            mat[row_id][i] *= raito;
    }
    // D += S * R
    public void elementaryRowOperations(int destination, double raito, int source) {
        for (int i = 0; i < mat[destination].length; ++i)
            mat[destination][i] += mat[source][i] * raito;
    }
    public boolean pivot(int row_id, int column_id)
    {
        if (Math.abs(mat[row_id][column_id]) < SMALL_VALUE)
            return false;
        double raito = 1 / mat[row_id][column_id];
        for (int i = 0; i < mat.length; ++i)
            if (i != row_id)
                elementaryRowOperations(i, -raito*mat[i][column_id], row_id);
        elementaryRowOperations(row_id, raito);
        return true;
    }
    
    public double getAt(int i, int j) {
        i %= mat.length;
        j %= mat[0].length;
        return mat[i][j];
    }
    public void setAt(int i, int j, double value) {
        i %= mat.length;
        j %= mat[0].length;
        mat[i][j] = value;
    }
    public int size() {
        return mat.length;
    }
};