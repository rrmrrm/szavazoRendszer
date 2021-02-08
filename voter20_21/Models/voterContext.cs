using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models 
{
    public class voterContext : DbContext
    {
        //public DbSet<VotingsAssignedToUserViewModel> activeVotes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AssignedUser> _AssignedUsers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public voterContext(DbContextOptions<voterContext> options)
            : base(options)
        {
        }
    }
}
