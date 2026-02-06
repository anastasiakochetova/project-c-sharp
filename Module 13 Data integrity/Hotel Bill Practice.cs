using System;

namespace HotelAccounting;

public class AccountingModel : ModelBase
{
    private double price;
    private int nightsCount;
    private double discount;
    private double total;
    private bool isUpdating;

    public double Price
    {
        get => price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price must be non-negative");
            
            if (Math.Abs(price - value) < 1e-10) return;
            
            price = value;
            Notify(nameof(Price));
            UpdateTotal();
        }
    }

    public int NightsCount
    {
        get => nightsCount;
        set
        {
            if (value <= 0)
                throw new ArgumentException("NightsCount must be positive");
            
            if (nightsCount == value) return;
            
            nightsCount = value;
            Notify(nameof(NightsCount));
            UpdateTotal();
        }
    }

    public double Discount
    {
        get => discount;
        set
        {
            if (Math.Abs(discount - value) < 1e-10) return;
            
            discount = value;
            Notify(nameof(Discount));
            UpdateTotal();
        }
    }

    public double Total
    {
        get => total;
        set
        {
            if (value < 0)
                throw new ArgumentException("Total must be non-negative");
            
            if (Math.Abs(total - value) < 1e-10) return;
            
            if (isUpdating) return;
            
            isUpdating = true;
            try
            {
                total = value;
                Notify(nameof(Total));
                UpdateDiscountFromTotal();
            }
            finally
            {
                isUpdating = false;
            }
        }
    }

    private void UpdateTotal()
    {
        if (isUpdating) return;
        
        isUpdating = true;
        try
        {
            double newTotal = price * nightsCount * (1 - discount / 100.0);
            
            if (newTotal < 0)
                throw new ArgumentException("Total would become negative with given values");
            
            if (Math.Abs(total - newTotal) > 1e-10)
            {
                total = newTotal;
                Notify(nameof(Total));
            }
        }
        finally
        {
            isUpdating = false;
        }
    }

    private void UpdateDiscountFromTotal()
    {
        double product = price * nightsCount;
        
        if (Math.Abs(product) < 1e-10)
            throw new ArgumentException("Cannot calculate discount when Price * NightsCount is zero");
        
        double newDiscount = (1 - total / product) * 100.0;
        
        if (Math.Abs(discount - newDiscount) > 1e-10)
        {
            discount = newDiscount;
            Notify(nameof(Discount));
        }
    }
}
