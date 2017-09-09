//
//  LexicalAnalyser.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "LexicalAnalyser.hpp"
#include <string>
#include <sstream>

namespace Assignment2 {
    EOperatorType getOT(std::string input)
    {
        if (input == "quit")
            return QUIT;
        if (input == "exit")
            return QUIT;
        return NOTMATCH;
    }
    
    CLexicalAnalyser::CLexicalAnalyser(std::istream& ist)
        : inputStream(ist)
    {
        
    }
    
    CSymbol CLexicalAnalyser::formDigit()
    {
        CSymbol result;
        std::string buffer;
        while (isdigit(inputStream.peek()))
            buffer += inputStream.get();
        std::stringstream converter;
        converter << buffer;
        __int64_t conv;
        converter >> conv;
        result.setSymbol(conv);
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
            case '(':
                result.setSymbol(OPERATOR, LEFT_BRACKET);
                inputStream.get();
                break;
            case ')':
                result.setSymbol(OPERATOR, RIGHT_BRACKET);
                inputStream.get();
                break;
            default:
                if (isdigit(nextChar))
                    return formDigit();
                if (isalpha(nextChar))
                    return formString();
        }
        return result;
    }
}
