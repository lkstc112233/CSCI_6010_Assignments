//
//  Symbol.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "Symbol.hpp"
#include <memory>

namespace Assignment2 {
    
    SAdditionInformation::SAdditionInformation() {}
    SAdditionInformation::SAdditionInformation(EOperatorType ot) { m_operator = ot; }
    SAdditionInformation::SAdditionInformation(__int64_t integer) { m_integer = integer; }
    SAdditionInformation::SAdditionInformation(std::string vin) { m_variable = vin; }
    
    std::ostream& operator<<(std::ostream &ost, const CSymbol& in)
    {
        switch (in.getType())
        {
            case INTEGER:
                ost << in.getAdditionalInformation().m_integer;
                break;
            case FLOATING:
                ost << in.getAdditionalInformation().m_floating;
                break;
            case VARIABLE:
                ost << in.getAdditionalInformation().m_variable;
                break;
            case OPERATOR:
                switch (in.getAdditionalInformation().m_operator) {
                    case ADD:
                        ost << "+";
                        break;
                    case SUB:
                        ost << "-";
                        break;
                    case MULTIPLY:
                        ost << "*";
                        break;
                    case DIVISION:
                        ost << "/";
                        break;
                    case LEFT_BRACKET:
                        ost << "(";
                        break;
                    case RIGHT_BRACKET:
                        ost << ")";
                        break;
                    case NEWLINE:
                        ost << std::endl;
                        break;
                    default:
                        break;
                }
        }
        return ost;
    }
    
}
