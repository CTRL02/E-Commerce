using E_Commerce.Models;
using Microsoft.Build.Evaluation;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }



    public Category Category { get; set; }
    public ICollection<ProductItem> ProductItems { get; set; }


}
