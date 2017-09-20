//
//  Matrix.cpp
//  Assignment #3
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "Matrix.hpp"
#include <cmath>

namespace Assignment3 {
    CMatrix::CMatrix(size_t size)
    {
        size_t i = 0;
        do
            mat.emplace_back(size + 1, 0.0);
        while (i++ < size);
    }
    // Switch two rows.
    void CMatrix::elementaryRowOperations(size_t row_id, size_t row_id2)
    {
        auto temp = mat[row_id];
        mat[row_id] = mat[row_id2];
        mat[row_id2] = temp;
    }
    // Multiple one row.
    void CMatrix::elementaryRowOperations(size_t row_id, double raito)
    {
        for (auto& i : mat[row_id])
            i *= raito;
    }
    // D += S * R
    void CMatrix::elementaryRowOperations(size_t destination, double raito, size_t source)
    {
        for (int i = 0; i < mat[0].size(); ++i)
            mat[destination][i] += mat[source][i] *= raito;
    }
    
    bool CMatrix::pivot(size_t row_id, size_t column_id)
    {
        if (fabs(mat[row_id][column_id]) < SMALL_VALUE)
            return false;
        double raito = 1 / mat[row_id][column_id];
        for (int i = 0; i < mat.size(); ++i)
            if (i != row_id)
                elementaryRowOperations(i, raito*mat[i][column_id], row_id);
        elementaryRowOperations(row_id, raito);
        return true;
    }
    const std::vector<double>& CMatrix::operator[](size_t i) const
    {
        return mat[i];
    }
    double CMatrix::getAt(size_t i, size_t j) const
    {
        i %= mat.size();
        j %= mat[0].size();
        return mat[i][j];
    }
    void CMatrix::setAt(size_t i, size_t j, double value)
    {
        i %= mat.size();
        j %= mat[0].size();
        mat[i][j] = value;
    }
    std::ostream& operator<<(std::ostream& ost, const CMatrix &mat)
    {
        size_t size = mat[0].size() - 1;
        for (int i = 0; i < size; ++i)
        {
            for (auto d : mat[i])
                ost << d << ' ';
            ost << std::endl;
        }
        return ost;
    }
}

