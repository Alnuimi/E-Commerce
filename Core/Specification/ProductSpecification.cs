using System;
using Core.Entities;

namespace Core.Specification;

public class ProductSpecification:BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParmas specParmas):base(x=>
       (string.IsNullOrEmpty(specParmas.Search) || x.Name.ToLower().Contains(specParmas.Search) ) &&
       (specParmas.Brands.Count==0 || specParmas.Brands.Contains(x.Brand) ) &&
       (specParmas.Types.Count==0 || specParmas.Types.Contains(x.Type))
      
    )
    {
        ApplyPaging(specParmas.PageSize *(specParmas.PageIndex-1),specParmas.PageSize);
        switch(specParmas.Sort)
        {
            case "priceAsc":
                AddOrderBy(x=>x.Price);
                break;
            case "priceDsc":
                AddOrderByDescending(x=>x.Price);
                break;
            default:
                AddOrderBy(x=>x.Name);
                break;
        }
       
    }
    
}
