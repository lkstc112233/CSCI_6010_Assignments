//
//  Symbol.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef Symbol_hpp
#define Symbol_hpp

#include <string>
#include <iostream>
#include <map>
#include <cmath>

namespace Assignment2 {
    typedef __int64_t integer_type;
    typedef double floating_type;
    
    enum ESymbolType
    {
        INTEGER,
        FLOATING,
        OPERATOR,
        VARIABLE,
    };
    
    enum EOperatorType
    {
        // Controlling Symbols
        NOTMATCH = 0, // Indicates if something is not an operator.
        NEWLINE,
        QUIT, // Quits main loop somehow.
        
        LEFT_BRACKET,
        RIGHT_BRACKET,
        
        DOT,                // I have no idea how to use this symbol yet.
        
        ASSIGNMENT = 0x100, // =
        
        ADD = 0x200,        // +
        SUB,                // -
        
        MULTIPLY = 0x400,   // *
        DIVISION,           // /
        MOD,                // %
        
        POWER = 0x600,      // ^
        
        FLOATING_FUNCTION_CALL_BASE = 0x700,
        FUNCTION_SIN,
        FUNCTION_COS,
        FUNCTION_TAN,
        FUNCTION_ASIN,
        FUNCTION_ACOS,
        FUNCTION_ATAN,
        FUNCTION_LOG,
        FUNCTION_LOG2,
        FUNCTION_LN,
        
        INTEGER_FUNCTION_CALL_BASE = 0x780,
        FUNCTION_ABS,
        
        PI = 0x800,
        E,
    };
    
    struct SAdditionInformation
    {
        SAdditionInformation();
        SAdditionInformation(EOperatorType ot);
        SAdditionInformation(integer_type ot);
        SAdditionInformation(std::string ot);
        
        integer_type m_integer;
        floating_type m_floating;
        EOperatorType m_operator;
        std::string m_variable;     // Variable Name
    };
    
    class CSymbol
    {
    private:
        ESymbolType type;
        SAdditionInformation information;
    public:
        ESymbolType getType() const {return type;}
        const SAdditionInformation& getAdditionalInformation() const {return information;}
        void setSymbol(ESymbolType typeIn, SAdditionInformation informationIn) { type = typeIn; information = informationIn;}
        void setSymbol(integer_type valueIn){type = INTEGER; information.m_integer = valueIn;}
        void setSymbol(floating_type valueIn){type = FLOATING; information.m_floating = valueIn;}
        integer_type toInteger() const;
        floating_type toFloating() const;
        CSymbol getRValue(const std::map<std::string,CSymbol>&) const;
        CSymbol getLValue() const;
    };
    
    std::ostream& operator<<(std::ostream&, const CSymbol& in);
    
}


#endif /* Symbol_hpp */
