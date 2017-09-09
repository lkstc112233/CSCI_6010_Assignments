//
//  main.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include <iostream>
#include "LexicalAnalyser.hpp"

using std::cout;
using std::endl;

void engineTest()
{
    Assignment2::CLexicalAnalyser engine(std::cin);
    
    while (true)
    {
        auto c = engine.getNextToken();
        switch (c.getType()) {
            case Assignment2::OPERATOR:
                cout << "Operator: " << c << endl;
                break;
            case Assignment2::INTEGER:
                cout << "Integer: " << c << endl;
                break;
            default:
                break;
        }
    }
}

int main(int argc, const char * argv[]) {
    // insert code here...
    
    engineTest();
    
    return 0;
}
