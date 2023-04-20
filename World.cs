namespace Country_method;

public class World
{
    private List<Zone> _society = new List<Zone>();

    private double _theWorstAverage;
    private double _theBestAverage;

    private double _pMax;
    private double _pMin;
    private int _nMax;
    private int _nMin;

     
    private int _tMax;


    private double _die;
    private double _survive;
    
    private int _mMax;
    private int _mMin;

    private int _amountExchange;
    private int _warriorAmount;
    
    

    public World(double pMax, double pMin, 
        int nMax, int nMin,
        int tMax,
        double die, double survive, 
        int mMax, int mMin, int unionAmount, int population, int countryAmount,
        int amountExchange, int warriorAmount)
    {
        _pMax = pMax;
        _pMin = pMin;
        _nMax = nMax;
        _nMin = nMin;
        _tMax = tMax;
        _die = die;
        _survive = survive;
        _mMax = mMax;
        _mMin = mMin;
        _warriorAmount = warriorAmount;
        _amountExchange = amountExchange;

        for (int i = 0; i < unionAmount; i++)
        {
            int countryPerZone = countryAmount / unionAmount;
            _society.Add(new Zone(countryPerZone, population));
        }

        _theBestAverage = _society[0].BestCountryAverage;
        _theWorstAverage = _society[0].WorstCountryAverage;
        UpdateTheBestAverage();
        UpdateTheWorstAverage();

    }

    public double StartTheSimulation()
    {
        for (int i = 0; i < _tMax; i++)
        {
            foreach (var union in _society)
            {
                union.DistributeEvents(_die, _survive, _pMax, _amountExchange, _warriorAmount);
                union.StartHorny(_pMax, _pMin, _nMax, _nMin, _theBestAverage, _theWorstAverage, _tMax, i);
                union.StartPurge(_mMax, _mMin, _theBestAverage, _theWorstAverage);
                UpdateTheBestAverage();
                UpdateTheWorstAverage();
            }
        }

        return FindBestFunc();


    }
    private void UpdateTheBestAverage()
    {
        foreach (var union in _society)
        {
            if (union.BestCountryAverage < _theBestAverage)
            {
                _theBestAverage = union.BestCountryAverage;
            }
        }
    }
    private void UpdateTheWorstAverage()
    {
        foreach (var union in _society)
        {
            if (union.WorstCountryAverage > _theWorstAverage)
            {
                _theWorstAverage = union.WorstCountryAverage;
            }
        }
    }

    private double FindBestFunc()
    {
        foreach (var union in _society)
        {
            if (union.BestCountryAverage == _theBestAverage)
            {
                return union.GetBestFunc();
            }
        }

        return 1.0;
    }
    
}