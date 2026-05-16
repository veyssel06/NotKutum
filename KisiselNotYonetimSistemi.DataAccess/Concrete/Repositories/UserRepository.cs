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
    public class UserRepository : IUserDal
    {
        Context _context = new Context();
        DbSet<User> _object;

        public UserRepository()
        {
            _object = _context.Users;
        }

        public void Add(User user)
        {
            _object.Add(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _object.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _object.ToList();
        }

        public List<User> GetAll(Func<User, bool> filter)
        {
            return _object.Where(filter).ToList();
        }


        public User GetById(int id)
        {
            return _object.Find(id);
        }

        public void Update(User user)
        {
            _object.Update(user);
            _context.SaveChanges();
        }
    }
}
