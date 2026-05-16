using KisiselNotYonetimSistemi.DataAccess.Abstract;
using KisiselNotYonetimSistemi.Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.DataAccess.Concrete.Repositories
{
    public class CategoryRepository : ICategoryDal
    {
        Context _context = new Context();
        DbSet<Category> _object;

        public CategoryRepository()
        {
            _object = _context.Categories;
        }

        public void Add(Category category)
        {
            _object.Add(category);
            _context.SaveChanges();
        }

        public void Delete(Category category)
        {
            _object.Remove(category);
            _context.SaveChanges();
        }

        public List<Category> GetAll()
        {
            return _object.ToList();
        }

        public List<Category> GetAll(Func<Category, bool> filter)
        {
            return _object.Where(filter).ToList();
        }

        public Category GetById(int id)
        {
            return _object.Find(id);
        }

        public void Update(Category category)
        {
            _object.Update(category);
            _context.SaveChanges();
        }
    }
}
