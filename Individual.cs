using System.Net;
using System.Security.Cryptography;

namespace Country_method;

public class Individual
{
    private Guid _id;

    public Guid Id
    {
        get => _id;
    }

    private List<double> _variables = new List<double>();

    private int _amountVar;

    public delegate void FuncUsedEventHandler(int didHappen);

    public event FuncUsedEventHandler FuncUsedEvent; 

    public int AmountVar
    {
        get => _amountVar;
        set => _amountVar = value;
    }

    public List<double> Variables
    {
        get => _variables;
        set => _variables = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double getVar(int index)
    {
        return _variables[index];
    }

    private int _epidemicSurvived;

    public int EpidemicSurvived
    {
        get => _epidemicSurvived;
        set => _epidemicSurvived = value;
    }
    
    
    public Individual(int amountVar)
    {
        _id = Guid.NewGuid();
        for (int i = 0; i < amountVar; i++)
        {
            _variables.Add(GetRandomDouble(-512, 512));
        }

        _amountVar = amountVar;
        CalculateFunc();
        _epidemicSurvived = 0;
    }

    public Individual(Individual father, Individual mother,
        double pMax, double pMin,
        double fMax, double fMin, double fAvg,
        int tMax, int tCurrent)
    {
        _id = Guid.NewGuid();
        
        
        double p = ((pMax - pMin) * (1 - tCurrent / tMax) * (fAvg - fMin) / (fMax - fMin)) + pMin;

        _amountVar = father.AmountVar;
        for (int i = 0; i < _amountVar; i++)
        {
            if (i % 2 == 0) 
            {
                _variables.Add(father.getVar(i));
                _variables[i] = father.getVar(i) + GetRandomDouble(-p * _variables[i], p * _variables[i]);
            }
            else
            {
                _variables.Add(mother.getVar(i));
                _variables[i] = mother.getVar(i) + GetRandomDouble(-p * _variables[i], p * _variables[i]);
            }

            if (_variables[i] > 512)
            {
                _variables[i] = 512;
            } else if (_variables[i] < -512)
            {
                _variables[i] = -512;
            }
        }
        
        CalculateFunc();
        _epidemicSurvived = 0;
    }
    
    private double _f;

    public double f
    {
        get => _f;
    }
    
    public void MoveToLeader(Individual leader)
    {
        var rand = new Random();

        for (int i = 0; i < _amountVar; i++)
        {
            _variables[i] = _variables[i] + rand.NextDouble() * 2 * (leader.getVar(i) - _variables[i]);
            
            if (_variables[i] > 512)
            {
                _variables[i] = 512;
            } else if (_variables[i] < -512)
            {
                _variables[i] = -512;
            }
        }

        CalculateFunc();
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pMax"></param>
    public void FallIll(double pMax)
    {

        for (int i = 0; i < _amountVar; i++)
        {
            if (_epidemicSurvived != 0)
            {
                _variables[i]= pMax * GetRandomDouble(-1 * _variables[i], _variables[i])/_epidemicSurvived;
            }
            else
            {
                _variables[i] = pMax * GetRandomDouble(-1 * _variables[i], _variables[i]);
            }
            
            if (_variables[i] > 512)
            {
                _variables[i] = 512;
            } else if (_variables[i] < -512)
            {
                _variables[i] = -512;
            }
        }
        CalculateFunc();
        _epidemicSurvived++;
    }
    
    
    private double GetRandomDouble(double lowerBound, double upperBound)
    {
        var rand = new Random();
        var rDouble = rand.NextDouble();
        
        var rMod = rDouble * (upperBound - lowerBound) + lowerBound;
        return rMod;
    }
    
    public void CalculateFunc()
    {
        _f = 0;
        for (int i = 0; i < _variables.Count - 1; i++)
        {
            _f += -( _variables[i + 1] + 47) * Math.Sin(Math.Sqrt(Math.Abs( _variables[i + 1] + _variables[i] / 2 + 47))) - _variables[i] * Math.Sin(Math.Sqrt(Math.Abs(_variables[i] - (_variables[i + 1] + 47))));
        }
        FuncUsedEvent?.Invoke(1);
    }
    
    
    
}