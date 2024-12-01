using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simul
{
    internal class WholesalePackage:Product,IComparable<WholesalePackage>
    {
        public int PackageCount; //Количество упаковок в одной оптовой упаковке
        public int TempPackageCount;//Текущее количество
        public double DiscountPrice;
        public int WasteDay;
        public bool IsDicounted => DiscountPrice < base.Price;

        public WholesalePackage(Product p,int PackageCount,int WasteDay)
        {
            this.PackageCount=PackageCount;
            base.Name = p.Name;
            base.Quantity = p.Quantity;
            base.ExpiryDays = p.ExpiryDays;
            base.Price = p.Price;
            base.MinWholesalePackages = p.MinWholesalePackages;
            DiscountPrice = p.Price;
            this.WasteDay = WasteDay;
            
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
