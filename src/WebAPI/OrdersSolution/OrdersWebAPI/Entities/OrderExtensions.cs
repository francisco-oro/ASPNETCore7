namespace OrdersWebAPI.Entities
{
    public static class OrderExtensions
    {
        public static double? UpdateTotalAmount(this Order order)
        {

            if (order.OrderItems != null)
            {
                order.TotalAmount = (float)order.OrderItems.Sum(temp => temp.TotalPrice);
                return order.TotalAmount;
            }
            return null;
        }
    }
}
