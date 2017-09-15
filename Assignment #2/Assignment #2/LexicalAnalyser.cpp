//
//  LexicalAnalyser.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "LexicalAnalyser.hpp"
#include <string>
#include <sstream>

#include "Symbol.hpp"

namespace Assignment2 {
    EOperatorType getOT(std::string input)
    {
        if (input == "quit")
            return QUIT;
        if (input == "exit")
            return QUIT;
        if (input == "sin")
            return FUNCTION_SIN;
        if (input == "cos")
            return FUNCTION_COS;
        if (input == "tan")
            return FUNCTION_TAN;
        if (input == "asin")
            return FUNCTION_ASIN;
        if (input == "acos")
            return FUNCTION_ACOS;
        if (input == "atan")
            return FUNCTION_ATAN;
        if (input == "log")
            return FUNCTION_LOG;
        if (input == "log2")
            return FUNCTION_LOG2;
        if (input == "ln")
            return FUNCTION_LN;
        if (input == "abs")
            return FUNCTION_ABS;
        if (input == "pi")
            return PI;
        if (input == "PI")
            return PI;
        if (input == "e")
            return E;
        if (input == "E")
            return E;
        return NOTMATCH;
    }
    
    CLexicalAnalyser::CLexicalAnalyser(std::istream& ist)
        : inputStream(ist)
    {
        
    }
    
    CSymbol CLexicalAnalyser::formDigit()
    {
        CSymbol result;
        bool isFloating = false;
        std::string buffer;
        while (isdigit(inputStream.peek()))
            buffer += inputStream.get();
        if (inputStream.peek() == '.')
        {
            isFloating = true;
            buffer += inputStream.get();
            while (isdigit(inputStream.peek()))
                buffer += inputStream.get();
        }
        if (buffer == ".")
        {
            result.setSymbol(OPERATOR, DOT);
            return result;
        }
        std::stringstream converter;
        converter << buffer;
        if (isFloating)
        {
            floating_type conv;
            converter >> conv;
            result.setSymbol(conv);
        }
        else
        {
            integer_type conv;
            converter >> conv;
            result.setSymbol(conv);
        }
        return result;
    }
    
    CSymbol CLexicalAnalyser::formString()
    {
        CSymbol result;
        std::string buffer;
        do
        {
            buffer += inputStream.get();
        }while(isalnum(inputStream.peek()));
        auto checker = getOT(buffer);
        if (checker != NOTMATCH)
            result.setSymbol(OPERATOR, checker);
        else
            result.setSymbol(VARIABLE, buffer);
        return result;
    }
    
    CSymbol CLexicalAnalyser::getNextToken()
    {
        CSymbol result;
        int nextChar = inputStream.peek();
        while (isspace(nextChar))
        {
            if (inputStream.get() == '\n')
            {
                result.setSymbol(OPERATOR, NEWLINE);
                return result;
            }
            nextChar = inputStream.peek();
        }
        switch (nextChar)
        {
            case EOF:
                result.setSymbol(OPERATOR, QUIT);
                break;
            case '=':
                result.setSymbol(OPERATOR, ASSIGNMENT);
                inputStream.get();
                break;
            case '+':
                result.setSymbol(OPERATOR, ADD);
                inputStream.get();
                break;
            case '-':
                result.setSymbol(OPERATOR, SUB);
                inputStream.get();
                break;
            case '*':
                result.setSymbol(OPERATOR, MULTIPLY);
                inputStream.get();
                break;
            case '/':
                result.setSymbol(OPERATOR, DIVISION);
                inputStream.get();
                break;
            case '%':
                result.setSymbol(OPERATOR, MOD);
                inputStream.get();
                break;
            case '^':
                result.setSymbol(OPERATOR, POWER);
                inputStream.get();
                break;
            case '(':
                result.setSymbol(OPERATOR, LEFT_BRACKET);
                inputStream.get();
                break;
            case ')':
                result.setSymbol(OPERATOR, RIGHT_BRACKET);
                inputStream.get();
                break;
            case '.':
                return formDigit();
            default:
                if (isdigit(nextChar))
                    return formDigit();
                if (isalpha(nextChar))
                    return formString();
        }
        return result;
    }
}
