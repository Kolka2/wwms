namespace backend
{
    public class WholesalePackage : Product, IComparable<WholesalePackage>
    {
        public readonly int PackageCount; // Количество упаковок в одной оптовой упаковке
        public double DiscountPrice;
        public readonly int WasteDay;
        public bool IsDiscounted => DiscountPrice < base.Price;

        public WholesalePackage(Product product, int packageCount, int wasteDay)
        {
            this.PackageCount = packageCount;
            base.Name = product.Name;
            base.Quantity = product.Quantity;
            base.ExpiryDays = product.ExpiryDays;
            base.Price = product.Price;
            base.MinWholesalePackages = product.MinWholesalePackages;
            DiscountPrice = product.Price;
            this.WasteDay = wasteDay;
        }

        public void MakeDiscount(double disc)
        {
            DiscountPrice = base.Price - base.Price * disc;
        }

        int IComparable<WholesalePackage>.CompareTo(WholesalePackage other)
        {
            if (other.Name.Equals(this.Name) && other.WasteDay.Equals(this.WasteDay)) return 0;
            return base.CompareTo(other);
        }
    }
}