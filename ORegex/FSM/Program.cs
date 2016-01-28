using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Antlr4.Runtime.Atn;

namespace ORegex.FSM
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Func<int, bool> isZero = x => x == 0;
            Func<int, bool> isOne = x => x == 1;
            var Q = new List<string> { "q0", "q1" };
            var Sigma = new List<Func<int, bool>> { isZero, isOne };
            var Delta = new List<Transition<int>>
            {
                new Transition<int>("q0", isZero, "q0"),
                new Transition<int>("q0", isOne, "q1"),
                new Transition<int>("q1", isOne, "q1"),
                new Transition<int>("q1", isZero, "q0")
            };
            var Q0 = new List<string> { "q0" };
            var F = new List<string> { "q0", "q1" };
            var DFSM = new DFSM<int>(Q, Sigma, Delta, Q0, F);

            var minimizedDFSM = Minimize<int>.MinimizeDFSM(DFSM);
        }
    }
}