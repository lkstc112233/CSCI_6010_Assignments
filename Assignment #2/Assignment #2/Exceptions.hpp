//
//  Exceptions.hpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#ifndef Exceptions_hpp
#define Exceptions_hpp

namespace Assignment2 {
    class Exception{};
    
    class SyntaxErrorException : public Exception {};
    class MathErrorException : public Exception {};
    class NeedToExitException : public Exception {};
    class UnexpectedOperatorException : public Exception {};
    
}

#endif /* Exceptions_hpp */
