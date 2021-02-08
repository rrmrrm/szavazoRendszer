using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voter20_21.Models
{
    public class AnswerStat{
        public String text { get; set; }
        public Int32 voteChosenNum { get; set; }
        public Double voteChosenPercent { get; set; }
    }
    public class VotingStat
    {
        public Int32 votingId { get; set; }
        public Int32 participantsNum { get; set; }
        public Double participantsPercent { get; set; }

        public ICollection<AnswerStat> votingAnswerStats { get; set; }

        public VotingStat()
        {
            votingAnswerStats = new List<AnswerStat>();
        }
    }
}
