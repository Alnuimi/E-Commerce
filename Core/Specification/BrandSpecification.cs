using System;
using Core.Entities;

namespace Core.Specification;

public class BrandSpecification:BaseSpecification<Product,string>
{
    public BrandSpecification()
    {
        AddSelect(x=>x.Brand);
        ApplyDistinct();
    }
}
