using KisiselNotYonetimSistemi.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.DataAccess.Abstract
{
    public interface IUserDal
    {
        List<User> GetAll();
        List<User> GetAll(Func<User, bool> filter);
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
