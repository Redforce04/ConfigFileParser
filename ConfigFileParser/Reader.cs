// <copyright file="Log.cs" company="Redforce04#4091">
// Copyright (c) Redforce04. All rights reserved.
// </copyright>
// -----------------------------------------
//    Solution:         ConfigFileParser
//    Project:          ConfigFileParser
//    FileName:         Reader.cs
//    Author:           Redforce04#4091
//    Revision Date:    08/11/2023 3:08 PM
//    Created Date:     08/11/2023 3:08 PM
// -----------------------------------------

using System.Linq.Expressions;

namespace ConfigFileParser;

class Reader {
    private static Thread inputThread;
    private static AutoResetEvent getInput, gotInput;
    private static string input;

    static Reader() {
        getInput = new AutoResetEvent(false);
        gotInput = new AutoResetEvent(false);
        inputThread = new Thread(reader);
        inputThread.IsBackground = true;
        inputThread.Start();
    }

    private static void reader() {
        try
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }
        catch (ThreadInterruptedException)
        {
            
        }
    }

    // omit the parameter to read a line without a timeout
    private static string ReadLine(int timeOutMillisecs = Timeout.Infinite) {
        getInput.Set();
        bool success = gotInput.WaitOne(timeOutMillisecs);
        if (success)
            return input;
        else
        {
            
            throw new TimeoutException("User did not provide input within the timelimit.");
        }
    }
}