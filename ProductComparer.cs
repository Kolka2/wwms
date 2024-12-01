namespace wwms
{
    internal class ProductComparer : IEqualityComparer<Product>, IComparable<Product>
    {
        public bool Equals(Product x, Product y)
        {
            // Сравниваем только по имени
            return x.Name == y.Name;
        }

        public int GetHashCode(Product obj)
        {
            // Хеш-код только по имени
            return obj.Name.GetHashCode();
        }

        int IComparable<Product>.CompareTo(Product other)
        {
            return other.Name.CompareTo(other.Name);
        }
    }
}