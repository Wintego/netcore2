namespace WebStore.Domain.ViewModels.Order
{
    public class UserOrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public decimal TotalSum { get; set; }
    }
}
