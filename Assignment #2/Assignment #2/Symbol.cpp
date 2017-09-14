//
//  Symbol.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "Symbol.hpp"
#include <memory>
#include "Exceptions.hpp"

namespace Assignment2 {
    
    SAdditionInformation::SAdditionInformation() {}
    SAdditionInformation::SAdditionInformation(EOperatorType ot) { m_operator = ot; }
    SAdditionInformation::SAdditionInformation(integer_type integer) { m_integer = integer; }
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
                    case POWER:
                        ost << "^";
                        break;
                    case MOD:
                        ost << "%";
                        break;
                    case ASSIGNMENT:
                        ost << "=";
                        break;
                    case NEWLINE:
                        ost << std::endl;
                        break;
                    case FUNCTION_SIN:
                        ost << "sin";
                        break;
                    case FUNCTION_COS:
                        ost << "cos";
                        break;
                    case FUNCTION_TAN:
                        ost << "tan";
                        break;
                    case FUNCTION_ASIN:
                        ost << "asin";
                        break;
                    case FUNCTION_ACOS:
                        ost << "acos";
                        break;
                    case FUNCTION_ATAN:
                        ost << "atan";
                        break;
                    case FUNCTION_LOG:
                        ost << "log";
                        break;
                    case FUNCTION_LOG2:
                        ost << "log2";
                        break;
                    case FUNCTION_LN:
                        ost << "ln";
                        break;
                    case FUNCTION_ABS:
                        ost << "abs";
                        break;
                    case PI:
                        ost << "pi";
                        break;
                    case E:
                        ost << "e";
                        break;
                    default:
                        break;
                }
        }
        return ost;
    }
    
    integer_type CSymbol::toInteger() const
    {
        switch (getType()) {
            case INTEGER:
                return getAdditionalInformation().m_integer;
            case FLOATING:
                return getAdditionalInformation().m_floating;
            case OPERATOR:
            case VARIABLE:
            default:
                return 0;
        }
    }
    
    floating_type CSymbol::toFloating() const
    {
        switch (getType()) {
            case INTEGER:
                return getAdditionalInformation().m_integer;
            case FLOATING:
                return getAdditionalInformation().m_floating;
            case OPERATOR:
            case VARIABLE:
            default:
                return 0;
        }
    }
    
    CSymbol CSymbol::getRValue(const std::map<std::string,CSymbol>& data) const
    {
        if (getType() != VARIABLE)
            return *this;
        auto i = data.find(getAdditionalInformation().m_variable);
        if (i != data.end())
            return i->second.getRValue(data);
        throw IdentifierNotFoundException();
    }
    CSymbol CSymbol::getLValue() const
    {
        if (getType() != VARIABLE)
            throw NotValidLeftValueException();
        return *this;
    }
}
