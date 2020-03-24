using Project.BLL.DesignPatterns.RepositoryPattern.BaseRep;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.RepositoryPattern.ConcRep
{
    public class PARepository:BaseRepository<ProductAttribute>
    {
        public override void Update(ProductAttribute item)
        {
            ProductAttribute guncellenecek = Default(x => x.ProductID == item.ProductID && x.AttributeID == item.AttributeID);
            item.Status = MODEL.Enums.DataStatus.Updated;
            item.ModifiedDate = DateTime.Now;
            db.Entry(guncellenecek).CurrentValues.SetValues(item);
            Save();
        }
    }
}
