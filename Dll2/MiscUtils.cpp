#include "stdafx.h"
#include "MiscUtils.h"

using namespace Dll2;

MiscUtils::MiscUtils()
{
}

String^ MiscUtils::CombineIntAndString(int bar, String^ foobar)
{
	return gcnew String("you passed int = " + bar + " and string = " + foobar);
}
