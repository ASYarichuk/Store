using System;
using System.Collections.Generic;

namespace OnlineStore
{
    class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            warehouse.ShowGoods();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3);

            cart.ShowGoods();

            cart.Order();

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9);
        }
    }

    class Good
    {
        public Good(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Неправильное имя товара");
                return;
            }

            Name = name;
        }

        public string Name { get; private set; }
    }

    class Warehouse
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public void Delive(Good good, int count)
        {
            if (count < 0)
            {
                Console.WriteLine("Передано отрицательное количество товара");
                return;
            }

            if (_goods.ContainsKey(good) == false)
            {
                _goods.Add(good, count);
            }
            else
            {
                _goods[good] += count;
            }
        }

        public void ShowGoods()
        {
            var stringBuilder = new StringBuilder();

            foreach (var item in _goods)
            {
                stringBuilder.AppendFormat("{0} - {1}", item.Key.Name, item.Value);
                stringBuilder.Append("\n");
            }

            Console.WriteLine(stringBuilder);
        }
    }

        public bool CheckGoods(Good good, int count)
        {
            if (_goods.ContainsKey(good))
            {
                if (_goods[good] < count)
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public void TransferGood(Good good, int count)
        {
            _goods[good] -= count;
        }
    }

    class Cart
    {
        private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        private Shop _shop;

        public Cart(Shop shop)
        {
            _shop = shop;
        }

        public void Add(Good good, int count)
        {
            if (count < 0)
            {
                Console.WriteLine("Передано отрицательное количество товара");
                return;
            }

            if (_shop.CheckAvailabilityOnWarehouse(good, count))
            {
                throw new ArgumentException("Невозможно добавить данную покупку в корзину");
            }

            if (_goods.ContainsKey(good) == false)
            {
                _goods.Add(good, count);
            }
            else
            {
                _goods[good] += count;
            }
        }

        public void ShowGoods()
        {
            foreach (var item in _goods)
            {
                Console.WriteLine($"{item.Key.Name} - {item.Value} шт.");
            }
        }

        public Order Order()
        {
            _shop.Sell(_goods);
            return new Order();
        }
    }

    class Order
    {
        public string Paylink { get; private set; } = "Paylink";
    }

    class Shop
    {
        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        private Warehouse _warehouse;

        public Cart Cart()
        {
            return new Cart(this);
        }

        public bool CheckAvailabilityOnWarehouse(Good good, int count)
        {
            if (_warehouse.CheckGoods(good, count))
                return true;
            else
                return false;
        }

        public void Sell(Dictionary<Good, int> goods)
        {
            foreach (var item in goods)
            {
                _warehouse.TransferGood(item.Key, item.Value);
            }
        }
    }
}
