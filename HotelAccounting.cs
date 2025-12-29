using System;

namespace HotelAccounting;

public class AccountingModel : ModelBase
{
    private double price;
    private int nightsCount = 1;
    private double discount;

    public double Price
    {
        get { return price; }
        set
        {
            if (value < 0) 
                throw new ArgumentException("Цена должна быть неотрицательной");
            if (value * nightsCount * (1 - discount / 100) < 0) 
                throw new ArgumentException("Итого должно быть неотрицательным");

            if (price != value)
            {
                price = value;
                Notify(nameof(Price));
                Notify(nameof(Total));
            }
        }
    }

    public int NightsCount
    {
        get { return nightsCount; }
        set
        {
            if (value <= 0) 
                throw new ArgumentException("Количество ночей должно быть положительным");
            if (price * value * (1 - discount / 100) < 0) 
                throw new ArgumentException("Итого должно быть неотрицательным");

            if (nightsCount != value)
            {
                nightsCount = value;
                Notify(nameof(NightsCount));
                Notify(nameof(Total));
            }
        }
    }

    public double Discount
    {
        get { return discount; }
        set
        {
            if (price * nightsCount * (1 - value / 100) < 0) 
                throw new ArgumentException("Итого должно быть неотрицательным");

            if (discount != value)
            {
                discount = value;
                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }
    }

    public double Total
    {
        get { return price * nightsCount * (1 - discount / 100); }
        set
        {
            if (value < 0) 
                throw new ArgumentException("Итого должно быть неотрицательным");

            if (price * nightsCount == 0)
            {
                if (value > 0) 
                    throw new ArgumentException("Итого не может быть положительным при цене или количестве ночей равном нулю");
                return;
            }

            Discount = 100 * (1 - value / (price * nightsCount));
        }
    }
}
