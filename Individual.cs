using System.Security.Cryptography;

namespace Country_method;

public class Individual
{
    private Guid _id;

    public Guid Id
    {
        get => _id;
    }

    private double _x;

    private int _epidemicSurvived;

    public int EpidemicSurvived
    {
        get => _epidemicSurvived;
        set => _epidemicSurvived = value;
    }

    public Individual()
    {
        _id = Guid.NewGuid();
        _x = GetRandomDouble(-512, 512);
        _y = GetRandomDouble(-512, 512);
        CalculateFunc();
        _epidemicSurvived = 0;
    }

    public Individual(Individual father, Individual mother,
        double pMax, double pMin,
        double fMax, double fMin, double fAvg,
        int tMax, int tCurrent)
    {
        _id = Guid.NewGuid();
        _x = father.x;
        _y = mother.y;

        double p = ((pMax - pMin) * (1 - tCurrent / tMax) * (fAvg - fMin) / (fMax - fMin)) + pMin;

        if (Double.IsNaN(p))
        {
            Console.WriteLine("Pizdec");
        }

        _x = father.x + GetRandomDouble(-p * _x, p * _x);
        if (_x < -512)
        {
            _x = -512;
        }
        if (_x > 512)
        {
            _x = 512;
        }
        _y = mother.y + GetRandomDouble(-p * _y, p * _y);
        if (_y < -512)
        {
            _y = -512;
        }
        if (_y > 512)
        {
            _y = 512;
        }

        if (double.IsNaN(_x))
        {
            _x = father.x + GetRandomDouble(-p * _x, p * _x);
        }

        
        CalculateFunc();
        _epidemicSurvived = 0;
    }
    
    public double x
    {
        get => _x;
        set
        { 
            _x = value;
            CalculateFunc();
        }
    }


    private double _y;
    public double y
    {
        get => _y;
        set
        {
            _y = value;
            CalculateFunc();
        }
    }
    
    private double _f;

    public double f
    {
        get => _f;
    }
    
    public void MoveToLeader(Individual leader)
    {
        var rand = new Random();

        _x = this._x + rand.NextDouble() * 2 * (leader.x - _x);
        _y = this._y + rand.NextDouble() * 2 * (leader.y - _y);

        if (_x < -512)
        {
            _x = -512;
        }
        if (_x > 512)
        {
            _x = 512;
        }
        if (_y < -512)
        {
            _y = -512;
        }
        if (_y > 512)
        {
            _y = 512;
        }
        
        CalculateFunc();
    }

    public void FallIll(double pMax)
    {
        if (_epidemicSurvived != 0)
        {
            _x = pMax * GetRandomDouble(-1 * _x, _x)/_epidemicSurvived;
            _y = pMax * GetRandomDouble(-1 * _y, _y)/_epidemicSurvived;
        }
        else
        {
            _x = pMax * GetRandomDouble(-1 * _x, _x);
            _y = pMax * GetRandomDouble(-1 * _y, _y);
        }
        if (_x < -512)
        {
            _x = -512;
        }
        if (_x > 512)
        {
            _x = 512;
        }
        if (_y < -512)
        {
            _y = -512;
        }
        if (_y > 512)
        {
            _y = 512;
        }
        
        _epidemicSurvived++;
    }
    
    
    private double GetRandomDouble(double lowerBound, double upperBound)
    {
        var rand = new Random();
        var rDouble = rand.NextDouble();
        
        var rMod = rDouble * (upperBound - lowerBound) + lowerBound;
        
        {
            
        }
        return rMod;
    }
    
    public void CalculateFunc()
    {
        _f = -(_y + 47) * Math.Sin(Math.Sqrt(Math.Abs(_y + _x / 2 + 47))) - _x * Math.Sin(Math.Sqrt(Math.Abs(_x - (_y + 47))));
    }
}