﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOS;

public class UserLoginRequest
{
    public string EmailUsername { get; set; }
    public string Password { get; set; }
}
