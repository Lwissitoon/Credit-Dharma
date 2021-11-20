﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManager.Models;

namespace Credit_Dharma.Data
{
    public class Credit_DharmaContext : DbContext
    {
        public Credit_DharmaContext (DbContextOptions<Credit_DharmaContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserManager.Models.Usuario> Usuario { get; set; }
    }
}