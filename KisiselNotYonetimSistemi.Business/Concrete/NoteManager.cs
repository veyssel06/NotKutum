using KisiselNotYonetimSistemi.DataAccess.Concrete.Repositories;
using KisiselNotYonetimSistemi.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.Business.Concrete
{
    public class NoteManager
    {
        NoteRepository _noteRepository = new NoteRepository();

        public List<Note> GetAll()
        {
            return _noteRepository.GetAll();
        }

        public List<Note> GetAll(Func<Note, bool> filter)
        {
            return _noteRepository.GetAll(filter);
        }

        public Note GetById(int id)
        {
            return _noteRepository.GetById(id);
        }

        public int Add(Note note)
        {
            if (note.Title == "" || note.Title == null || note.Title.Length < 3)
            {
                return -1;
            }
            _noteRepository.Add(note);
            return 1;
        }

        public int Update(Note note)
        {
            if (note.Title == "" || note.Title == null || note.Title.Length < 3)
            {
                return -1;
            }
            _noteRepository.Update(note);
            return 1;
        }

        public void Delete(Note note)
        {
            _noteRepository.Delete(note);
        }
    }
}
