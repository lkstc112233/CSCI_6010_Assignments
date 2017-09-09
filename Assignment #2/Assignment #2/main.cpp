//
//  main.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include <iostream>
#include <deque>
#include "ExpressionMachine.hpp"
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

void machineTest()
{
    Assignment2::CLexicalAnalyser engine(std::cin);
    Assignment2::CExpressionMachine machine;
    
    std::deque<Assignment2::CSymbol> inputs;
    while (true)
    {
        auto c = engine.getNextToken();
        if (c.getType() == Assignment2::OPERATOR
            && c.getAdditionalInformation().m_operator == Assignment2::NEWLINE)
        {
            for (auto c : inputs)
                cout << c << " ";
            cout << endl;
            machine.compile(inputs);
            cout << machine.getCompiledExpression() << endl;
            inputs.clear();
        }
        else
            inputs.push_back(c);
    }
}

int main(int argc, const char * argv[]) {
    // insert code here...
    
    machineTest();
    
    return 0;
}
