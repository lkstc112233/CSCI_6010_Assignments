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
#include <cmath>

#include "SymbolStack.hpp"
#include "Exceptions.hpp"

#define OPERATOR_SIGNATURE(name, op1, op2, implement) \
static auto name##_int = [](integer_type op1, integer_type op2)->integer_type implement ; \
static auto name##_float = [](floating_type op1, floating_type op2)->floating_type implement ; \

#define GET_APPROATE_SIGNATURE(name) if (typeid(T) == typeid(integer_type)) return name##_int; else return name##_float

namespace Assignment2 {
    bool morePriority(EOperatorType ot1, EOperatorType ot2)
    {
        return (ot1 >> 8) > (ot2 >> 8);
    }
    
    integer_type HandleInteger(integer_type int1, std::function<integer_type(integer_type)> func)
    {
        return func(int1);
    }
    
    floating_type HandleFloating(floating_type float1, std::function<floating_type(floating_type)> func)
    {
        return func(float1);
    }
    
    integer_type HandleTwoIntegers(integer_type int1, integer_type int2, std::function<integer_type(integer_type,integer_type)> func)
    {
        return func(int1, int2);
    }
    
    floating_type HandleTwoFloatings(floating_type float1, floating_type float2, std::function<floating_type(floating_type,floating_type)> func)
    {
        return func(float1, float2);
    }
    
    OPERATOR_SIGNATURE(add, op1, op2, {return op1 + op2;})
    OPERATOR_SIGNATURE(sub, op1, op2, {return op1 - op2;})
    OPERATOR_SIGNATURE(mut, op1, op2, {return op1 * op2;})
    OPERATOR_SIGNATURE(div, op1, op2, {
        if (op2 == 0)
            throw MathErrorException();
        return op1 / op2;
    })
    OPERATOR_SIGNATURE(mod, op1, op2, {
        if (static_cast<integer_type>(op2) == 0)
            throw MathErrorException();
        return static_cast<integer_type>(op1) % static_cast<integer_type>(op2);
    })
    OPERATOR_SIGNATURE(pow, op1, op2, {return pow(op1, op2);})
    
    template<typename T>
    static std::function<T(T)> getApproprateFunction(EOperatorType ot)
    {
        switch (ot)
        {
            case FUNCTION_ABS:
                return &fabs;
            case FUNCTION_SIN:
                return &sin;
            case FUNCTION_COS:
                return &cos;
            case FUNCTION_TAN:
                return &tan;
            case FUNCTION_ASIN:
                return &asin;
            case FUNCTION_ACOS:
                return &acos;
            case FUNCTION_ATAN:
                return &atan;
            case FUNCTION_LOG:
                return &log10;
            case FUNCTION_LOG2:
                return &log2;
            case FUNCTION_LN:
                return &log;
            case NEGATIVE:
                return [](T i)->T{return -i;};
            default:
                throw UnexpectedOperatorException();
        }
    }
    
    template<>
    std::function<integer_type(integer_type)> getApproprateFunction(EOperatorType ot)
    {
        switch (ot)
        {
            case FUNCTION_ABS:
                return &llabs;
            case NEGATIVE:
                return [](integer_type i)->integer_type{return -i;};
            default:
                throw UnexpectedOperatorException();
        }
    }
    
    template<typename T>
    static std::function<T(T, T)> getApproprateFunction2(EOperatorType ot)
    {
        switch (ot)
        {
            case ADD:
                GET_APPROATE_SIGNATURE(add);
            case SUB:
                GET_APPROATE_SIGNATURE(sub);
            case MULTIPLY:
                GET_APPROATE_SIGNATURE(mut);
            case DIVISION:
                GET_APPROATE_SIGNATURE(div);
            case MOD:
                GET_APPROATE_SIGNATURE(mod);
            case POWER:
                GET_APPROATE_SIGNATURE(pow);
            default:
                throw UnexpectedOperatorException();
        }
    }
    
    CSymbol operate(CSymbol operand1, CSymbol operand2, EOperatorType ot)
    {
        CSymbol result;
        if (operand1.getType() == INTEGER && operand2.getType() == INTEGER)
            result.setSymbol(HandleTwoIntegers(operand1.toInteger(), operand2.toInteger(), getApproprateFunction2<integer_type>(ot)));
        else
            result.setSymbol(HandleTwoFloatings(operand1.toFloating(), operand2.toFloating(), getApproprateFunction2<floating_type>(ot)));
        return result;
    }
    
    CSymbol operate(CSymbol operand1, EOperatorType ot)
    {
        CSymbol result;
        try{
            if (operand1.getType() == INTEGER)
            {
                result.setSymbol(HandleInteger(operand1.toInteger(), getApproprateFunction<integer_type>(ot)));
                return result;
            }
        }
        catch(UnexpectedOperatorException)
        {
            // Do nothing - Slipped to floating number.
        }
        result.setSymbol(HandleFloating(operand1.toFloating(), getApproprateFunction<floating_type>(ot)));
        return result;
    }
    
    void CExpressionMachine::compile(std::deque<CSymbol>& line)
    {
        bool operateeLast = false;
        if (compiledExpression.size())
            compiledExpression.clear();
        // Create a stack which holds the operators.
        CStackForSymbol stack;
        for (CSymbol symbol : line)
        {
            switch (symbol.getType()) {
                case OPERATOR:
                    if (symbol.getAdditionalInformation().m_operator == QUIT) throw NeedToExitException();
                    if (!operateeLast)
                        if (symbol.getAdditionalInformation().m_operator == ADD || symbol.getAdditionalInformation().m_operator == SUB)
                        {
                            if (symbol.getAdditionalInformation().m_operator == ADD)
                                symbol.setSymbol(OPERATOR, POSITIVE);
                            else
                                symbol.setSymbol(OPERATOR, NEGATIVE);
                        }
                    if (symbol.getAdditionalInformation().m_operator == RIGHT_BRACKET)
                    {
                        while (stack.top().getAdditionalInformation().m_operator != LEFT_BRACKET)
                            compiledExpression.push_back(stack.pop());
                        stack.pop();
                        operateeLast = true;
                    }
                    else
                    {
                        if (symbol.getAdditionalInformation().m_operator != LEFT_BRACKET)
                            // Priority check.
                            while (!stack.empty() && !morePriority(symbol.getAdditionalInformation().m_operator,stack.top().getAdditionalInformation().m_operator))
                            {
                                if (!morePriority(stack.top().getAdditionalInformation().m_operator, symbol.getAdditionalInformation().m_operator))
                                    if (stack.top().getAdditionalInformation().m_operator & 0x100)
                                        break;
                                compiledExpression.push_back(stack.pop());
                            }
                        stack.push(symbol);
                        operateeLast = symbol.getAdditionalInformation().m_operator > CONSTANTS;
                    }
                    break;
                case INTEGER:
                case FLOATING:
                case VARIABLE:
                    operateeLast = true;
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
                        case MOD:
                        case POWER:
                            operand2 = stack.pop().getRValue(variablesTable);
                            operand1 = stack.pop().getRValue(variablesTable);
                            stack.push(operate(operand1,
                                               operand2,
                                               sym.getAdditionalInformation().m_operator)
                                       );
                            break;
                        case ASSIGNMENT:
                            operand2 = stack.pop().getRValue(variablesTable);
                            operand1 = stack.pop().getLValue();
                            variablesTable[operand1.getAdditionalInformation().m_variable] = operand2;
                            stack.push(operand1);
                            break;
                        case FUNCTION_SIN:
                        case FUNCTION_COS:
                        case FUNCTION_TAN:
                        case FUNCTION_ASIN:
                        case FUNCTION_ACOS:
                        case FUNCTION_ATAN:
                        case FUNCTION_LOG:
                        case FUNCTION_LOG2:
                        case FUNCTION_LN:
                        case FUNCTION_ABS:
                        case NEGATIVE:
                            operand1 = stack.pop().getRValue(variablesTable);
                            stack.push(operate(operand1, sym.getAdditionalInformation().m_operator));
                            break;
                        case POSITIVE:
                            break;
                        case PI:
                            operand1.setSymbol(M_PI);
                            stack.push(operand1);
                            break;
                        case E:
                            operand1.setSymbol(M_E);
                            stack.push(operand1);
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
            throw SyntaxErrorException();
        return result.getRValue(variablesTable);
    }
    
}
