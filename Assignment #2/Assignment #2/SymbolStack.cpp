//
//  SymbolStack.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "SymbolStack.hpp"

namespace Assignment2 {
    CStackForSymbol::CStackForSymbol()
    {
        max_size = 20;
        array = new CSymbol[max_size];
        m_size = 0;
    }
    
    CStackForSymbol::~CStackForSymbol()
    {
        delete[] array;
    }
    
    void CStackForSymbol::extend()
    {
        auto newarray = new CSymbol[max_size * 2];
        for (int i = 0; i < max_size; ++i)
            newarray[i] = array[i];
        delete[] array;
        array = newarray;
        max_size *= 2;
    }
    
    void CStackForSymbol::push(const CSymbol &toPush)
    {
        if (m_size >= max_size)
            extend();
        array[m_size++] = toPush;
    }
    
    CSymbol& CStackForSymbol::top()
    {
        if (m_size <= 0) // Prevent this from happening!
            throw CSymbol();
        return array[m_size - 1];
    }
    
    CSymbol CStackForSymbol::pop()
    {
        auto result = top();
        --m_size;
        return result;
    }
    
    bool CStackForSymbol::empty()
    {
        return m_size == 0;
    }
}
