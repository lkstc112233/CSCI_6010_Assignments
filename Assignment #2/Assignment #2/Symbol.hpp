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
        
        QUIT, // Quits main loop somehow.
    };
    
    union UAdditionInformation
    {
        UAdditionInformation();
        UAdditionInformation(EOperatorType ot);
        UAdditionInformation(__int64_t ot);
        
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
    public:
        ESymbolType getType() const {return type;}
        UAdditionInformation getAdditionalInformation() const {return information;}
        void setSymbol(ESymbolType typeIn, UAdditionInformation informationIn) { type = typeIn; information = informationIn;}
        void setSymbol(__int64_t valueIn){type = INTEGER; information.m_integer = valueIn;}
        void setSymbol(double valueIn){type = FLOATING; information.m_integer = valueIn;}
    };
}


#endif /* Symbol_hpp */
