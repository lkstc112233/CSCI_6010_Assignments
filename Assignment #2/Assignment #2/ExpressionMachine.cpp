//
//  ExpressionMachine.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "ExpressionMachine.hpp"
#include <string>
#include <sstream>
#include <deque>

#include "SymbolStack.hpp"

namespace Assignment2 {
    void CExpressionMachine::compile(std::deque<CSymbol>& line)
    {
        if (compiledExpression.size())
            compiledExpression.clear();
        // Create a stack which holds the operators.
        CStackForSymbol stack;
        for (CSymbol symbol : line)
        {
            switch (symbol.getType()) {
                case OPERATOR:
                    while (!stack.empty())
                    {
                        //if (stack.top()) // Priority check.
                        compiledExpression.push_back(stack.pop());
                    }
                    stack.push(symbol);
                    break;
                case INTEGER:
                case FLOATING:
                case VARIABLE:
                    compiledExpression.push_back(symbol);
                    break;
                default:
                    break;
            }
        }
        while (!stack.empty())
            compiledExpression.push_back(stack.pop());
    }
    
    std::string CExpressionMachine::getCompiledExpression()
    {
        std::stringstream converter;
        for (CSymbol sym : compiledExpression)
            converter << sym << " ";
        return converter.str();
    }
    
    CSymbol CExpressionMachine::evaluateExpression()
    {
        return CSymbol();
    }
    
}
