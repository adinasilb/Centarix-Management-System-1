﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CommentTypesProc : ApplicationDbContextProc<CommentType>
    {
        public CommentTypesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public IQueryable<CommentType> Read(List<Expression<Func<CommentType, object>>> includes = null)
        {
            var comments = _context.CommentTypes.AsNoTracking().AsQueryable();
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    comments = comments.Include(t);
                }
            }
            return comments;
        }
    }
}