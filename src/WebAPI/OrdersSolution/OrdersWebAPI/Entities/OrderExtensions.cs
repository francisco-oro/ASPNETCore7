namespace OrdersWebAPI.Entities
{
    public static class OrderExtensions
    {
        public static double? UpdateTotalAmount(this Order order)
        {

            if (order.OrderItems != null)
            {
                order.TotalAmount = 0;
                foreach (var orderOrderItem in order.OrderItems)
                {
                    order.TotalAmount += orderOrderItem.TotalPrice;
                }
                return order.TotalAmount;
            }
            return null;
        }
    }
}
