using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entity;

public class UserGarbage
{
    public long Id { get; set; }

    public virtual User User { get; set; }
    public virtual Garbage Garbage { get; set; }

    public DateTime ScannedDateTime { get; set; }
}