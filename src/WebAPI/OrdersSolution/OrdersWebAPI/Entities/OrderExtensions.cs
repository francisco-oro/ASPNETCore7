namespace OrdersWebAPI.Entities
{
    public static class OrderExtensions
    {
        public static float UpdateTotalAmount(this Order order)
        {
            if (order.OrderItems != null) return (float)order.OrderItems.Sum(temp => temp.TotalPrice);
            return 0;
        }
    }
}
