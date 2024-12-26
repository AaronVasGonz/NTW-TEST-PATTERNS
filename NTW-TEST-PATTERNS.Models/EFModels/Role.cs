﻿using Models.EFModels;
using System;
using System.Collections.Generic;

namespace Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
