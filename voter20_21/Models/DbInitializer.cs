using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security;
using System.Security.Cryptography;

namespace voter20_21.Models
{
    public static class DbInitializer
    {
        //TODO: ER diagram abrat frissiteni
        
        public static void Initialize(IServiceProvider provider, string imageDirectory)
        {
            voterContext vC = provider.GetRequiredService<voterContext>();
            /*
            Adatbázis létrehozása (amennyiben nem létezik), illetve a migrációk alapján a frissítése.
            Amennyiben teljesen el szeretnénk vetni a jelenlegi adatbázisunk, akkor a
            törléshez az Sql Server Object Explorer ablak használható a Visual Studioban.
            Itt SQL Server > (localdb)\\MSSQLLocalDB útvonalon találjuk az adatbázisainkat.
            */
            vC.Database.EnsureDeleted();
            vC.Database.Migrate();

            if (vC.Users.Any())
            {
                return;
            };
            /*
            ICollection<AssignedUser> defaultAssignedUsers= new List<AssignedUser>
            {
                        new AssignedUser { votingId = 1, userId = 1, Voted = false},
                        new AssignedUser { votingId = 1, userId = 2, Voted = true },
                        new AssignedUser { votingId = 1, userId = 3, Voted = true },


                        new AssignedUser { votingId = 2, userId = 1, Voted = true },
                        new AssignedUser { votingId = 2, userId = 2, Voted = false},
                        new AssignedUser { votingId = 2, userId = 3 },
                        new AssignedUser { votingId = 2, userId = 4, Voted = true },

                        new AssignedUser { votingId = 3, userId = 7, Voted = true },
                        new AssignedUser { votingId = 3, userId = 8, Voted = true },
                        new AssignedUser { votingId = 3, userId = 9, Voted = true }
            };
            foreach (var assignedUser in defaultAssignedUsers)
            {
                vC._AssignedUsers.Add(assignedUser);
            }
            */

            ICollection<User> defaultUsers;
            using (SHA512CryptoServiceProvider crypto = new SHA512CryptoServiceProvider())
            {
                defaultUsers = new List<User>{
                    new User {email = "neil.collings@gmail.com",           password = crypto.ComputeHash(Encoding.UTF8.GetBytes("asdfgh"))},
                    new User {email = "brucecastor@gmail.com",             password = crypto.ComputeHash(Encoding.UTF8.GetBytes("123456"))},
                    new User {email = "arthurkatkinson@gmail.com",         password = crypto.ComputeHash(Encoding.UTF8.GetBytes("234567"))},
                    new User {email = "alexanderPrinceYugoslav@gmail.com", password = crypto.ComputeHash(Encoding.UTF8.GetBytes("iLikeMasonry"))},
                    new User {email = "ras.putin@gmail.com",               password = crypto.ComputeHash(Encoding.UTF8.GetBytes("didHeReallyVX_A-232Sarin"))},

                    new User {email = "bobdole@gmail.com",                 password = crypto.ComputeHash(Encoding.UTF8.GetBytes("asdfgh")) },
                    new User {email = "IvanMaslennikov@gmail.com",         password = crypto.ComputeHash(Encoding.UTF8.GetBytes("asdfgh")) },
                    new User {email = "ValentinSavitsky@gmail.com",        password = crypto.ComputeHash(Encoding.UTF8.GetBytes("asdfgh")) },
                    new User {email = "VasilyArkhipov@gmail.com",          password = crypto.ComputeHash(Encoding.UTF8.GetBytes("62Thanks_-sincerely_K19_B59_and_everyone")) },
                    new User {email = "testUser1@alma.com",        password = crypto.ComputeHash(Encoding.UTF8.GetBytes("123456")) },

                    new User {email = "testUser2@alma.com",        password = crypto.ComputeHash(Encoding.UTF8.GetBytes("123456")) },
                    new User {email = "testUser3@alma.com",        password = crypto.ComputeHash(Encoding.UTF8.GetBytes("123456")) },
                };
            }
            foreach (var user in defaultUsers)
            {
                vC.Users.Add(user);
            }
            vC.SaveChanges();
            ICollection<Voting> defaultVotings = new List<Voting>
            {
                new Voting{
                    creatorUserId = defaultUsers.ElementAt(2).Id,
                    start = new DateTime(2010,1,1, 6,0,0),
                    end = new DateTime(2028,3,1, 6,0,0),
                    question = "what does the fox say",
                    answers = new List<Answer>
                    {
                        new Answer{text = "ding-ding-ding-dingaring"},
                        new Answer{text = "both cat, and dog noises"},
                        new Answer{text = "meowoof"} 
                    },
                    assignedUsers = new List<AssignedUser>{
                        new AssignedUser {userId = defaultUsers.ElementAt(0).Id, Voted = false},
                        new AssignedUser {userId = defaultUsers.ElementAt(1).Id, Voted = true },
                        new AssignedUser {userId = defaultUsers.ElementAt(2).Id, Voted = true } 
                    }
                },
                new Voting{
                    creatorUserId = defaultUsers.ElementAt(3).Id,
                    start = new DateTime(2021,1,1, 6,0,0),
                    end = new DateTime(2024,10,1, 12,0,0),
                    question = "who should win next US election?",
                    answers = new List<Answer>
                    {
                        new Answer{text = "putin"},
                        new Answer{text = "biden"},
                        new Answer{text = "Lincler"}
                    },
                    assignedUsers = new List<AssignedUser>{
                        new AssignedUser {userId = defaultUsers.ElementAt(0).Id, Voted = true },
                        new AssignedUser {userId = defaultUsers.ElementAt(1).Id, Voted = false},
                        new AssignedUser {userId = defaultUsers.ElementAt(2).Id },
                        new AssignedUser {userId = defaultUsers.ElementAt(3).Id, Voted = true }
                    }
                },
                new Voting
                {
                    creatorUserId = defaultUsers.ElementAt(6).Id,
                    start = new DateTime(1962, 10, 27,  17, 0, 0),
                    end = new DateTime(1962, 10, 27,  20, 51, 0),
                    question = "Should we nuke the americans?",
                    answers = new List<Answer>
                    {
                        new Answer{text = "da."},
                        new Answer{text = "het, i think is a bad idea."},
                        new Answer{text = "cyka blyat lets nuke 'em."}
                    },
                    assignedUsers = new List<AssignedUser>{
                        new AssignedUser {userId = defaultUsers.ElementAt(6).Id, Voted = true },
                        new AssignedUser {userId = defaultUsers.ElementAt(7).Id, Voted = true },
                        new AssignedUser {userId = defaultUsers.ElementAt(8).Id, Voted = true }
                    }
                },
                new Voting
                {
                    creatorUserId = defaultUsers.ElementAt(2).Id,
                    start = new DateTime(1962, 10, 27,  17, 0, 0),
                    end = new DateTime(2962, 10, 27,  20, 51, 0),
                    question = "Test voting",
                    answers = new List<Answer>
                    {
                        new Answer{text = "ich habe keine katze"},
                        new Answer{text = "nem"},
                        new Answer{text = "igen"},
                        new Answer{text = "talán"}
                    },
                    assignedUsers = new List<AssignedUser>{
                        new AssignedUser {userId = defaultUsers.ElementAt(0).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(1).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(2).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(3).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(4).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(5).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(6).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(7).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(8).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(9).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(10).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(11).Id},
                    }
                },
                new Voting
                {
                    creatorUserId = defaultUsers.ElementAt(9).Id,
                    start = DateTime.Now.AddSeconds(15),
                    end = DateTime.Now.AddSeconds(40),
                    question = "gyorsan lejaro tesztszaavazas",
                    answers = new List<Answer>
                    {
                        new Answer{text = "random valasz"},
                        new Answer{text = "nem"},
                        new Answer{text = "igen"},
                        new Answer{text = "talán"}
                    },
                    assignedUsers = new List<AssignedUser>{
                        new AssignedUser {userId = defaultUsers.ElementAt(0).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(1).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(2).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(3).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(4).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(5).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(6).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(7).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(8).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(9).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(10).Id},
                        new AssignedUser {userId = defaultUsers.ElementAt(11).Id},
                    }
                }
            };
            foreach(var voting in defaultVotings)
            {
                vC.Votings.Add(voting);
            }
            vC.SaveChanges();

            ICollection<Vote> defaultVotes = new List<Vote>
            {
                new Vote{ votingId = defaultVotings.ElementAt(0).Id, answerId = defaultVotings.ElementAt(0).answers.ElementAt(0).Id},
                new Vote{ votingId = defaultVotings.ElementAt(0).Id, answerId = defaultVotings.ElementAt(0).answers.ElementAt(2).Id},

                new Vote{ votingId = defaultVotings.ElementAt(1).Id, answerId = defaultVotings.ElementAt(1).answers.ElementAt(2).Id},
                new Vote{ votingId = defaultVotings.ElementAt(1).Id, answerId = defaultVotings.ElementAt(1).answers.ElementAt(0).Id},

                new Vote{ votingId = defaultVotings.ElementAt(2).Id, answerId = defaultVotings.ElementAt(2).answers.ElementAt(0).Id},
                new Vote{ votingId = defaultVotings.ElementAt(2).Id, answerId = defaultVotings.ElementAt(2).answers.ElementAt(0).Id},
                new Vote{ votingId = defaultVotings.ElementAt(2).Id, answerId = defaultVotings.ElementAt(2).answers.ElementAt(1).Id}
            };
            foreach (var vote in defaultVotes)
            {
                vC.Votes.Add(vote);
            }
            vC.SaveChanges();
        }
    }
}
