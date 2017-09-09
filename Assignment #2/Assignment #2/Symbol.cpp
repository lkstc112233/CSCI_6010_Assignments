//
//  Symbol.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include "Symbol.hpp"
#include <memory>

namespace Assignment2 {
    
    UAdditionInformation::UAdditionInformation() { memset(this, 0, sizeof(UAdditionInformation)); }
    UAdditionInformation::UAdditionInformation(EOperatorType ot) { m_operator = ot; }
    UAdditionInformation::UAdditionInformation(__int64_t integer) { m_integer = integer; }
}
