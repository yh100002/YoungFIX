using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoungServices
{
    public enum ExecutionType {MARKET, LIMIT};
    public class SymbolBase :ICloneable
    {
        // Common params
        private String exanteId;
        private String name;
        private String baseCurrency;
        private Double mpi;

       

        public String EXANTEId
        {
            get { return exanteId; }
            set { exanteId = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Double MPI
        {
            get { return this.mpi; }
        }


        private TimeSpan sessionStart;
        public TimeSpan SessionStart
        {
            get { return sessionStart; }
            set { this.sessionStart = value; }
        }

        private TimeSpan sessionEnd;
        public TimeSpan SessionEnd
        {
            get { return sessionEnd; }
            set { this.sessionEnd = value; }
        }

        public override String ToString()
        {            
            return ExpirationDate;
        }
        
        // Common Data feed params
        private String cfiCode;
        public String CFICode
        {
            get { return cfiCode; }
            set { this.cfiCode = value; }
        }

        // Common Execution params
        ExecutionType executionType;
        public ExecutionType ExecutionType
        {
            get { return executionType; }
        }

        public String Currency
        {
            get { return baseCurrency; }
        }

        public SymbolBase(String exanteId)
        {
            this.exanteId = exanteId;
        }

        private void Init1(String exanteId, TimeSpan sessionStart, TimeSpan sessionEnd, ExecutionType executionType)
        {
            this.exanteId = exanteId;
            this.sessionStart = sessionStart;
            this.sessionEnd = sessionEnd;
            this.executionType = executionType;
        }
        
        public SymbolBase(String exanteId, TimeSpan sessionStart, TimeSpan sessionEnd, ExecutionType executionType)
        {
            Init1(exanteId, sessionStart, sessionEnd, executionType);
        }


        public SymbolBase(String exanteId, TimeSpan sessionStart, TimeSpan sessionEnd, ExecutionType executionType, String baseCurrency, Double MPI)
        {
            Init1(exanteId, sessionStart, sessionEnd, executionType);
            this.baseCurrency = baseCurrency;
            this.mpi = MPI;
        }
        
        public object Clone()
        {
            return new SymbolBase(exanteId, sessionStart, sessionEnd, executionType, baseCurrency, mpi);
            throw new NotImplementedException();
        }
        public string ExpirationDate { get; set; }

        public int StrikePrice { get; set; }

        public bool IsCall { get; set; }

        public string QuotesReportName { get; set; }

        public Dictionary<Double, Double> Bids { get; set; } = new Dictionary<double, double>();

        public Dictionary<Double, Double> Asks { get; set; } = new Dictionary<double, double>();

        public Dictionary<Double, Double> Trades { get; set; } = new Dictionary<double, double>();

    }
}
