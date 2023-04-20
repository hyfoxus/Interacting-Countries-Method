namespace Country_method;

public class Zone
{
    private List<Country> _union = new List<Country>();

    private double _bestCountryAverage;
    private double _worstCountryAverage;
    private Country _bestCountry;

    public Country BestCountry
    {
        get => _bestCountry;
    }

    public double WorstCountryAverage
    {
        get => _worstCountryAverage;
    }

    public double BestCountryAverage
    {
        get => _bestCountryAverage;
    }

    public Zone(int zoneAmount, int countryPopulation)
    {
        for (int i = 0; i < zoneAmount; i++)
        {
            _union.Add(new Country(countryPopulation));
        }

        _bestCountryAverage = _union[0].AverageObjectiveFunc;
        _bestCountry = _union[0];
        UpdateBestAverage();
        UpdateWorstAverage();
    }
    
    
    public void DistributeEvents(double die, double survive, double pMax,
        int amountExchanger, int warriorAmount)
    {
        
        var rand = new Random();
        List<int> passedPhase = new List<int>();

        for (int i = 0; i < _union.Count && passedPhase.Count < _union.Count; i++)
        {
            if (_union[i].IsAlive)
            {
                
            
            bool wasUsed = false;
            foreach (var id in passedPhase)
            {
                if (id == i)
                {
                    wasUsed = true;
                    break;
                }
            }

            if (wasUsed)
            {
                continue;
            }
            
            switch (rand.Next(4))
            {
                case 0:
                    _union[i].MoveToLeader();
                    passedPhase.Add(i);
                    UpdateBestAverage();
                    UpdateWorstAverage();
                    break;
                case 1:
                    bool foundExchangePartner = false;
                    int exchangePartnerId;
                    if (passedPhase.Count == _union.Count - 1)
                    {
                        if (rand.Next(1) == 1)
                        {
                            goto case default;
                        }
                        else
                        {
                            goto case 0;
                        }
                    }
                    do
                    {
                        exchangePartnerId = rand.Next(_union.Count);
                        bool isPartnerFree = true;
                        foreach (var id in passedPhase)
                        {
                            if (id == exchangePartnerId || i == exchangePartnerId)
                            {
                                    isPartnerFree = false;
                            }
                        }
                        if (isPartnerFree)
                        {
                            foundExchangePartner = true;
                        }
                    } while (!foundExchangePartner);

                    _union[i] = _union[i];
                        
                    Exchange(_union[i], _union[exchangePartnerId], amountExchanger);
                        
                    passedPhase.Add(i);
                    passedPhase.Add(exchangePartnerId);
                    UpdateBestAverage();
                    UpdateWorstAverage();
                    break;
                case 2:
                    bool foundWarPartner = false;
                    int warPartnerId;
                    if (passedPhase.Count == _union.Count - 1)
                    {
                        if (rand.Next(1) == 1)
                        {
                            goto case default;
                        }
                        else
                        {
                            goto case 0;
                        }
                    }
                    do
                    {
                        warPartnerId = rand.Next(_union.Count);
                        bool isPartnerFree = true;
                        foreach (var id in passedPhase)
                        {
                            if (id == warPartnerId || i == warPartnerId)
                            {
                                isPartnerFree = false;
                            }
                        }

                        if (isPartnerFree)
                        {
                            foundWarPartner = true;
                        }

                    } while (!foundWarPartner);
                    War(_union[i], _union[warPartnerId], warriorAmount);
                        
                    passedPhase.Add(i);
                    passedPhase.Add(warPartnerId);
                    UpdateBestAverage();
                    UpdateWorstAverage();
                    break;
                default:
                    _union[i].Epidemic(die, survive, pMax);
                    passedPhase.Add(i);
                    UpdateBestAverage();
                    UpdateWorstAverage();
                    break;
            }
            continue;
            }
        }
    }
    
    public void Exchange(Country firstCountry, Country secondCountry, int exchagersAmount)
    {
        int travelersAmount = exchagersAmount;
        if (firstCountry.Population <= exchagersAmount || secondCountry.Population <= exchagersAmount)
        {
            if (firstCountry.Population < secondCountry.Population)
            {
                travelersAmount = firstCountry.Population / 2;
            }
            else
            {
                travelersAmount = secondCountry.Population / 2;
            }
        }

        var rand = new Random();
            
            List<Individual> exchangerGroup1 = new List<Individual>();
            List<Individual> exchangerGroup2 = new List<Individual>();
            for (int i = 0; i < travelersAmount; i++)
            {
                int idFirstCountry;
                bool isUnique = false;
                do
                {
                     idFirstCountry = rand.Next(firstCountry.Population);
                     bool isUsed = false;
                     foreach (var citizen in exchangerGroup1)
                     {
                         if (citizen.Id == firstCountry.GetCitizen(idFirstCountry).Id)
                         {
                             isUsed = true;
                             break;
                         }
                     }
                     if (!isUsed)
                     {
                         isUnique = true;
                     }
                } while (!isUnique);

                exchangerGroup1.Add(firstCountry.GetCitizen(idFirstCountry));
                
                isUnique = false;

                int idSecondCountry;
                do
                {
                    idSecondCountry = rand.Next(secondCountry.Population);
                    bool isUsed = false;
                    foreach (var citizen in exchangerGroup2)
                    {
                        if (citizen.Id == secondCountry.GetCitizen(idSecondCountry).Id)
                        {
                            isUsed = true;
                            break;
                        }
                    }

                    if (!isUsed)
                    {
                        isUnique = true;
                    }
                } while (!isUnique);
                
                exchangerGroup2.Add(secondCountry.GetCitizen(idSecondCountry));
            }

            foreach (var exchanger in exchangerGroup1)
            {
                secondCountry.ReceiveCitizen(exchanger);
                firstCountry.TakeCitizen(exchanger.Id);
            }
            foreach (var exchanger in exchangerGroup2)
            {
                firstCountry.ReceiveCitizen(exchanger);
                secondCountry.TakeCitizen(exchanger.Id);
            }
        
    }

    public void War(Country firstCountry, Country secondCountry, int warriorsAmmount)
    {
        int warriorsNumbers = warriorsAmmount;
        if (firstCountry.Population <= warriorsAmmount || secondCountry.Population <= warriorsAmmount)
        {
            if (firstCountry.Population < secondCountry.Population)
            {
                warriorsNumbers = firstCountry.Population;
            }
            else
            {
                warriorsNumbers = secondCountry.Population;
            }
        }

        var rand = new Random();
            
            List<Individual> warriorGroup1 = new List<Individual>();
            List<Individual> warriorGroup2 = new List<Individual>();
            for (int i = 0; i < warriorsNumbers; i++)
            {
                int idFirstCountry;
                bool isUnique = false;
                do
                {
                     idFirstCountry = rand.Next(firstCountry.Population);
                     bool isUsed = false;
                     foreach (var citizen in warriorGroup1)
                     {
                         if (citizen.Id == firstCountry.GetCitizen(idFirstCountry).Id)
                         {
                             isUsed = true;
                             break;
                         }
                     }

                     if (!isUsed)
                     {
                         isUnique = true;
                     }
                     
                } while (!isUnique);

                warriorGroup1.Add(firstCountry.GetCitizen(idFirstCountry));
                
                isUnique = false;

                int idSecondCountry;
                do
                {
                    idSecondCountry = rand.Next(secondCountry.Population);
                    bool isUsed = false;
                    foreach (var citizen in warriorGroup2)
                    {
                        
                        if (citizen.Id == secondCountry.GetCitizen(idSecondCountry).Id)
                        {
                            isUsed = true;
                            break;
                        }
                    }

                    if (!isUsed)
                    {
                        isUnique = true;
                    }
                } while (!isUnique);
                
                warriorGroup2.Add(secondCountry.GetCitizen(idSecondCountry));

                int firstGroupVistories = 0;
                int secondGroupVistories = 0;
                for (int j = 0; j < warriorsNumbers; j++)
                {
                    if (warriorGroup1[i].f < warriorGroup2[i].f)
                    {
                        firstGroupVistories++;
                        secondCountry.TakeCitizen(warriorGroup2[i].Id);
                    } else if (warriorGroup1[i].f > warriorGroup2[i].f)
                    {
                        secondGroupVistories++;
                        firstCountry.TakeCitizen(warriorGroup1[i].Id);
                    }
                }

                if (firstGroupVistories > secondGroupVistories)
                {
                    foreach (var warrior in warriorGroup2)
                    {
                        firstCountry.ReceiveCitizen(warrior);
                    }
                } else if (firstGroupVistories < secondGroupVistories)
                {
                    foreach (var warrior in warriorGroup1)
                    {
                        secondCountry.ReceiveCitizen(warrior);
                    }
                }
            }
        
    }

    public void StartPurge(int mMax, int mMin,
        double fMax, double fMin)
    {
        foreach (var country in _union.ToList())
        {
            country.Genocide(mMax, mMin, fMax, fMin);
            if (country.IsAlive == false)
            {
              _union.Remove(country);
            }
        }
        UpdateBestAverage();
        UpdateWorstAverage();
    }

    public void StartHorny(double pMax, double pMin, int nMax, int nMin,
        double fMax, double fMin,
        int tMax, int tCurrent)
    {
        UpdateBestAverage();
        UpdateWorstAverage();
        foreach (var country in _union.ToList())
        {
            if (country.Population == 1)
            {
                var rand = new Random();
                bool isChosen = false;
                do
                {
                    if (_union[rand.Next(_union.Count)] != country)
                    {
                        isChosen = true;
                    }
                } while (!isChosen);
                
                {
                    _union[rand.Next(_union.Count)].ReceiveCitizen(country.GetCitizen(0));
                    country.TakeCitizen(country.GetCitizen(0).Id);

                    _union.Remove(country);
                }
            }

            country.Reproduction(pMax, pMin, nMax, nMin, fMax, fMin, tMax, tCurrent);
        }
        UpdateBestAverage();
        UpdateWorstAverage();
    }


        public List<Country> GetAllCountries()
    {
        return _union;
    }

    private void UpdateBestAverage()
    {
        foreach (var country in _union)
        {
            if (country.AverageObjectiveFunc < _bestCountryAverage)
            {
                _bestCountryAverage = country.AverageObjectiveFunc;
                _bestCountry = country;
            }
        }
    }


    public double GetBestFunc()
    {
        return _bestCountry.BestCitizen.f;
    }
    
    private void UpdateWorstAverage()
    {
        foreach (var country in _union)
        {
            if (country.AverageObjectiveFunc > _worstCountryAverage)
            {
                _worstCountryAverage = country.AverageObjectiveFunc;
            }
        }
    }
}