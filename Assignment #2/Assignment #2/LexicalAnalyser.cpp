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
        while (isdigit(inputStream.peek()))
            buffer += inputStream.get();
        std::stringstream converter;
        converter << buffer;
        __int64_t conv;
        converter >> conv;
        result.setSymbol(conv);
        return result;
    }
    
    CSymbol CLexicalAnalyser::getNextToken()
    {
        CSymbol result;
        int nextChar = inputStream.peek();
        while (isspace(nextChar))
        {
            inputStream.get();
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
            default:
                if (isdigit(nextChar))
                    return formDigit();
                if (isalpha(nextChar))
                    return formString();
        }
        return result;
    }
}
