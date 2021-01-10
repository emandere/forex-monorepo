using System.Collections.Generic;
namespace forex_experiment_worker.Domain
{
    public abstract class Variable
    {
        public abstract List<Strategy> CartesianProduct(List<Strategy> currentProduct);
    }
    public class Variable<T>:Variable
    {
        
        public T[] staticOptions{get;set;}
       
        public T min{get;set;}
        public T max{get;set;}
       
        public T increment{get;set;}
        public string name{get;set;}

        IEnumerable<T> options()
        { 
            
            if(staticOptions.Length > 0)
            {
                return staticOptions;
            }
            List<T> returnList = new List<T>();
            for (dynamic i = min; i < max; i += increment)
            {
                returnList.Add(i);
            } 
            return returnList;   
        }
        Strategy createStrategy(Strategy oldStrategy, dynamic currentValue)
        {
            Strategy newStrategy = new Strategy();

            newStrategy.window=oldStrategy.window;
            newStrategy.stopLoss = oldStrategy.stopLoss;
            newStrategy.takeProfit = oldStrategy.takeProfit;
            newStrategy.position =oldStrategy.position;
            newStrategy.ruleName =oldStrategy.ruleName;
            newStrategy.units = oldStrategy.units;

            switch(name)
            {
            case "window":
                newStrategy.window=currentValue;
                break;
            case "stoploss":
                newStrategy.stopLoss=currentValue;
                break;
            case "takeprofit" :
                newStrategy.takeProfit=currentValue;
                break;
            case "units":
                newStrategy.units=currentValue;
                break;
            case "rulename":
                newStrategy.ruleName = currentValue;
                break;
            case "position":
                newStrategy.position = currentValue;
                break;
            }
            return newStrategy;
        }
        public override List<Strategy> CartesianProduct(List<Strategy> currentProduct)
        {
            var returnNewList = new List<Strategy>();
            if(currentProduct.Count==0)
            {
                foreach(var currentValue in options())
                {
                    returnNewList.Add(createStrategy(new Strategy(), currentValue));
                }
              
                
            }
            else
            {
                foreach(Strategy strat in currentProduct)
                {
                    foreach(var currentValue in options())
                    {
                        returnNewList.Add(createStrategy(strat, currentValue));
                    }
                }
            }

            return returnNewList;
        }
    }
}