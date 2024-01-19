namespace BizLand.ViewModels
{
    public class PaginationVM<T> where T : class,new()
    {
        public ICollection<T> Items { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }
}
