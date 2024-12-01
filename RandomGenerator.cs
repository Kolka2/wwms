﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal class RandomGenerator
    {
        Random r = new Random();
        public int _NumProducts;
        public int _MinimalCountProducts;
        public int _MinQ;
        public int _MaxQ;
     public RandomGenerator()
        { }
        public RandomGenerator (int CountProducts)
        {
            _NumProducts = CountProducts;
            _MinimalCountProducts = 1;
        }
        public RandomGenerator(int CountProducts,int MinimalProducts,int minQuantity,int maxQuantity) { 
            _MinQ = minQuantity;
            _MaxQ = maxQuantity;
            _NumProducts = CountProducts;
            _MinimalCountProducts = MinimalProducts;
        }
        public int NextInt(int a,int b) { return r.Next(a,b); }
        public double NextDouble(double a,double b) {
            double minValue=a; double maxValue=b;
            double randomDouble = minValue + (r.NextDouble() * (maxValue - minValue));
            randomDouble=Math.Round(randomDouble,2);
            return randomDouble;
        }

        public List<Product> GetListProducts(List<Product> products)
        {
            //Подумать над логикой
            if (_MinimalCountProducts > products.Count)
            {
                throw new ArgumentException("n не может быть больше размера списка.");
            }

            Random random = new Random();
            List<Product> shuffledList = products.OrderBy(x => random.Next()).ToList();

            return shuffledList.Take(_MinimalCountProducts).ToList();
        
            
        }

        public int GetNumProducts()
        {
            return r.Next(_MinimalCountProducts,_NumProducts+1);
        }

        public double GenQuantity()
        {
            double minValue = _MinQ; double maxValue = _MaxQ;
            double randomDouble = minValue + (r.NextDouble() * (maxValue - minValue));
            randomDouble = Math.Round(randomDouble, 2);
            return randomDouble;
        }

    }
}