using KisiselNotYonetimSistemi.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.DataAccess.Abstract
{
    public interface ICategoryDal
    {
        List<Category> GetAll();
        List<Category> GetAll(Func<Category, bool> filter);
        Category GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
