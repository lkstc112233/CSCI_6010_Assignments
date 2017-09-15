//
//  LexicalAnalyser.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef LexicalAnalyser_hpp
#define LexicalAnalyser_hpp

#include <iostream>

namespace Assignment2 {
    class CSymbol;
    class CLexicalAnalyser
    {
    private:
        std::istream& inputStream;
    public:
        CLexicalAnalyser(std::istream& ist);
        CSymbol getNextToken();
    private:
        CSymbol formDigit();
        CSymbol formString();
    };
}

#endif /* LexicalAnalyser_hpp */
