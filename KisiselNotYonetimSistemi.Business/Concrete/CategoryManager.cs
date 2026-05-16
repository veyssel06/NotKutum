using KisiselNotYonetimSistemi.DataAccess.Concrete.Repositories;
using KisiselNotYonetimSistemi.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.Business.Concrete
{
    public class CategoryManager
    {
        CategoryRepository _categoryRepository = new CategoryRepository();

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public List<Category> GetAll(Func<Category, bool> filter)
        {
            return _categoryRepository.GetAll(filter);
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public int Add(Category category)
        {
            _categoryRepository.Add(category);
            return 1;
        }

        public int Update(Category category)
        {
            if (category.CategoryName == "" || category.CategoryName == null || category.CategoryName.Length < 3)
            {
                return -1;
            }
            _categoryRepository.Update(category);
            return 1;
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }
    }
}
