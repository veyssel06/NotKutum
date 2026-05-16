using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisiselNotYonetimSistemi.Entity.Concrete
{
    public class Note
    {
        public int NoteID { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserID { get; set; }
        public int? CategoryID { get; set; }
        public bool NoteStatus { get; set; }
        public bool IsArchived { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsPinned { get; set; } = false;
        public User User { get; set; }
        public Category Category { get; set; }
    }
}
