using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CommentTypeData
    {
        public static List<CommentType> Get()
        {
            List<CommentType> list = new List<CommentType>();
            list.Add(new CommentType
            {
                TypeID = 1,
                Description = "Comment",
                Icon = "icon-comment-24px",
                DescriptionEnum = AppUtility.CommentTypeEnum.Comment.ToString(),
                Color = "#30BCC9"
            });
            list.Add(new CommentType
            {
                TypeID= 2,
                Description = "Warning",
                Icon = "icon-report_problem-24px",
                DescriptionEnum = AppUtility.CommentTypeEnum.Warning.ToString(),
                Color = " var(--danger-color)"
            });
         
            return list;
        }
             
    }
}
