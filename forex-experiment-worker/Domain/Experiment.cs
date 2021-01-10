using System.Collections.Generic;
using System.Linq;
using forex_experiment_worker.Models;
namespace forex_experiment_worker.Domain
{
    
    public class ForexExperiment
    {
        private  List<SessionAnalysis> _sessions;
        public ForexExperiment()
        {
            _sessions = new List<SessionAnalysis>();
        }
        
        public string name { get; set; }
        public string indicator{get;set;}
        
        public string startdate{get;set;}
        
        public string enddate{get;set;}
        
        public string position{get;set;}

        public double startamount{get;set;}
        
        public Variable<int> window{get;set;}
       
        public Variable<int> units{get;set;}
       
        public Variable<double> stoploss{get;set;}
        public Variable<double> takeprofit{get;set;}
        public string percentcomplete{get;set;}
        public bool complete{get;set;}
        public string starttime{get;set;}
        public string endtime{get;set;}
        public string elapsedtime{get;set;}
        public List<SessionAnalysis> sessions
        {
           
            get
            {
                return _sessions;
            }

            set
            {
                _sessions = value;    
            }
        }

        public List<Strategy> GetStrategies()
        {
            List<Variable> variables = new List<Variable>();
            window.name="window";
            stoploss.name ="stoploss";
            takeprofit.name ="takeprofit";
            units.name ="units";
            
            Variable<string> position = new Variable<string>();
            position.name="position";
            position.staticOptions= new string[]{ this.position};

            Variable<string> rulename = new Variable<string>();
            rulename.name="rulename";
            rulename.staticOptions= new string[]{indicator};

            variables.Add(window);
            variables.Add(stoploss);
            variables.Add(takeprofit);
            variables.Add(units);
            variables.Add(position);
            variables.Add(rulename);
            

            return GetStrategyHelper(variables);
        }

        public List<Strategy> GetStrategyHelper(List<Variable> variables)
        {
            if(variables.Count==1)
            {
                return variables[0].CartesianProduct(new List<Strategy>());
            }
            else
            {
                return variables[0].CartesianProduct(GetStrategyHelper(variables.Skip(1).ToList()));
            }
        }

    }
}