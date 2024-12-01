﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simul
{
    internal interface IOrder
    {
         Dictionary<Product, int> Items { get; set ; }  //Список продуктов в заказе

        void AddItem(Product product, int quantity); //Добавить продукт в заказ


        void UpdateItemQuantity(Product product, int quantity); //Изменить количество продуктов в заказе


        void RemoveItem(Product product); //Удалить продукт
       
    }
}