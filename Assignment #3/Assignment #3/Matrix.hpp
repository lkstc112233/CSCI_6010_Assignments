//
//  Matrix.hpp
//  Assignment #3
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef Matrix_hpp
#define Matrix_hpp

#include <iostream>
#include <vector>

namespace Assignment3 {
    const double SMALL_VALUE = 1e-200;
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
        bool pivot(size_t row_id, size_t column_id);
        const std::vector<double>& operator[](size_t i) const;
        double getAt(size_t i, size_t j) const;
        void setAt(size_t i, size_t j, double value);
        size_t size() const;
    };
    
    std::ostream& operator<<(std::ostream&, const CMatrix&);
}


#endif /* Matrix_hpp */
