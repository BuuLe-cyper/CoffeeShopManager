﻿using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<bool> SoftDeleteCategoryEntity(int categoryID);
    }
}
