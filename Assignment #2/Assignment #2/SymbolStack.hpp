//
//  SymbolStack.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef SymbolStack_hpp
#define SymbolStack_hpp

/* This stack could be a template.
 * But in this assignment, the only
 * useage of the stack would be the
 * Symbol stack, so...
 */

namespace Assignment2 {
    class CSymbol;
    class CStackForSymbol
    {
    public:
        CStackForSymbol();
        ~CStackForSymbol();
        CSymbol& top();
        CSymbol pop();
        void push(const CSymbol&);
        bool empty();
    private:
        CSymbol* array;
        int max_size;
        int m_size;
        void extend();
    };
}

#endif /* SymbolStack_hpp */
