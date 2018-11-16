#include "stdafx.h"
#include "MathUtils.h"

using namespace Dll2;

MathUtils::MathUtils()
{
}

MathUtils::~MathUtils()
{
}

double MathUtils::Add(double a, double b)
{
	return a + b;
	//return Lib2::MathUtils::Add(a, b);
}

double MathUtils::Subtract(double a, double b)
{
	return a - b;
	//return Lib2::MathUtils::Subtract(a, b);
}

double MathUtils::Multiply(double a, double b)
{
	return a * b;
	//return Lib2::MathUtils::Multiply(a, b);
}

double MathUtils::Divide(double a, double b)
{
	return a / b;
	//return Lib2::MathUtils::Divide(a, b);
}

