//
//  LexicalAnalyser.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "LexicalAnalyser.hpp"

namespace Assignment2 {
    CLexicalAnalyser::CLexicalAnalyser(std::istream& ist)
        : inputStream(ist)
    {
        
    }
    
    CSymbol CLexicalAnalyser::getNextToken()
    {
        CSymbol result;
        int nextChar = inputStream.peek();
        switch (nextChar)
        {
            case EOF:
                result.setSymbol(OPERATOR, QUIT);
                break;
                
        }
        return result;
    }
}
