namespace backend
{
    public class WholesalePackage : Product, IComparable<WholesalePackage>
    {
        public readonly int PackageCount; // Количество упаковок в одной оптовой упаковке
        public double DiscountPrice;
        public readonly int WasteDay;
        public bool IsDiscounted => DiscountPrice < base.Price;

        public WholesalePackage(Product p, int packageCount, int wasteDay)
        {
            this.PackageCount = packageCount;
            base.Name = p.Name;
            base.Quantity = p.Quantity;
            base.ExpiryDays = p.ExpiryDays;
            base.Price = p.Price;
            base.MinWholesalePackages = p.MinWholesalePackages;
            DiscountPrice = p.Price;
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