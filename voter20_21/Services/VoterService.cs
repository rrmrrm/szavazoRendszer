using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using voter20_21.Models;
using System.Security.Cryptography.X509Certificates;
namespace voter20_21.Services
{
    public class VoterService
    {
        private voterContext vC;
        public VoterService(voterContext _vC)
        {
            vC = _vC;
        }
        public User tryFindUser(string email)
        {
            return vC.Users.FirstOrDefault(u => u.email == email);
        }
        /// <summary>
        /// Azon Voting-okat adja vissza, amikhez a user hozzá van rendelve.
        /// </summary>
        /// <param name="userId">a user Id-je. A hozzá tartozó Voting-okat fogja visszaadni ez a függvény</param>
        /// <param name="includeVotingsWhereUserVoted">default értéke igaz. ha hamis, akkor csak azon szavazásokat adja vissza függvény, amikre a user még nem szavazott</param>
        /// <returns></returns>
        private IQueryable<Voting> getUserVotings(Int32 userId)
        {
            return vC._AssignedUsers.Include(a => a.voting).
                Where(a => a.userId == userId).
                Select(a => a.voting);
        }

        public List<Voting> findAssignedOpenVotingsList(Int32 userId)
        {
            return findAssignedOpenVotings(userId).ToList();
        }
        private IQueryable<Voting> findAssignedOpenVotings(Int32 userId)
        {
            DateTime now = DateTime.Now;
            return vC._AssignedUsers.Include(a => a.voting).
                Where(a => a.userId == userId && a.Voted == false).
                Select(a => a.voting).
                Where(v => v.start < now && v.end > now).
                OrderBy(v => v.end);
            
        }
        private IQueryable<Voting> closedVotings()
        {
            DateTime time = DateTime.Now;
            //megkeressük azokat az szavazásokat, amik olyan assignedUser-ekhez (is) tartoznak, akik még nem szavaztak:
            IQueryable<Voting> someoneNotVoted = vC._AssignedUsers.Include(a => a.voting).
                Where(a => a.Voted == false).
                Select(a => a.voting);

            //megkeressük azokat a szavazásokat, amikre minden résztvevő szavazott:
            IQueryable<Voting> everyoneVoted = vC.Votings.
                Except(someoneNotVoted);

            //visszaadjuk azokat a szavazásokat, amik a lejártak VAGY minden résztvevő szavazott már rájuk:
            IQueryable<Voting> ret = vC._AssignedUsers.Include(a => a.voting).
                Where(a => a.voting.end < time).
                Select(a => a.voting).
                Union(everyoneVoted);
            return ret;
        }

        public Boolean isFilterValid(VotingFilter filter)
        {
            //data annotációk ellenőrzése:
            if (!Validator.TryValidateObject(filter, new ValidationContext(filter, null, null), null))
            {
                return false;
            }
            return true;
        }

        public List<Voting> findAssignedClosedVotingsList(Int32 userId, VotingFilter _filter = null /*String? title, DateTime? minStart, DateTime? maxEnd*/)
        {
            return findAssignedClosedVotings(userId, _filter).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="_filter">az eredmény Votingokat ezzel a kifejezéssel szűri az eljárás. (default értéke null). Ha null, akkor nem hajtunk végre szűrést.</param>
        /// <returns></returns>
        private IQueryable<Voting> findAssignedClosedVotings(Int32 userId, VotingFilter _filter = null /*String? title, DateTime? minStart, DateTime? maxEnd*/)
        {
            DateTime time = DateTime.Now;
            //megkeressük azokat az szavazásokat, amik olyan assignedUser-ekhez (is) tartoznak, akik még nem szavaztak:
            IQueryable<Voting> someoneNotVoted = vC._AssignedUsers.Include(a => a.voting).
                Where(a => a.Voted == false).
                Select(a => a.voting);
            //megkeressük azokat a szavazásokat, amikre minden résztvevő szavazott:
            IQueryable<Voting> everyoneVoted = vC._AssignedUsers.Include(a => a.voting).
                Select(a => a.voting).
                Except(someoneNotVoted);
            //csak azok a szavazások érdekelnek minket, amikhez a user is hozzá lett rendelve ÉS az összes szavazásban résztvevő felhasználó szavazott:
            IQueryable<Voting> everyoneVotedAndUserIsAssigned = everyoneVoted.Intersect(getUserVotings(userId));

            VotingFilter filter = null;
            if (_filter == null)
            {
                filter = new VotingFilter { maxEnd = null, minStart = null, title = null };
            }
            else
            {
                filter = _filter;
            }

            //visszaadjuk azokat a szavazásokat, amik a felhasználóra vonatkoznak ÉS { lejártak VAGY minden résztvevő szavazott már rájuk}:
            //ezen belül filter szerint szűrjük az eredményt:
            //a filter azon Voting-okat engedi át, amik tartalmazzák a filter szöveget(kis/nagy betűket nem különbözteti meg),
            // és a megadott intervallumon belül vannak(nem azokat, amik metszik ezt az intervallumot)
            //amelyik property nincs megadva a filter-ben, aszerint nem szűrünk:
            IQueryable<Voting> ret = vC._AssignedUsers.Include(a => a.voting).
                Where(a => a.userId == userId && (a.voting.end < time)).
                Select(a => a.voting).
                Union(everyoneVotedAndUserIsAssigned).
                Where(v =>
                   (filter.title == null || v.question.ToLower().Contains(filter.title.ToLower())) &&
                   (filter.minStart == null || v.start >= filter.minStart) &&
                   (filter.maxEnd == null || v.end <= filter.maxEnd)
                );
            return ret;
            /*
            //return vC.Votings.Where(v => v.creatorUserId == Id).Where(v => v.creatorUserId == Id).ToList();
            */
        }

        public Boolean isUserIsAssignedToVoting(Int32 votingId, Int32 userId)
        {
            return vC._AssignedUsers.
                Where(a => a.userId == userId).
                Where(a => a.votingId == votingId).
                Any();
        }
        public VotingStat getVotingStats(Int32 votingId, Int32 userId)
        {
            //checking whether voting is closed and the user is assigned to it:
            //TODO: lehet az aktuális filtert itt is jó lenne alkalmazni, de nem tűnik szükségesnek
            if (!findAssignedClosedVotings(userId).Any(v => v.Id == votingId))
            {
                return null;
            }
            Int32 assignedToVotingNum = vC._AssignedUsers.
                Where(a => a.votingId == votingId).
                Count();
            Int32 votedNum = vC._AssignedUsers.
                Where(a => a.Voted && a.votingId == votingId).
                Count();
            if(votedNum < 1)
            {
                var ansStat = vC.Answers.
                        Where(a => a.votingId == votingId).ToList().
                    Select(a => new AnswerStat
                    {
                        text = a.text,
                        voteChosenNum = 0,
                        voteChosenPercent = 0
                    });
                return new VotingStat
                {
                    votingId = votingId,
                    participantsNum = 0,
                    participantsPercent = 0,
                    votingAnswerStats = ansStat.ToList()
                };
            }

            var v1 = vC.Votes.
                    Include ( vote => vote.answer).
                    ThenInclude(a => a.voting).
                    Where(vote => vote.answer.voting.Id == votingId).ToList();
            var v2 = v1.GroupBy(vote => vote.answer);
            var v3 = v2.Select(group => new AnswerStat
                    {
                        text = group.Key.text,
                        voteChosenNum = group.Count(),
                        voteChosenPercent = votedNum > 0 ? 100.0 * (double)group.Count() / (double)votedNum : 0.0
                    });
            var v4 = v3.Union(
                vC.Answers.Except(//ezek az answerek már számba vannak véve az előző lekérdezésben, ezért a ebből a lekérdezésből kihagyom őket
                     vC.Votes.Include(v => v.answer).
                     Select( v => v.answer)
                    ).
                   // Include(a => a.voting).
                    Where(a => a.votingId == votingId).ToList().
                    Select(a => new AnswerStat
                    {
                        text = a.text,
                        voteChosenNum = 0,
                        voteChosenPercent = 0
                    })
                );

            return new VotingStat
            {
                votingId = votingId,
                participantsNum = votedNum,
                participantsPercent = assignedToVotingNum > 0 ? 100.0 * (double)votedNum / (double)assignedToVotingNum : 0.0,
                votingAnswerStats = v4.ToList()
            };
        }
        public List<Answer> findAnswers(Int32? votingId)
        {
            if (votingId == null)
            {
                return new List<Answer>();
            }
            return vC.Answers.Where(a => a.votingId == votingId).ToList();
        }
        //
        public Boolean Vote(Int32? userId, Int32? votingId, Int32? answerId)
        {
            if (userId == null || votingId == null || answerId == null)
            {
                return false;
            }
            DateTime now = DateTime.Now;
            //checking whether votingId is valid, and whether votingId and answerId align:
            Answer answer;
            try
            {
                answer = vC.Answers.Include(a => a.voting).Single(a => a.Id == answerId && a.votingId == votingId);
            }
            catch
            {
                return false;
            }
            //checking  whether the voting is still open
            if (answer == null || answer.voting.start >= now || answer.voting.end <= now)
            {
                return false;
            }
            //checking whether the user is assigned to the voting:
            if (vC._AssignedUsers.Where(au => au.votingId == votingId && au.userId == userId).FirstOrDefault() == null)
            {
                return false;
            }

            //frissítem az assignedUsers táblát:
            //ha itt hibát dob a single, az nagy baj, tehát nem fedem el a hibát, inkább hagyom hogy elszálljon a program.
            //TODO: beadás előtt kiszedni ezt a kommentet
            AssignedUser auReference = vC._AssignedUsers.Single(a => a.votingId == votingId && a.userId == userId);
            //ellenőrzöm, hogy nem szavazott-e még:
            if (auReference.Voted == true)
            {
                return false;
            }
            vC.Votes.Add(new Models.Vote { answerId = (int)answerId, votingId = (int)votingId });
            auReference.Voted = true;
            vC.SaveChanges();
            return true;
        }
        public List<User> getRegisteredUsers()
        {
            return vC.Users.ToList();
        }
        public void registerUser(User user)
        {
            vC.Users.Add(user);
        }
        public async Task<int> saveChangesAsync()
        {
            return await vC.SaveChangesAsync();
        }
        public void addUserToVote(User u)
        {

        }
    }
}
