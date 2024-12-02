namespace backend
{
    public class RandomGenerator
    {
        private readonly Random _r = new();
        private readonly int _minimalCountProducts;
        private readonly int _minQ;
        private readonly int _maxQ;

        public RandomGenerator(int countProducts)
        {
            _minimalCountProducts = 1;
        }

        public RandomGenerator(int countProducts, int minimalProducts, int minQuantity, int maxQuantity)
        {
            _minQ = minQuantity;
            _maxQ = maxQuantity;
            _minimalCountProducts = minimalProducts;
        }

        public int NextInt(int a, int b)
        {
            return _r.Next(a, b);
        }

        public double NextDouble(double a, double b)
        {
            double minValue = a;
            double maxValue = b;
            double randomDouble = minValue + (_r.NextDouble() * (maxValue - minValue));
            randomDouble = Math.Round(randomDouble, 2);
            return randomDouble;
        }

        public List<Product> GetListProducts(List<Product> products)
        {
            if (_minimalCountProducts > products.Count)
            {
                throw new ArgumentException("n не может быть больше размера списка.");
            }

            Random random = new();
            List<Product> shuffledList = products.OrderBy(x => random.Next()).ToList();

            return shuffledList.Take(_minimalCountProducts).ToList();
        }

        public double GenQuantity()
        {
            double minValue = _minQ;
            double maxValue = _maxQ;
            double randomDouble = minValue + (_r.NextDouble() * (maxValue - minValue));
            randomDouble = Math.Round(randomDouble, 2);
            return randomDouble;
        }
    }
}