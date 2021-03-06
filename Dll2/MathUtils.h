#pragma once

using namespace System;

namespace Dll2
{
	public ref class MathUtils sealed
	{
	private:
		// todo: move any methods here that you don't want exposed by class scope DllExport statement

	public:
		MathUtils();
		//~MathUtils(); // unnecessary

		// Returns a + b
		double Add(double a, double b);

		// Returns a - b
		double Subtract(double a, double b);

		// Returns a * b
		double Multiply(double a, double b);

		// Returns a / b
		double Divide(double a, double b);

		String^ StringInputAndOutputTest(String^ input);
	};
}