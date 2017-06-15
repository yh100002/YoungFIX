using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    public class SymbolFormula: SymbolBase
    {
        private List<SymbolBase> allSymbols;
        private Dictionary<SymbolBase, Quote> lastQuotes = new Dictionary<SymbolBase, Quote>();
        private List<Object> postfixList = new List<Object>();

        // For testing only
        public String PostfixFormula
        {
            get {
                String res = "";
                foreach (var elem in postfixList.ToArray())
                {
                    res += elem.ToString();
                }

                return res;
            }
        }

        private string formula;

        public SymbolFormula(String exanteId, TimeSpan sessionStart, TimeSpan sessionEnd, String formula, List<SymbolBase> allSymbols, ExecutionType executionType)
            : base(exanteId, sessionStart, sessionEnd, executionType)
        {
            this.allSymbols = allSymbols;
            this.formula = formula;
            ParseFormula();
        }

        private int Priority(String st)
        {
            switch (st)
            {
                case "*":
                case "/": { return 2; }
                case "+":
                case "-": { return 1; }
                default: { return 0; }
            }
        }

        private SymbolBase ExanteId2Symbol(String ExanteId)
        {
            foreach (var symbol in allSymbols)
            {
                if (symbol.EXANTEId.CompareTo(ExanteId) == 0)
                    return symbol;
            }
            throw new Exception ("no symbol " + ExanteId);
        }

        private Double tryToParseConstant(String input)
        {
            Double result = 0;
            Double.TryParse(input, out result);
            return result;
        }

        private bool IsSymbol(Object obj)
        {
            return obj.GetType().BaseType.Equals(typeof(SymbolBase)) || obj.GetType().Equals(typeof(SymbolBase));
        }

        public void ParseFormula()
        {
            String infixFormula = this.Formula;
            String [] delimeters = { "+", "-", "(", ")", "*", "/" };
            foreach (var delimeter in delimeters)
            {
                infixFormula = infixFormula.Replace(delimeter, "|" + delimeter + "|");
            }

            List<String> infixStringArray = infixFormula.Split('|').ToList();
            List<Object> infixArray = new List<Object>();

            for (int i = 0; i <= infixStringArray.Count - 1; i++)
            {
                String element = infixStringArray[i].Trim(' ');
                if (element.Length == 0) continue;
                if (delimeters.Contains(element))
                {// operand
                    infixArray.Add(element);
                }
                else
                {
                    Double constant = tryToParseConstant(element);
                    if (constant > 0)
                    {// Constant
                        infixArray.Add(constant);

                    }
                    else
                    {// Symbol
                        SymbolBase smb = ExanteId2Symbol(element);
                        lastQuotes[smb] = new Quote(smb, null, null, DateTime.MinValue);
                        infixArray.Add(smb);
                    }
                }
            }

            var stack = new Stack<Object>();
            var postfix = new Stack<Object>();

            Object obj;
            for (int i = 0; i < infixArray.Count; i++)
            {
                var currentElement = infixArray[i];
                if (IsSymbol (currentElement))
                {// Symbol
                    postfix.Push(currentElement);
                }
                else if (delimeters.Contains(currentElement.ToString()))
                {
                    //"()*/+-"
                    String operand = currentElement.ToString();
                    if (operand.Equals("("))
                    {
                        stack.Push("(");
                    }
                    else if (operand.Equals(")"))
                    {
                        obj = stack.Pop();
                        while (!(obj.Equals("(")))
                        {
                            postfix.Push(obj);
                            obj = stack.Pop();
                        }
                    }
                    else
                    {
                        while (stack.Count > 0)
                        {
                            obj = stack.Pop();
                            if (Priority(obj.ToString()) >= Priority(currentElement.ToString()))
                            {
                                postfix.Push(obj);
                            }
                            else
                            {
                                stack.Push(obj);
                                break;
                            }
                        }
                        stack.Push(currentElement);
                    }
                }
                else
                {// Constant
                    postfix.Push((Double)currentElement);
                }
            }
            while (stack.Count > 0)
            {
                postfix.Push(stack.Pop());
            }

            postfixList.Clear();
            while (postfix.Count > 0)
            {
                postfixList.Insert(0, postfix.Pop());
            }
        }


        public String Formula
        {
            get { return formula; }
        }


        public List<SymbolBase> FormulaSymbols
        {
            get { return lastQuotes.Keys.ToList(); }
        }

        public void UpdateQuote(SymbolBase symbol, Quote quote)
        {
            lastQuotes[symbol] = quote;
        }

        private Boolean CanCalculate()
        {
            foreach (var key in lastQuotes.Keys)
            {
                if (!lastQuotes[key].Ask.HasValue)
                    return false;
            }
            return true;
        }
        
        public Quote Calculate()
        {
            if (!CanCalculate())
                return null;
            DateTime timestamp = DateTime.MinValue;
            Stack<Double> ask = new Stack<Double>();
            foreach (var element in postfixList)
            {
                if (IsSymbol(element))
                {
                    Quote tempQuote = lastQuotes[element as SymbolBase];
                    ask.Push(tempQuote.Ask.Value);
                    if (timestamp.CompareTo(tempQuote.Timestamp) < 0)
                        timestamp = tempQuote.Timestamp;
                }
                else if (element.GetType().Equals(typeof(Double)))
                {
                    ask.Push((Double) element);
                }
                else
                {// Calculation
                    Double op1 = ask.Pop();
                    Double op2 = ask.Pop();
                    Double newValue = 0;
                    switch ((String)element)
                    {
                        case "+": newValue = op1 + op2; break;
                        case "-": newValue = op2 - op1; break;
                        case "*": newValue = op1 * op2; break;
                        case @"/": newValue = op2 / op1; break;
                    }
                    ask.Push(newValue);
                }
            }
            double formulaAskValue = ask.Pop();
            double formulaBidValue = formulaAskValue;
            return new Quote(this, formulaBidValue, formulaAskValue, timestamp);
        }
    }
}
