﻿using Microsoft.AspNetCore.Identity;

namespace DotnetProjet5.Areas.Identity.Models
{
    public class AppUser : IdentityUser
    {
        // Propriétés supplémentaires pour l'utilisateur
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
