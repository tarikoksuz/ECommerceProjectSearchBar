using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MODEL.Entities
{
    public class ProductAttribute:BaseEntity
    {
        //EntityAttributeValue(EAV) modelinin sonlanacagı coka cok ilişkiyi kuran ara tablomuz (Junction table'imiz)
        public int AttributeID { get; set; }

        public int ProductID { get; set; }

        public string Value { get; set; }


        //Relational Properties


        public virtual EntityAttribute Attribute { get; set; }

        public virtual Product Product { get; set; }




    }
}
