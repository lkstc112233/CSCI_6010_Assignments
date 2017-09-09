//
//  Symbol.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef Symbol_hpp
#define Symbol_hpp

#include <stdio.h>

namespace Assignment2 {
    enum ESymbolType
    {
        INTEGER,
        FLOATING,
        OPERATOR,
        VARIABLE,
    };
    
    enum EOperatorType
    {
        ADD, // A + B
        SUB, // A - B
    };
    
    union UAdditionInformation
    {
        __int64_t m_integer;
        double m_floating;
        EOperatorType m_operator;
        char* m_variable;     // Variable Name
    };
    
    class CSymbol
    {
    private:
        ESymbolType type;
        UAdditionInformation information;
    };
}


#endif /* Symbol_hpp */
