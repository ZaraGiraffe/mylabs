using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;

namespace naumlab1
{
    public struct Pair
    {
        public string First;
        public string Second;

        public static bool operator ==(Pair a, Pair b)
        {
            return a.First == b.First && a.Second == b.Second;
        }

        public static bool operator !=(Pair a, Pair b)
        {
            return a.First != b.First || a.Second != b.Second;
        }

        public Pair(string f, string s)
        {
            First = f;
            Second = s;
        }

        public List<Pair> FindAllVertex(string exp)
        {
            exp = Regex.Replace(exp, @"\s+", ""); ;
            Regex regex = new Regex("[A-Z]+[0-9]+");
            Regex letter = new Regex("[A-Z]+");
            Regex number = new Regex("[0-9]+");
            MatchCollection matches = regex.Matches(exp);
            List<Pair> res = new List<Pair>();
            foreach (Match m in matches)
            {
                res.Add(new Pair(letter.Match(m.Value).Value, number.Match(m.Value).Value));
            }
            return res;
        }

        public void Calculate(string exp, Dictionary<Pair, double> dct)
        {
            Expression eval = new NCalc.Expression(exp);
            foreach (var par in dct.Keys)
            {
                eval.Parameters[par.First + par.Second] = dct[par];
            }
            dct[this] = Convert.ToDouble(eval.Evaluate().ToString());
        }

    }

    public class Graph
    {
        public Dictionary<Pair, List<Pair>> Dict;
        public Dictionary<Pair, string> Exp;

        public Graph()
        {
            Dict = new Dictionary<Pair, List<Pair>>();
            Exp = new Dictionary<Pair, string>();
        }

        public void AddVertex(Pair pair)
        {
            Dict.Add(pair, new List<Pair>());
            Exp.Add(pair, "0");
        }

        public void DeleteVertex(Pair pair)
        {
            Dict.Remove(pair);
            Exp.Remove(pair);
        }
        private void _TopologicalSort(Pair pair, Dictionary<Pair, bool> dct, List<Pair> lst)
        {
            dct[pair] = true;
            foreach (var pr in Dict[pair])
            {
                if (!dct[pr]) _TopologicalSort(pr, dct, lst);
            }
            lst.Add(pair);
        }

        public List<Pair> TopologicalSort()
        {
            Dictionary<Pair, bool> dct = new Dictionary<Pair, bool>();
            foreach (var pr in Dict.Keys)
            {
                dct.Add(pr, false);
            }
            List<Pair> res = new List<Pair>();
            foreach (var pr in Dict.Keys)
            {
                if (!dct[pr])
                {
                    List<Pair> lst = new List<Pair>();
                    _TopologicalSort(pr, dct, lst);
                    foreach (var par in lst)
                    {
                        res.Add(par);
                    }
                }
            }
            return res;
        }

        public bool AddNewExpression(Pair pair, string exp)
        {
            if (exp == "") exp = "0";
            List<Pair> dop = Dict[pair];
            Dict[pair] = pair.FindAllVertex(exp);
            if (HasCycle()) Dict[pair] = dop;
            else Exp[pair] = exp;
            return Exp[pair] == exp;
        }
        private bool _HasCycle(Pair pair, Pair first, Dictionary<Pair, bool> dct)
        {
            dct[pair] = true;
            bool ch = false;
            foreach (var pr in Dict[pair])
            {
                if (pr == first) ch = true;
                else if (!dct[pr]) ch = _HasCycle(pr, first, dct) | ch;
            }
            return ch;
        }

        public bool HasCycle()
        {
            if (Dict.Count is 0) return true;
            Dictionary<Pair, bool> dct = new Dictionary<Pair, bool>();
            foreach (var pair in Dict.Keys)
            {
                dct.Add(pair, false);
            }
            bool ch = false;
            foreach (var pr in Dict.Keys)
            {
                ch = _HasCycle(pr, pr, dct) | ch;
                List<Pair> dop = new List<Pair>(dct.Keys);
                foreach (var par in dop)
                {
                    dct[par] = false;
                }
            }
            return ch;
        }

        public Dictionary<Pair, double> Eval()
        {
            List<Pair> lst = TopologicalSort();
            Dictionary<Pair, double> dct = new Dictionary<Pair, double>();
            foreach (var par in lst)
            {
                par.Calculate(Exp[par], dct);
            }
            return dct;
        }
    }
}
