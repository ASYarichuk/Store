class Good
{
    private string _name;

    public string Name => _name;

    public Good(string name)
    {
        if (name == null)
        {
            Console.WriteLine("Неправильное имя товара");
            return;
        }

        _name = name;
    }
}

class Cell
{
    private int _count;
    private Good _good;

    public Cell(string nameGood, int count)
    {
        if (nameGood == null)
            Console.WriteLine("Неправильное имя");
            return;
        
        _good = new Good(nameGood);

        _count = count;

        if(_count < 0)
        {
            _count = 0;
        }
    }

    public void AddCount(int count)
    {
        if (count < 0)
        {
            Console.WriteLine("Отрицательное количество");
        }

        _count += count;
    }

    public void DeleteCount(int count)
    {
        _count -= count;

        if (_count < 0)
        {
            Console.WriteLine("Количество товара ушло в минус");
        }
    }
}

class Warehouse
{
    private List<Cell> _goods = new();

    public int CountCell()
    {
        return _goods.Count;
    }

    public void Delive(string name, int count)
    {
        if (name == null)
        {
            Console.WriteLine("Передано нулевое имя");
            return;
        }

        if (count < 0)
        {
            Console.WriteLine("Передано отрицательное количество товара");
            return;
        }

        string currentGood = null;
        int indexCell = 0;

        ResearchGood();

        if (currentGood != null)
        {
            _goods[indexCell].AddCount(count);
        }
        else
        {
            _goods.Add(new Cell(name, count));
        }
    }

    public void ReduceCountGood()
    {
        string currentGood = null;
        int indexCell = 0;

        ResearchGood();

        _goods.DeleteCount();
    }

    private void ResearchGood(out string currentGood, out int indexCell)
    {
        currentGood = null;
        indexCell = 0;

        for (int i = 0; i < _goods.Count; i++)
        {
            if (_goods[i].name == name)
            {
                currentGood = name;
                indexCell = i;
                break;
            }
        }
    }
}

class Cart
{
    private List<Cell> _goods = new List<Cell>();

    public void AddGood(Good good, int count)
    {
        if (good == null)
        {
            Console.WriteLine("Передан неверный товар");
            return;
        }

        if (count < 0)
        {
            Console.WriteLine("Передано отрицательное количество товара");
            return;
        }

        string currentGood = null;
        int indexCell = 0;

        for (int i = 0; i < _goods.Count; i++)
        {
            if (_goods[i].name == name)
            {
                currentGood = name;
                indexCell = i;
                break;
            }
        }

        if (currentGood != null)
        {
            _goods[indexCell].AddCount(count);
        }
        else
        {
            _goods.Add(new Cell(name, count));
        }
    }

    public void ShowGoods()
    {
        for (int i = 0; i < _goods.Count; i++)
        {
            Console.WriteLine(_goods[i].name - _goods[i].count);
        }
    }
}

class Store
{
    private Warehouse _warehouse = new();

    warehouse.Delive(iPhone12, 10);
    warehouse.Delive(iPhone11, 1);

    public void ShowCoodsWarehouse()
    {
        for (int i = 0; i < _warehouse.CountCell(); i++)
        {
            Console.WriteLine(_warehouse[i].name - _warehouse[i].count);
        }
    }

    public bool CheckNameAndAmount(string name, int count)
    {
        if (name == null)
        {
            Console.WriteLine("Передан неверный товар");
            return;
        }

        if (count < 0)
        {
            Console.WriteLine("Передано отрицательное количество товара");
            return;
        }

        string currentGood = null;
        int indexCell = 0;

        for (int i = 0; i < _goods.Count; i++)
        {
            if (_goods[i].name == name)
            {
                currentGood = name;
                indexCell = i;
                break;
            }
        }

        if (currentGood != null)
        {
            if (_goods[indexCell].count - count >= 0)
            {
                _goods[indexCell].DeleteCount(count);
                _warehouse.ReduceCountGood();
                return true;
            }
            else
            {
                Console.WriteLine("На складе нет такого количества данного товара");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Такого товара нет на складе");
            return false;
        }
    }
}

class Buyer
{
    private Cart _cart = new Cart();
    private Store _store = new Store();

    private void TryAddGood(string name, int count)
    {
        if (name == null)
        {
            Console.WriteLine("Передан неверный товар");
            return;
        }

        if (count < 0)
        {
            Console.WriteLine("Передано отрицательное количество товара");
            return;
        }

        if (_store.CheckNameAndAmount(name, count))
        {
            _cart.AddGood(name, count);
        }
        else
        {
            Console.WriteLine("Купить не получилось");
        }
    }

    TryAddGood(iPhone12, 4);
    TryAddGood(iPhone11, 3);

    _cart.ShowGoods();

    Console.WriteLine(cart.Order().Paylink);

    TryAddGood(iPhone12, 9);
}
