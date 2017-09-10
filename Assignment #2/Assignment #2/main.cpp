//
//  main.cpp
//  Assignment #2
//
//  Copyright © 2017 Must Studio. All rights reserved.
//

#include <iostream>
#include <fstream>
#include <deque>
#include "ExpressionMachine.hpp"
#include "LexicalAnalyser.hpp"
#include "Exceptions.hpp"

using std::cout;
using std::endl;

void engineTest()
{
    Assignment2::CLexicalAnalyser engine(std::cin);
    
    while (true)
    {
        auto c = engine.getNextToken();
        switch (c.getType()) {
            case Assignment2::OPERATOR:
                cout << "Operator: " << c << endl;
                break;
            case Assignment2::INTEGER:
                cout << "Integer: " << c << endl;
                break;
            default:
                break;
        }
    }
}

void machineTest()
{
    Assignment2::CLexicalAnalyser engine(std::cin);
    Assignment2::CExpressionMachine machine;
    
    std::deque<Assignment2::CSymbol> inputs;
    while (true)
    {
        try
        {
        auto c = engine.getNextToken();
        if (c.getType() == Assignment2::OPERATOR
            && c.getAdditionalInformation().m_operator == Assignment2::NEWLINE)
        {
            for (auto c : inputs)
                cout << c;
            cout << endl;
            machine.compile(inputs);
            cout << machine.getCompiledExpression() << endl;
            cout << "Result:" << machine.evaluateExpression() << endl;
            
            inputs.clear();
        }
        else
            inputs.push_back(c);
        }
        catch(Assignment2::NeedToExitException)
        {
            break;
        }
        catch(Assignment2::SyntaxErrorException)
        {
            std::cerr << "Syntax error." << endl;
            inputs.clear();
        }
        catch(Assignment2::MathErrorException)
        {
            std::cerr << "Math error." << endl;
            inputs.clear();
        }
        catch(Assignment2::UnexpectedOperatorException)
        {
            std::cerr << "Unexpected Operation" << endl;
            inputs.clear();
        }
    }
}


int main(int argc, const char * argv[]) {
    std::ifstream file;
    bool fromFile = false;
    if (argc > 1)
    {
        fromFile = true;
        file.open(argv[1]);
    }
    
    Assignment2::CLexicalAnalyser engine(fromFile? file: std::cin);
    Assignment2::CExpressionMachine machine;
    
    std::deque<Assignment2::CSymbol> inputs;
    
    if (!fromFile)
        cout << ">> ";
    while (true)
    {
        try
        {
            auto c = engine.getNextToken();
            if (c.getType() == Assignment2::OPERATOR
                && c.getAdditionalInformation().m_operator == Assignment2::NEWLINE)
            {
                if (fromFile)
                {
                    cout << "Input: ";
                    for (auto c : inputs)
                        cout << c;
                    cout << endl;
                }
                machine.compile(inputs);
                cout << "RPN: " << machine.getCompiledExpression() << endl;
                cout << "Result: " << machine.evaluateExpression() << endl;
                inputs.clear();
                if (!fromFile)
                    cout << ">> ";
            }
            else
                inputs.push_back(c);
        }
        catch(Assignment2::NeedToExitException)
        {
            if (!fromFile)
                cout << "Thank you for using RPN calculator." << endl;
            break;
        }
        catch(Assignment2::SyntaxErrorException)
        {
            std::cerr << "Syntax error." << endl;
            inputs.clear();
            if (!fromFile)
                cout << ">> ";
        }
        catch(Assignment2::MathErrorException)
        {
            std::cerr << "Math error." << endl;
            inputs.clear();
            if (!fromFile)
                cout << ">> ";
        }
        catch(Assignment2::UnexpectedOperatorException)
        {
            std::cerr << "Unexpected Operation" << endl;
            inputs.clear();
            if (!fromFile)
                cout << ">> ";
        }
    }
    return 0;
}
