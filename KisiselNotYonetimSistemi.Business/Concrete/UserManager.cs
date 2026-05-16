using KisiselNotYonetimSistemi.DataAccess.Concrete.Repositories;
using KisiselNotYonetimSistemi.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.Business.Concrete
{
    public class UserManager
    {
        UserRepository _userRepository = new UserRepository();

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public List<User> GetAll(Func<User, bool> filter)
        {
            return _userRepository.GetAll(filter);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void Add(User user)
        {
            _userRepository.Add(user);
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }
    }
}
