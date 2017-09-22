//
//  main.cpp
//  Assignment #3
//
//  Copyright © 2017 Must Studio. All rights reserved.
//


// This is a program intended to do the Gaussian Elimination calculation.

#include <iostream>
#include "Matrix.hpp"

using std::cout;

void testMat()
{
    Assignment3::CMatrix mat0(0);
    Assignment3::CMatrix mat1(1);
    Assignment3::CMatrix mat3(3);
    cout << mat0 << mat1 << "----------------" << std::endl;
    mat3.setAt(0, 0, 3);
    mat3.setAt(0, 1, -2);
    mat3.setAt(0, 2, 8);
    mat3.setAt(0, 3, 9);
    mat3.setAt(1, 0, -2);
    mat3.setAt(1, 1, 2);
    mat3.setAt(1, 2, 1);
    mat3.setAt(1, 3, 3);
    mat3.setAt(2, 0, 1);
    mat3.setAt(2, 1, 2);
    mat3.setAt(2, 2, -3);
    mat3.setAt(2, 3, 8);
    cout << mat3;
    cout << "----------------" << std::endl;
    mat3.pivot(0,0);
    cout << mat3;
    cout << "----------------" << std::endl;
    mat3.pivot(1,1);
    cout << mat3;
    cout << "----------------" << std::endl;
    mat3.pivot(2,2);
    cout << mat3;
}

int main(int argc, const char * argv[]) {
    testMat();
    return 0;
}
