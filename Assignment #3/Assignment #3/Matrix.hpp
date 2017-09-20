//
//  Matrix.hpp
//  Assignment #3
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef Matrix_hpp
#define Matrix_hpp

#include <stdio.h>
#include <vector>

namespace Assignment3 {
    class CMatrix
    {
    private:
        std::vector<std::vector<double>> mat;
    public:
        CMatrix(size_t size);
        // Switch two rows.
        void elementaryRowOperations(size_t row_id, size_t row_id2);
        // Multiple one row.
        void elementaryRowOperations(size_t row_id, double raito);
        // D += S * R
        void elementaryRowOperations(size_t destination, double raito, size_t source);
    };
}


#endif /* Matrix_hpp */
