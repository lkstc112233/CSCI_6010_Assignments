//
//  Matrix.cpp
//  Assignment #3
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "Matrix.hpp"


namespace Assignment3 {
    CMatrix::CMatrix(size_t size)
    {
        for (size_t i = 0; i < size; ++i)
            mat.emplace_back(size + 1, 0.0);
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
}
