






// var result = generations.Aggregate(0, (sum, gen) => sum + gen.PopulationSize);

internal record LanternFishGeneration
{
    public Int64 PopulationSize { get; private set; }
    public int DaysUntilReproduction { get; private set; }

    public LanternFishGeneration(Int64 populationSize, int daysUntilReproduction)
    {
        PopulationSize = populationSize;
        DaysUntilReproduction = daysUntilReproduction;
    }

    public bool AdvanceDayAnd()
    {
        if (DaysUntilReproduction == 0)
        {
            DaysUntilReproduction = 6;
            return true;
        }
        DaysUntilReproduction--;
        return false;
    }

    public override string ToString()
    {
        return $"{DaysUntilReproduction}, {PopulationSize} fish";
    }
}