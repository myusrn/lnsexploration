#pragma once

namespace Dll2
{
	//class MathUtils
	//public class MathUtils sealed  // w/o ref keyword none of the publics show up in managed code reference side of things
	public ref class MathUtils sealed
	{
	private:
		// todo: move any methods here that you don't want exposed by class scope DllExport statement

	public:
		MathUtils();
		~MathUtils();

		// Returns a + b
		double Add(double a, double b);

		// Returns a - b
		double Subtract(double a, double b);

		// Returns a * b
		double Multiply(double a, double b);

		// Returns a / b
		double Divide(double a, double b);
	};
}