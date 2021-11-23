﻿using Asset.Models;
using Asset.ViewModels.OriginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.Domain.Repositories
{
   public interface IClassificationRepository
    {
        IEnumerable<Classification> GetAll();
         Classification GetById(int id);
        int Add(Classification classObj);
        int Update(Classification classObj);
        int Delete(int id);
    }
}
