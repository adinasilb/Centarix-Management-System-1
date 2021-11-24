using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ProductComment :CommentBase
    {
        [ForeignKey("ObjectID")]
        public Product Product { get; set; }
        public override AppUtility.CommentModelTypeEnum ModelType
        {
            get { return AppUtility.CommentModelTypeEnum.Product; }

        }
    }
}
