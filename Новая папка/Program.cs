using System;
using System.Collections.Generic;

namespace ConsoleApp18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player(50);
            Seller seller = new Seller(1000);
            Shop shop = new Shop();

            shop.Work(player, seller);
        }
    }


    class Product
    {
        public Product(int id, string name, string description, int price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public int Id { get; private set; }
        public int Price { get; private set; } 
    }

    class Cell
    {
        public Cell(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public Product Product { get; private set; }
        public int Amount { get; private set; }
        public int TotalPrice => Amount * Product.Price;

        public void Decrease(int amount)
        {
            if (Amount - amount >= 0)
                Amount -= amount;
        }

        public void Increase(int amount)
        {
            Amount += amount;
        }
    }

    class Character
    {
        protected List<Cell> Inventory;

        public Character(int money)
        {
            Inventory = new List<Cell>();
            Money = money;
        }

        public int Money { get; protected set; }

        public void ShowInventory()
        {
            foreach (Cell cell in Inventory)
            {
                Console.WriteLine($"В инвентаре находится {cell.Product.Name} в количестве {cell.Amount},его описание: {cell.Product.Description}, стоимость одной единицы товара {cell.Product.Price}");
            }

            Console.WriteLine($"Количество денег {Money}");
            Console.WriteLine();
        }
    }

    class Player : Character
    {
        public Player(int money) : base(money)
        {
            Inventory.Add(new Cell(new Product(1, "Меч новичка", "Меч, которым можно убить только слизня", 100), 1));
        }

        public void BuyProduct(Cell purchase)
        {
            bool isContain = false;

            foreach (Cell cell in Inventory)
            {
                if (cell.Product.Name == purchase.Product.Name)
                {
                    isContain = true;
                    cell.Increase(purchase.Amount);
                    break;
                }
            }

            if (isContain == false)
            {
                Inventory.Add(purchase);
            }

            Console.WriteLine($"Товар {purchase.Product.Name} в количестве {purchase.Amount}");
            Money -= purchase.TotalPrice;
        }

        public bool CanPay(Cell product, int amount)
        {
            return Money >= product.Product.Price * amount;
        }
    }

    class Seller : Character
    {
        public Seller(int money) : base(money)
        {
            Inventory.Add(new Cell(new Product(1, "Зелье здоровья", "Стандартное зелье здоровья", 10), 30));
            Inventory.Add(new Cell(new Product(2, "Зелье маны", "Стандартное зелье маны", 20), 15));
            Inventory.Add(new Cell(new Product(3, "Меч новичка", "Меч, которым можно убить только слизня", 150), 2));
        }

        public void Sell(Cell purchase)
        {
            foreach (Cell cell in Inventory)
            {
                if (cell.Product.Name == purchase.Product.Name)
                {
                    cell.Decrease(purchase.Amount);
                    break;
                }
            }

            Money += purchase.TotalPrice;
        }

        public bool TrySell(int id, int amount, out Cell purchace)
        {
            purchace = null;

            if (id < 0 || id >= Inventory.Count)
            {
                Console.WriteLine("У торговца нет такого лота");
                return false;
            }

            foreach (Cell cell in Inventory)
            {
                if (cell.Product.Id == id)
                {
                    if (amount > 0 && amount <= cell.Amount)
                    {
                        purchace = new Cell(cell.Product, amount);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("У торговца нет столько данного товара");
                        return false;
                    }
                }
            }

            return false;
        }
    }

    class Shop
    {
        public void Work(Player player, Seller seller)
        {
            bool isContinue = true;

            while (isContinue)
            {
                const string SwowProductsSellerCommand = "1";
                const string SwowInventoryCommand = "2";
                const string BuyProductCommand = "3";
                const string ExitCommand = "4";

                Console.WriteLine("Введите необходимую команду: ");
                Console.WriteLine($"{SwowProductsSellerCommand} - посмотреть товар торговца");
                Console.WriteLine($"{SwowInventoryCommand} - посмотреть инвентарь");
                Console.WriteLine($"{BuyProductCommand} - купить товар");
                Console.WriteLine($"{ExitCommand} - выход");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case SwowProductsSellerCommand:
                        seller.ShowInventory();
                        break;

                    case SwowInventoryCommand:
                        player.ShowInventory();
                        break;

                    case BuyProductCommand:
                        Trade(player, seller);
                        break;

                    case ExitCommand:
                        isContinue = false;
                        break;

                    default:
                        Console.WriteLine("Введена неверная команда!");
                        break;
                }
            }
        }

        private void Trade(Player player, Seller seller)
        {
            Console.WriteLine("Введите лот товара, который желаете купить: ");
            int id = UserUtils.ReadNumber();
            Console.WriteLine("Введите количество товара, который желаете купить: ");
            int amount = UserUtils.ReadNumber();

            if (seller.TrySell(id, amount, out Cell purchace))
            {
                if (player.CanPay(purchace, amount))
                {
                    player.BuyProduct(purchace);
                    seller.Sell(purchace);
                }
                else
                {
                    Console.WriteLine("У вас недостаточно денег");
                }
            }
        }
    }

    class UserUtils
    {
        public static int ReadNumber() 
        {
            int result;

            do
            {
                Console.Write("Введите число: ");
            } while (int.TryParse(Console.ReadLine(), out result) == false);

            return result;
        }
    }
}