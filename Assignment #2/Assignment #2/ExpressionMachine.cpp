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
    bool morePriority(EOperatorType ot1, EOperatorType ot2)
    {
        return (ot1 >> 8) > (ot2 >> 8);
    }
    
    
    CSymbol operate(CSymbol operand1, CSymbol operand2, EOperatorType ot)
    {
        CSymbol result;
        switch (ot)
        {
            case ADD:
                // TODO: convert on floating!!!
                result.setSymbol(operand1.getAdditionalInformation().m_integer +
                                 operand2.getAdditionalInformation().m_integer);
                break;
            case SUB:
                // TODO: convert on floating!!!
                result.setSymbol(operand1.getAdditionalInformation().m_integer -
                                 operand2.getAdditionalInformation().m_integer);
                break;
            case MULTIPLY:
                // TODO: convert on floating!!!
                result.setSymbol(operand1.getAdditionalInformation().m_integer *
                                 operand2.getAdditionalInformation().m_integer);
                break;
            case DIVISION:
                // TODO: convert on floating!!!
                if (operand2.getAdditionalInformation().m_integer == 0)
                    throw CSymbol();
                result.setSymbol(operand1.getAdditionalInformation().m_integer /
                                 operand2.getAdditionalInformation().m_integer);
                break;
            default:
                throw CSymbol();
        }
        return result;
    }
    
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
                    if (symbol.getAdditionalInformation().m_operator == RIGHT_BRACKET)
                    {
                        while (stack.top().getAdditionalInformation().m_operator != LEFT_BRACKET)
                            compiledExpression.push_back(stack.pop());
                        stack.pop();
                    }
                    else if (symbol.getAdditionalInformation().m_operator != LEFT_BRACKET)
                        // Priority check.
                        while (!stack.empty() && !morePriority(symbol.getAdditionalInformation().m_operator,stack.top().getAdditionalInformation().m_operator))
                            compiledExpression.push_back(stack.pop());
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
        CStackForSymbol stack;
        for (CSymbol sym : compiledExpression)
        {
            CSymbol operand1;
            CSymbol operand2;
            
            switch (sym.getType()) {
                case FLOATING:
                case INTEGER:
                case VARIABLE:
                    stack.push(sym);
                    break;
                case OPERATOR:
                    switch (sym.getAdditionalInformation().m_operator) {
                        case ADD:
                        case SUB:
                        case MULTIPLY:
                        case DIVISION:
                            operand2 = stack.pop();
                            operand1 = stack.pop();
                            stack.push(operate(operand1,
                                               operand2,
                                               sym.getAdditionalInformation().m_operator)
                                       );
                            break;
                        default:
                            break;
                    }
                default:
                    break;
            }
        }
        auto result = stack.pop();
        if (!stack.empty())
            throw CSymbol();
        return result;
    }
    
}
