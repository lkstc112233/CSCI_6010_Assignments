//
//  LexicalAnalyser.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef LexicalAnalyser_hpp
#define LexicalAnalyser_hpp

#include <stdio.h>
#include <iostream>
#include "Symbol.hpp"

namespace Assignment2 {
    class CLexicalAnalyser
    {
    private:
        std::istream& inputStream;
    public:
        CLexicalAnalyser(std::istream& ist);
        CSymbol getNextToken();
    };
}

#endif /* LexicalAnalyser_hpp */
