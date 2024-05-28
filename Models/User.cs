using System;
using System.Collections.Generic;

namespace Practice.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string UserLogin { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public int IdRole { get; set; }

    public virtual ICollection<Florist> Florists { get; set; } = new List<Florist>();

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
