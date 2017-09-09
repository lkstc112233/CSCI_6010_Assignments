//
//  ExpressionMachine.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef ExpressionMachine_hpp
#define ExpressionMachine_hpp

#include <deque>
#include <string>
#include "Symbol.hpp"

namespace Assignment2 {
    class CExpressionMachine
    {
    private:
        std::deque<CSymbol> compiledExpression;
    public:
        void compile(std::deque<CSymbol>& line);
        std::string getCompiledExpression();
        CSymbol evaluateExpression();
    };
    
    
}

#endif /* ExpressionMachine_hpp */
