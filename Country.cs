namespace Country_method;

public class Country
{
    private List<Individual> _citizens = new List<Individual>();
    private bool _isAlive;
    private int _population;
    private Individual _bestCitizen;

    public Individual BestCitizen
    {
        get => _bestCitizen;
    }

    private double _averageObjectiveFunc;

    public double AverageObjectiveFunc
    {
        get => _averageObjectiveFunc;
    }

    public int Population
    {
        get => _population;
    }

    public bool IsAlive
    {
        get => _isAlive;
    }
    
    public Country(int population, int amountVar)
    {
        for (int i = 0; i < population; i++)
        {
            _citizens.Add(new Individual(amountVar));
        }
        
        _population = _citizens.Count;
        _isAlive = true;

        _bestCitizen = _citizens[0];
        UpdateBestCitizen();
        
        _averageObjectiveFunc = 0;
        UpdateAverageFunc();
    }

    public Individual GetCitizen(int citizenId)
    {
        return _citizens[citizenId];
    }

    public void TakeCitizen(Guid citizenId)
    {
        foreach (var citizen in _citizens.ToList())
        {
            if (citizen.Id == citizenId)
            {
                _citizens.Remove(citizen);
            }
        }
        UpdateBestCitizen();
        UpdateAverageFunc();
        _population = _citizens.Count;
    }

    public void ReceiveCitizen(Individual citizen)
    {
        _citizens.Add(citizen);
        UpdateBestCitizen();
        UpdateAverageFunc();
        _population = _citizens.Count;
    }
    
    
    public List<Individual> GetAllCitizens()
    {
        return _citizens;
    }
    
    public void Genocide(int mMax, int mMin,
        double fMax, double fMin)
    {
        int m = (int) ((mMax - mMin) * (_averageObjectiveFunc - fMin) / (fMax - fMin) + mMin);
        

       
        if (_citizens.Count != 0)
        {
            Individual candMin = _citizens[0];
            if (m < _citizens.Count)
            {
            
                for (int i = 0; i < m; i++)
                {
                    foreach (var candidate in _citizens)
                    {
                        if (candidate.f < candMin.f)
                        {
                            candMin = candidate;
                        }
                    }
                    _citizens.Remove(candMin);
                    _population = _citizens.Count;
                }
            
            }
            else
            {
                for (int i = 0; i < _citizens.Count - 1; i++)
                {
                    foreach (var candidate in _citizens)
                    {
                        if (candidate.f < candMin.f)
                        {
                            candMin = candidate;
                        }
                    }
                    _citizens.Remove(candMin);
                    _population = _citizens.Count;
                    
                }
                _isAlive = false;
                
            }
        }
        else
        {
            _isAlive = false;
        }
        _population = _citizens.Count;
        
        UpdateBestCitizen();
        UpdateAverageFunc();
        
    }

    public void Reproduction(double pMax, double pMin, int nMax, int nMin,
        double fMax, double fMin,
        int tMax, int tCurrent)
    {
        if (_citizens.Count > 1)
        {
            double sum = 0;
            foreach (var citizen in _citizens)
            {
                citizen.CalculateFunc();
                sum += citizen.f;
            }

            _averageObjectiveFunc = sum / _citizens.Count;

            double aux = sum / _citizens.Count;
            
            int luckyPairs = (int)(((nMax - nMin) * (fMax - aux) / (fMax - fMin)) + nMin);

            while (luckyPairs * 2 > _citizens.Count)
            {
                luckyPairs--;
            }

            if (luckyPairs == 0)
            {
                luckyPairs = 1;
                if (aux < fMax)
                {
                    luckyPairs = 0;
                }
            }
            List<int> usedPairs = new List<int>();
            var rand = new Random();
            for (int i = 0; i < luckyPairs; i++)
            {
                int first = 0;
                bool isUsed = false;
                do
                {
                    isUsed = false;
                    first = rand.Next(_citizens.Count);
                    foreach (var index in usedPairs)
                    {
                        if (index == first)
                        {
                            isUsed = true;
                        }
                    }
        
                    if (usedPairs.Count == _citizens.Count)
                    {
                        isUsed = false;
                    }
                } while (isUsed);
                
                usedPairs.Add(first);
                int second = 1;
                isUsed = false;
                do
                {
                    isUsed = false;
                    second = rand.Next(_citizens.Count);
                    foreach (var index in usedPairs)
                    {
                        if (index == second)
                        {
                            isUsed = true;
                        }
                    }
        
                    if (usedPairs.Count == _citizens.Count)
                    {
                        isUsed = false;
                    }
        
                } while (isUsed);
        
                usedPairs.Add(second);
                
                if(usedPairs.Count <= _citizens.Count)
                {
                    Individual newCitizen = new Individual(_citizens[first], _citizens[second], pMax, pMin, fMax, fMin,
                        _averageObjectiveFunc, tMax, tCurrent);
                    _citizens.Add(newCitizen);
                    _population = _citizens.Count;
                }
        
        
                UpdateBestCitizen();
                UpdateAverageFunc();
            }
        }
    }

    public void Epidemic(double die, double survive, double pMax)
    {

        _citizens.OrderBy(o => o.f);
        _citizens.OrderBy(o => o.f);
        for (int i = 0; i < _citizens.Count; i++)
        {
            if (i < die * _citizens.Count)
            {
                _citizens.Remove(_citizens[i]);
            } else if (i < (1 - survive) * _citizens.Count)
            {
                _citizens[i].FallIll(pMax);
            }
            else
            {
                _citizens[i].EpidemicSurvived++;
            }
        }
        UpdateBestCitizen();
        UpdateAverageFunc();
    }

    public void MoveToLeader()
    {
        foreach (var citizen in _citizens)
        {
            if (citizen.f < _bestCitizen.f)
            {
                _bestCitizen = citizen;
            }
        }

        foreach (var citizen in _citizens)
        {
            citizen.MoveToLeader(_bestCitizen);
        }
    }
    

    private void UpdateBestCitizen()
    {
        foreach (var citizen in _citizens)
        {
            if (citizen.f < _bestCitizen.f)
            {
                _bestCitizen = citizen;
            }
        }
    }

    public void UpdateAverageFunc()
    {
        if (_citizens.Count != 0)
        {


            double sum = 0;
            foreach (var citizen in _citizens)
            {
                sum += citizen.f;
            }

            _averageObjectiveFunc = sum / _citizens.Count;

            if (Double.IsNaN(_averageObjectiveFunc))
            {
                _averageObjectiveFunc = sum / _citizens.Count;
            }
        }
        else _isAlive = false;
    }
}