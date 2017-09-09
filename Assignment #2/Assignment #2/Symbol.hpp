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
        // Controlling Symbols
        NOTMATCH = 0, // Indicates if something is not an operator.
        NEWLINE,
        QUIT, // Quits main loop somehow.
        
        LEFT_BRACKET,
        RIGHT_BRACKET,
        
        ADD = 0x100, // A + B
        SUB, // A - B
        
        MULTIPLY = 0x200,
        DIVISION,
        
    };
    
    struct SAdditionInformation
    {
        SAdditionInformation();
        SAdditionInformation(EOperatorType ot);
        SAdditionInformation(__int64_t ot);
        SAdditionInformation(std::string ot);
        
        __int64_t m_integer;
        double m_floating;
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
        void setSymbol(__int64_t valueIn){type = INTEGER; information.m_integer = valueIn;}
        void setSymbol(double valueIn){type = FLOATING; information.m_integer = valueIn;}
    };
    
    std::ostream& operator<<(std::ostream&, const CSymbol& in);
    
}


#endif /* Symbol_hpp */
