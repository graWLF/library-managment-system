using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs.Branch
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
    }
}
