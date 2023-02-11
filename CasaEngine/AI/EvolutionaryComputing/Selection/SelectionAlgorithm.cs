using CasaEngine.AI.EvolutionaryComputing.Crossover;
using CasaEngine.AI.EvolutionaryComputing.Scaling;



namespace CasaEngine.AI.EvolutionaryComputing.Selection
{
    public abstract class SelectionAlgorithm<T>
    {

        protected internal int NumberParents;

        protected internal Random Generator;

        protected internal EvolutionObjective Objective;

        protected internal CrossoverMethod<T> Crossover;

        protected internal ScalingMethod<T> Scaling;

        protected internal Population<T> Population;

        protected internal ScalingMapping<T> ScaledPopulation;



        public SelectionAlgorithm(int numberParents, Random generator, EvolutionObjective objective, CrossoverMethod<T> crossover, ScalingMethod<T> scaling)
        {
            String message = String.Empty;

            if (ValidateNumberParents(numberParents, ref message) == false)
                throw new AiException("numberParents", this.GetType().ToString(), message);

            if (ValidateGenerator(generator, ref message) == false)
                throw new AiException("generator", this.GetType().ToString(), message);

            if (ValidateCrossover(crossover, ref message) == false)
                throw new AiException("crossover", this.GetType().ToString(), message);

            this.NumberParents = numberParents;
            this.Generator = generator;
            this.Objective = objective;
            this.Crossover = crossover;
            this.Scaling = scaling;
        }



        public Population<T> Selection(Population<T> population, int offspringPopulationSize)
        {
            Population<T> offsprings;
            List<Chromosome<T>> selected;

            //Save the parents
            this.Population = population;

            offsprings = population.FastEmptyInstance();

            //Test if scaling is used
            if (Scaling != null)
                ScaledPopulation = Scaling(population);

            //Generate the offsprings
            while (offsprings.Genome.Count < offspringPopulationSize)
            {
                selected = Select();
                offsprings.Genome.AddRange(Crossover(selected));
            }

            //Eliminate the excess offsprings
            if (offsprings.Genome.Count > offspringPopulationSize)
            {
                while (offsprings.Genome.Count > offspringPopulationSize)
                    offsprings.Genome.RemoveAt(offsprings.Genome.Count - 1);
            }

            return offsprings;
        }

        protected abstract List<Chromosome<T>> Select();



        public bool ValidateNumberParents(int numberParents, ref String message)
        {
            if (numberParents < 2)
            {
                message = "The number of selected parents must be greater than 1.";
                return false;
            }

            return true;
        }

        public static bool ValidateGenerator(Random generator, ref String message)
        {
            if (generator == null)
            {
                message = "The random number generator can�t be null.";
                return false;
            }

            return true;
        }

        public static bool ValidateCrossover(CrossoverMethod<T> crossover, ref string message)
        {
            if (crossover == null)
            {
                message = "The crossover operator can�t be null";
                return false;
            }

            return true;
        }

    }
}
