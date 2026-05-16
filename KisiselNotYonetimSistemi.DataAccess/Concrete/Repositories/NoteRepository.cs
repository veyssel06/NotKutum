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
    public class NoteRepository : INoteDal
    {
        Context _context = new Context();
        DbSet<Note> _object;

        public NoteRepository()
        {
            _object = _context.Notes;
        }

        public void Add(Note note)
        {
            _object.Add(note);
            _context.SaveChanges();
        }

        public void Delete(Note note)
        {
            _object.Remove(note);
            _context.SaveChanges();
        }

        public List<Note> GetAll()
        {
            return _object.ToList();
        }

        public List<Note> GetAll(Func<Note, bool> filter)
        {
            return _object.Include(x => x.Category).Where(filter).ToList();
        }

        public Note GetById(int id)
        {
            return _object.Find(id);
        }

        public void Update(Note note)
        {
            _object.Update(note);
            _context.SaveChanges();
        }
    }
}