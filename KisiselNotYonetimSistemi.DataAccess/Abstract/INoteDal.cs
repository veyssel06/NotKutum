using KisiselNotYonetimSistemi.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.DataAccess.Abstract
{
    public interface INoteDal
    {
        List<Note> GetAll();
        List<Note> GetAll(Func<Note, bool> filter);
        Note GetById(int id);
        void Add(Note note);
        void Update(Note note);
        void Delete(Note note);
    }
}
