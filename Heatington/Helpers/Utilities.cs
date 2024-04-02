// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Heatington.Helpers;

/// <include file='../Documentation/Helpers/UtilitiesDocs.xml' path='/doc/members[@name="Utilities"]/*' />
public static class Utilities
{
    /// <include file='../Documentation/Helpers/UtilitiesDocs.xml' path='/doc/members[@name="DisplayException"]/*' />
    public static void DisplayException(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
